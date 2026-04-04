using Microsoft.AspNetCore.Routing;

namespace ApiShared;

public interface IEndpointGroup
{
    RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder);
}

public interface ICreatableResponse<TSource>
{
    static abstract ICreatableResponse<TSource> Create(TSource success);
}

public interface IRequestWithValidationNeeded
{
    public ErrOrNothing Validate();
}