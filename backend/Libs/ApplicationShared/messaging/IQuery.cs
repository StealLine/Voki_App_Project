namespace ApplicationShared.messaging;

public interface IQuery<TResponse>
{
    bool RequireTransaction => false;
}