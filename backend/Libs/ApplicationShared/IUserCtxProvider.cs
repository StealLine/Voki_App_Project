using SharedKernel.user_ctx;

namespace ApplicationShared;

public interface IUserCtxProvider
{
    public IUserCtx Current { get; }
}