using SharedKernel.errs;

namespace ApplicationShared.messaging;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task<ErrOrNothing> Handle(TCommand command, CancellationToken ct);
}

public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    Task<ErrOr<TResponse>> Handle(TCommand command, CancellationToken ct);
}