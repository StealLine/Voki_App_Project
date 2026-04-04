using ApiShared;
using ApiShared.extensions;
using ApplicationShared;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.user_ctx;
using VokimiStorageService.s3_storage.s3;
using VokimiStorageService.s3_storage.storage_service;

namespace VokimiStorageService;

internal class EndpointHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/main");

        group.MapGet("/{*fileKey}", GetFileFromStorage)
            .DisableAntiforgery();
        group.MapPut("/upload-temp-image", UploadTempImage)
            .DisableAntiforgery();
        group.MapPut("/upload-temp-audio", UploadTempAudio)
            .DisableAntiforgery();

        return group;
    }

    private static async Task<IResult> GetFileFromStorage(
        CancellationToken ct,
        string fileKey,
        IS3MainBucketClient s3MainBucketClient
    ) {
        ErrOr<FileData> result = await s3MainBucketClient.GetFile(fileKey, ct);
        return CustomResults.FromErrOr(result, (file) =>
            Results.Stream(
                stream: file.Stream,
                contentType: file.ContentType,
                fileDownloadName: fileKey,
                enableRangeProcessing: true
            )
        );
    }

    private static async Task<IResult> UploadTempImage(
        CancellationToken ct,
        [FromForm] IFormFile file,
        IStorageService storageService,
        IUserCtxProvider userCtxProvider
    ) {
        if (userCtxProvider.Current.IsAuthenticated(out var _)) {
            FileData fileData = new(file.OpenReadStream(), file.ContentType);
            ErrOr<TempImageKey> res = await storageService.PutTempImageFile(fileData, ct);

            return CustomResults.FromErrOr(res, (key) => Results.Json(
                new { TempKey = key.ToString() }
            ));
        }

        return CustomResults.ErrorResponse(ErrFactory.AuthRequired());
    }

    private static async Task<IResult> UploadTempAudio(
        CancellationToken ct,
        [FromForm] IFormFile file,
        IStorageService storageService,
        IUserCtxProvider userCtxProvider
    ) {
        if (userCtxProvider.Current.IsAuthenticated(out var _)) {
            FileData fileData = new(file.OpenReadStream(), file.ContentType);
            ErrOr<TempAudioKey> res = await storageService.PutTempAudioFile(fileData, ct);

            return CustomResults.FromErrOr(res, (key) => Results.Json(
                new { TempKey = key.ToString() }
            ));
        }

        return CustomResults.ErrorResponse(ErrFactory.AuthRequired());
    }
}