using SharedKernel.errs;
using SharedKernel.errs.utils;
using SharedKernel.user_ctx;

namespace ApplicationShared.messaging.pipeline_behaviors;

public interface IWithAuthCheckStep
{
    public virtual Err UnauthenticatedErr => ErrFactory.NoAccess("Access denied. Authentication required");
}

public static class AuthCheckStepExtensions
{
    public static AuthenticatedUserCtx UserCtx(
        this IWithAuthCheckStep step,
        IUserCtxProvider provider
    ) =>
        provider.Current.Match<AuthenticatedUserCtx>(
            a => a,
            _ => throw new InvalidOperationException(
                $"Trying to extract authenticated user context from unauthenticated user context in {step.GetType().Name}"
            )
        );
}

public static class AuthCheckStepHandler
{
    public sealed class CommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse> where TCommand :
        ICommand<TResponse>,
        IWithAuthCheckStep
    {
        private readonly ICommandHandler<TCommand, TResponse> _innerHandler;
        private readonly IUserCtxProvider _userCtxProvider;

        public CommandHandler(ICommandHandler<TCommand, TResponse> innerHandler, IUserCtxProvider userCtxProvider) {
            _innerHandler = innerHandler;
            _userCtxProvider = userCtxProvider;
        }

        public async Task<ErrOr<TResponse>> Handle(TCommand command, CancellationToken ct) {
            if (_userCtxProvider.Current.IsAuthenticated(out  _)) {
                return await _innerHandler.Handle(command, ct);
            }

            return command.UnauthenticatedErr;
        }
    }

    public sealed class CommandBaseHandler<TCommand> : ICommandHandler<TCommand> where TCommand :
        ICommand,
        IWithAuthCheckStep
    {
        private readonly ICommandHandler<TCommand> _innerHandler;
        private readonly IUserCtxProvider _userCtxProvider;


        public CommandBaseHandler(ICommandHandler<TCommand> innerHandler, IUserCtxProvider userCtxProvider) {
            _innerHandler = innerHandler;
            _userCtxProvider = userCtxProvider;
        }

        public async Task<ErrOrNothing> Handle(TCommand command, CancellationToken ct) {
            if (_userCtxProvider.Current.IsAuthenticated(out  _)) {
                return await _innerHandler.Handle(command, ct);
            }

            return command.UnauthenticatedErr;
        }
    }

    public sealed class QueryHandler<TQuery, TResponse> : IQueryHandler<TQuery, TResponse> where TQuery :
        IQuery<TResponse>,
        IWithAuthCheckStep
    {
        private readonly IQueryHandler<TQuery, TResponse> _innerHandler;
        private readonly IUserCtxProvider _userCtxProvider;


        public QueryHandler(IQueryHandler<TQuery, TResponse> innerHandler, IUserCtxProvider userCtxProvider) {
            _innerHandler = innerHandler;
            _userCtxProvider = userCtxProvider;
        }

        public async Task<ErrOr<TResponse>> Handle(TQuery query, CancellationToken ct) {
            if (_userCtxProvider.Current.IsAuthenticated(out  _)) {
                return await _innerHandler.Handle(query, ct);
            }

            return query.UnauthenticatedErr;
        }
    }
}