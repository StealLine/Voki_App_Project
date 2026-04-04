using SharedKernel.errs;

namespace ApplicationShared.messaging.pipeline_behaviors;

public interface IWithBasicValidationStep
{
    public ErrOrNothing Validate();
}

public static class BasicValidationStepHandler
{
    public sealed class CommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse> where TCommand :
        ICommand<TResponse>,
        IWithBasicValidationStep
    {
        private readonly ICommandHandler<TCommand, TResponse> _innerHandler;

        public CommandHandler(ICommandHandler<TCommand, TResponse> innerHandler) {
            _innerHandler = innerHandler;
        }

        public async Task<ErrOr<TResponse>> Handle(TCommand command, CancellationToken ct) {
            ErrOrNothing validationRes = command.Validate();
            if (validationRes.IsErr(out var err)) {
                return err;
            }

            return await _innerHandler.Handle(command, ct);
        }
    }

    public sealed class CommandBaseHandler<TCommand> : ICommandHandler<TCommand> where TCommand :
        ICommand,
        IWithBasicValidationStep
    {
        private readonly ICommandHandler<TCommand> _innerHandler;

        public CommandBaseHandler(ICommandHandler<TCommand> innerHandler) {
            _innerHandler = innerHandler;
        }

        public async Task<ErrOrNothing> Handle(TCommand command, CancellationToken ct) {
            ErrOrNothing validationRes = command.Validate();
            if (validationRes.IsErr(out var err)) {
                return err;
            }

            return await _innerHandler.Handle(command, ct);
        }
    }

    public sealed class QueryHandler<TQuery, TResponse> : IQueryHandler<TQuery, TResponse> where TQuery :
        IQuery<TResponse>,
        IWithBasicValidationStep
    {
        private readonly IQueryHandler<TQuery, TResponse> _innerHandler;

        public QueryHandler(IQueryHandler<TQuery, TResponse> innerHandler) {
            _innerHandler = innerHandler;
        }

        public async Task<ErrOr<TResponse>> Handle(TQuery query, CancellationToken ct) {
            ErrOrNothing validationRes = query.Validate();
            if (validationRes.IsErr(out var err)) {
                return err;
            }

            return await _innerHandler.Handle(query, ct);
        }
    }
}
