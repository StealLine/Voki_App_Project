using System.Net;
using Amazon.S3;
using Amazon.S3.Model;

namespace VokimiStorageService.s3_storage.s3;

internal class S3MainBucketClient : IS3MainBucketClient
{
    private readonly IAmazonS3 _s3;
    private readonly S3MainBucketConf _s3MainBucketConf;
    private readonly ILogger<S3MainBucketClient> _logger;

    public S3MainBucketClient(
        IAmazonS3 s3,
        S3MainBucketConf s3MainBucketConf,
        ILogger<S3MainBucketClient> logger
    ) {
        _s3 = s3;
        _s3MainBucketConf = s3MainBucketConf;
        _logger = logger;
    }


    public async Task<ErrOrNothing> PutFile(ITempKey key, FileData file, CancellationToken ct) {
        try {
            PutObjectRequest req = new() {
                BucketName = _s3MainBucketConf.Name,
                Key = key.Value,
                InputStream = file.Stream,
                ContentType = file.ContentType
            };

            PutObjectResponse resp = await _s3.PutObjectAsync(req, ct);
            _logger.LogInformation("PUT temp success: key={Key} etag={ETag}", key.Value, resp.ETag);

            return ErrOrNothing.Nothing;
        }
        catch (AmazonS3Exception ex) {
            _logger.LogError(ex, "PUT temp failed: bucket={Bucket} key={Key}", _s3MainBucketConf.Name, key.Value);
            return ErrFactory.Unspecified("File upload failed");
        }
        catch (Exception ex) {
            _logger.LogError(ex, "PUT temp unexpected error: bucket={Bucket} key={Key}",
                _s3MainBucketConf.Name, key.Value);
            return ErrFactory.Unspecified("File upload failed");
        }
    }

    public async Task<ErrOr<FileData>> GetFile(string key, CancellationToken ct) {
        try {
            GetObjectRequest req = new() {
                BucketName = _s3MainBucketConf.Name,
                Key = key
            };

            GetObjectResponse resp = await _s3.GetObjectAsync(req, ct);

            MemoryStream ms = new();
            await using (resp.ResponseStream) {
                await resp.ResponseStream.CopyToAsync(ms, ct);
            }

            ms.Position = 0;

            string contentType = resp.Headers.ContentType ?? "application/octet-stream";
            _logger.LogInformation("GET success: key={Key} contentType={ContentType} length={Length}",
                key, contentType, ms.Length);

            return new FileData(ms, contentType);
        }
        catch (AmazonS3Exception ex) {
            if (ex.StatusCode == HttpStatusCode.NotFound) {
                _logger.LogWarning("GET not found: bucket={Bucket} key={Key}", _s3MainBucketConf.Name, key);
                return ErrFactory.NotFound.Common("Object not found");
            }

            _logger.LogError(ex, "GET failed: bucket={Bucket} key={Key}", _s3MainBucketConf.Name, key);
            return ErrFactory.Unspecified("File get failed");
        }
        catch (Exception ex) {
            _logger.LogError(ex, "GET unexpected error: bucket={Bucket} key={Key}", _s3MainBucketConf.Name, key);
            return ErrFactory.Unspecified("File get failed");
        }
    }
}