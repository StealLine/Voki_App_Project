using Microsoft.Extensions.Logging;
using SharedKernel.errs;
using SharedKernel.errs.utils;

namespace ApplicationShared.messaging.pipeline_behaviors;

internal static class UnitOfWorkStepHandler
{
    private static async Task<TOut> HandleWithUnitOfWork<TOut, TIn>(
        TIn msg,
        Func<TIn, CancellationToken, Task<TOut>> innerHandlerFunc,
        Func<TOut, ErrOrNothing> isOutputErr,
        Func<Err, TOut> parseErrToOutput,
        IUnitOfWorkManager unitOfWorkManager,
        ILogger logger,
        CancellationToken ct
    ) {
        await using IUnitOfWork unitOfWork = await unitOfWorkManager.Start(ct);
        logger.LogInformation("Started unit of work");

        try {
            TOut result = await innerHandlerFunc(msg, ct);

            if (isOutputErr(result).IsErr(out var err)) {
                await unitOfWork.Rollback();
                return parseErrToOutput(err);
            }

            await unitOfWork.Commit(ct);
            logger.LogDebug("Committed unit of work");

            return result;
        }
        catch (UnitOfWorkLockNotAvailableException e) {
            logger.LogInformation(
                e,
                "Unit of work failed due to lock contention for request"
            );

            await unitOfWork.Rollback();
            return parseErrToOutput(ErrFactory.Conflict(
                "Resource is currently being modified by another request. Please try again later"
            ));
        }
        catch (OperationCanceledException e) {
            logger.LogWarning(
                e,
                "Operation was canceled in the unit of work"
            );

            await unitOfWork.Rollback();
            throw;
        }
        catch (Exception ex) {
            logger.LogError(
                ex,
                "Exception was thrown in the unit of work"
            );

            await unitOfWork.Rollback();
            throw;
        }
    }

    public sealed class CommandHandler<TCommand, TResponse>
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        private readonly ICommandHandler<TCommand, TResponse> _innerHandler;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ILogger<CommandHandler<TCommand, TResponse>> _logger;

        public CommandHandler(
            ICommandHandler<TCommand, TResponse> innerHandler,
            IUnitOfWorkManager unitOfWorkManager,
            ILogger<CommandHandler<TCommand, TResponse>> logger
        ) {
            _innerHandler = innerHandler;
            _unitOfWorkManager = unitOfWorkManager;
            _logger = logger;
        }

        public async Task<ErrOr<TResponse>> Handle(TCommand command, CancellationToken ct) {
            if (command.RequireTransaction) {
                return await HandleWithUnitOfWork(
                    command,
                    _innerHandler.Handle,
                    isOutputErr: (res) => res.IsErr(out var err) ? err : ErrOrNothing.Nothing,
                    parseErrToOutput: res => res,
                    unitOfWorkManager: _unitOfWorkManager,
                    logger: _logger,
                    ct
                );
            }

            _logger.LogInformation("Handle command with no transaction because transaction is disabled");
            return await _innerHandler.Handle(command, ct);
        }
    }

    public sealed class CommandBaseHandler<TCommand>
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _innerHandler;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ILogger<CommandBaseHandler<TCommand>> _logger;

        public CommandBaseHandler(
            ICommandHandler<TCommand> innerHandler,
            IUnitOfWorkManager unitOfWorkManager,
            ILogger<CommandBaseHandler<TCommand>> logger
        ) {
            _innerHandler = innerHandler;
            _unitOfWorkManager = unitOfWorkManager;
            _logger = logger;
        }

        public async Task<ErrOrNothing> Handle(TCommand command, CancellationToken ct) {
            if (command.RequireTransaction) {
                return await HandleWithUnitOfWork(
                    command,
                    _innerHandler.Handle,
                    isOutputErr: res => res,
                    parseErrToOutput: err => err,
                    unitOfWorkManager: _unitOfWorkManager,
                    logger: _logger,
                    ct
                );
            }

            _logger.LogInformation("Handle command with no transaction because transaction is disabled");
            return await _innerHandler.Handle(command, ct);
        }
    }

    public sealed class QueryHandler<TQuery, TResponse>
        : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        private readonly IQueryHandler<TQuery, TResponse> _innerHandler;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ILogger<QueryHandler<TQuery, TResponse>> _logger;

        public QueryHandler(
            IQueryHandler<TQuery, TResponse> innerHandler,
            IUnitOfWorkManager unitOfWorkManager,
            ILogger<QueryHandler<TQuery, TResponse>> logger
        ) {
            _innerHandler = innerHandler;
            _unitOfWorkManager = unitOfWorkManager;
            _logger = logger;
        }

        public async Task<ErrOr<TResponse>> Handle(TQuery query, CancellationToken ct) {
            if (query.RequireTransaction) {
                return await HandleWithUnitOfWork(
                    query,
                    _innerHandler.Handle,
                    isOutputErr: (res) => res.IsErr(out var err) ? err : ErrOrNothing.Nothing,
                    parseErrToOutput: err => err,
                    unitOfWorkManager: _unitOfWorkManager,
                    logger: _logger,
                    ct
                );
            }

            _logger.LogInformation("Handle query with no transaction because transaction is disabled");
            return await _innerHandler.Handle(query, ct);
        }
    }
}