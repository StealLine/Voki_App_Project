using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;

namespace InfrastructureShared.Storage;

public abstract class BaseMainS3Bucket
{
    private readonly IAmazonS3 _s3;
    private readonly S3MainBucketConf _s3MainBucketConf;
    private readonly ILogger<BaseMainS3Bucket> _logger;

    protected BaseMainS3Bucket(IAmazonS3 s3, S3MainBucketConf s3MainBucketConf, ILogger<BaseMainS3Bucket> logger) {
        _s3 = s3;
        _s3MainBucketConf = s3MainBucketConf;
        _logger = logger;
    }

    protected Task<ErrOrNothing> CopyTempToStandard(
        ITempKey source,
        BaseStorageKey destination,
        CancellationToken ct
    ) => BaseCopy(
        new FileCopyDto(source.Value, source.Extension.Value),
        new FileCopyDto(destination.ToString(), destination.Extension.Value),
        logsString: "COPY temp->standard",
        ct: ct
    );

    protected Task<ErrOrNothing> CopyMultipleTempToStandard<TSrc, TDest>(
        IDictionary<TSrc, TDest> sourcesToDestinations, CancellationToken ct
    ) where TSrc : ITempKey where TDest : BaseStorageKey =>
        BaseCopyMultipleFailFast(
            sourcesToDestinations.Select(kv => (
                Src: new FileCopyDto(kv.Key.Value, kv.Key.Extension.Value),
                Dst: new FileCopyDto(kv.Value.ToString(), kv.Value.Extension.Value)
            )),
            logsString: "COPY-MULTI temp->standard", ct: ct
        );


    protected Task<ErrOrNothing> CopyStandardToStandard(
        BaseStorageKey source,
        BaseStorageKey destination,
        CancellationToken ct
    ) => BaseCopy(
        new FileCopyDto(source.ToString(), source.Extension.Value),
        new FileCopyDto(destination.ToString(), destination.Extension.Value),
        logsString: "COPY standard->standard",
        ct: ct
    );

    private record class FileCopyDto(string Key, string Ext);

    private async Task<ErrOrNothing> BaseCopy(
        FileCopyDto src,
        FileCopyDto dest,
        string logsString,
        CancellationToken ct
    ) {
        if (src.Ext != dest.Ext) {
            _logger.LogError(
                "{Log} conflict: ext mismatch srcExt={SrcExt} dstExt={DstExt} srcKey={Src} dstKey={Dst}",
                logsString, src.Ext, dest.Ext, src.Key, dest.Key
            );
            return ErrFactory.Conflict($"Extension mismatch: '{src.Ext}' -> '{dest.Ext}'");
        }

        try {
            var req = new CopyObjectRequest {
                SourceBucket = _s3MainBucketConf.Name,
                SourceKey = src.Key,
                DestinationBucket = _s3MainBucketConf.Name,
                DestinationKey = dest.Key,
                MetadataDirective = S3MetadataDirective.COPY
            };

            var resp = await _s3.CopyObjectAsync(req, ct);
            _logger.LogInformation("{Log} success: etag={ETag} dstKey={Dst}", logsString, resp.ETag, dest.Key);

            return ErrOrNothing.Nothing;
        }
        catch (AmazonS3Exception ex) {
            if (ex.StatusCode == HttpStatusCode.NotFound) {
                _logger.LogError("{Log} failed NotFound: srcKey={Src}", logsString, src.Key);
                return ErrFactory.NotFound.Common(
                    "Source object not found for copy",
                    details: $"No file with key: {src.Key}"
                );
            }

            _logger.LogError(ex, "{Log} failed: src={Src} dst={Dst}", logsString, src.Key, dest.Key);
            return ErrFactory.Unspecified("File copy failed");
        }
        catch (Exception ex) {
            _logger.LogError(ex, "{Log} unexpected error: src={Src} dst={Dst}", logsString, src.Key, dest.Key);
            return ErrFactory.Unspecified("File copy failed");
        }
    }

    private const int MaxBatchCopyCount = 100;
    const int MaxParallelForMultipleCopy = 10;

    private async Task<ErrOrNothing> BaseCopyMultipleFailFast(
        IEnumerable<(FileCopyDto Src, FileCopyDto Dst)> items,
        string logsString,
        CancellationToken ct
    ) {
        List<(FileCopyDto Src, FileCopyDto Dst)> pairs = items .ToList();
        int count = pairs.Count;

        if (count == 0) {
            _logger.LogInformation("{Log} nothing to copy", logsString);
            return ErrOrNothing.Nothing;
        }

        if (count > MaxBatchCopyCount) {
            _logger.LogError("{Log} limit exceeded: count={Count} > {Max}", logsString, count, MaxBatchCopyCount);
            return ErrFactory.LimitExceeded(
                $"Limit exceeded: at most {MaxBatchCopyCount} items per batch"
            );
        }

        if (count == 1) {
            var (src, dest) = pairs[0];
            _logger.LogInformation(
                "{CurrentMethod} detected single item; delegating to {SingleCopyMethod}. src={Src} dst={Dst}",
                nameof(BaseCopyMultipleFailFast), nameof(BaseCopy), src.Key, dest.Key
            );
            return await BaseCopy(
                src, dest,
                logsString: $"{logsString} (single item via {nameof(BaseCopy)})",
                ct: ct
            );
        }

        var mismatchedExtPairs = pairs
            .Where(p => p.Src.Ext != p.Dst.Ext)
            .ToArray();

        if (mismatchedExtPairs.Length > 0) {
            var firstFiveSrcKeys = string.Join(
                ", ",
                mismatchedExtPairs.Take(5).Select(p => p.Src.Key)
            );

            _logger.LogError(
                "{Log} conflict: ext mismatch in {Count} item(s). First5={FirstFive}",
                logsString, mismatchedExtPairs.Length, firstFiveSrcKeys
            );

            return ErrFactory.Conflict(
                $"Extension mismatch in {mismatchedExtPairs.Length} item(s)",
                details: $"First mismatched src keys: {firstFiveSrcKeys}"
            );
        }

        _logger.LogInformation("{Log} started: count={Count}", logsString, count);

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        CancellationToken listCt = cts.Token;

        List<Task<ErrOrNothing>> inFlight = [];

        async Task<ErrOrNothing> CopyOneAsync(FileCopyDto src, FileCopyDto dest, CancellationToken token) {
            try {
                var req = new CopyObjectRequest {
                    SourceBucket = _s3MainBucketConf.Name,
                    SourceKey = src.Key,
                    DestinationBucket = _s3MainBucketConf.Name,
                    DestinationKey = dest.Key,
                    MetadataDirective = S3MetadataDirective.COPY
                };

                var resp = await _s3.CopyObjectAsync(req, token);
                _logger.LogInformation("{Log} success: etag={ETag} dstKey={Dst}", logsString, resp.ETag, dest.Key);
                return ErrOrNothing.Nothing;
            }
            catch (AmazonS3Exception ex) {
                if (ex.StatusCode == HttpStatusCode.NotFound) {
                    _logger.LogError("{Log} failed NotFound: srcKey={Src}", logsString, src.Key);
                    return ErrFactory.NotFound.Common(
                        "Source object not found for copy",
                        details: $"No file with key: {src.Key}"
                    );
                }

                _logger.LogError(ex, "{Log} failed: src={Src} dst={Dst}", logsString, src.Key, dest.Key);
                return ErrFactory.Unspecified("File copy failed");
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested) {
                _logger.LogWarning("{Log} canceled one item: src={Src} dst={Dst}", logsString, src.Key, dest.Key);
                return ErrFactory.Unspecified("Copy was canceled");
            }
            catch (Exception ex) {
                _logger.LogError(ex, "{Log} unexpected error: src={Src} dst={Dst}", logsString, src.Key, dest.Key);
                return ErrFactory.Unspecified("File copy failed");
            }
        }

        using var enumerator = pairs.GetEnumerator();
        if ( inFlight.Count < MaxParallelForMultipleCopy && enumerator.MoveNext()) {
            var (s, d) = enumerator.Current;
            inFlight.Add(CopyOneAsync(s, d, listCt));
        }

        while (inFlight.Count > 0) {
            if (listCt.IsCancellationRequested) {
                _logger.LogWarning("{Log} canceled", logsString);
                return ErrFactory.Unspecified("Copy was canceled");
            }

            var finished = await Task.WhenAny(inFlight);
            inFlight.Remove(finished);

            var result = await finished; // to unwrap
            if (result.IsErr(out var err)) {
                await cts.CancelAsync(); // fail-fast: отменяем остальные
                _logger.LogWarning("{Log} fail-fast: returning error immediately", logsString);
                return err;
            }

            if (enumerator.MoveNext() && !listCt.IsCancellationRequested) {
                var (s, d) = enumerator.Current;
                inFlight.Add(CopyOneAsync(s, d, listCt));
            }
        }

        _logger.LogInformation("{Log} completed: total={Total}, errs=0", logsString, count);
        return ErrOrNothing.Nothing;
    }
}