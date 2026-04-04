using SharedKernel.common.vokis;
namespace VokiCreationServicesLib.Api.contracts;

public record class VokiNameResponse(
    string VokiName
) : ICreatableResponse<VokiName>

{
    public static ICreatableResponse<VokiName> Create(VokiName name) => new VokiNameResponse(
        name.ToString()
    );
}