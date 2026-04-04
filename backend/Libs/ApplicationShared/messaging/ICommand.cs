namespace ApplicationShared.messaging;

public interface ICommand
{
    bool RequireTransaction => true;
}

public interface ICommand<in TResponse>
{
    bool RequireTransaction => true;
}