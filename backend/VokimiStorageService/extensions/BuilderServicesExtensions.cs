using Amazon.Runtime;
using Amazon.S3;
using VokimiStorageService.s3_storage.audio_compression;
using VokimiStorageService.s3_storage.images_compression;
using VokimiStorageService.s3_storage.s3;
using VokimiStorageService.s3_storage.storage_service;

namespace VokimiStorageService.extensions;

internal static class BuilderServicesExtensions
{
    internal static IServiceCollection AddS3Storage(this IServiceCollection services, IConfiguration configuration) {
        var s3Config = configuration.GetSection("S3").Get<S3Config>();
        if (s3Config is null) {
            throw new Exception("S3 is not configured");
        }

        services.AddSingleton<IAmazonS3>(_ => new AmazonS3Client(
            new BasicAWSCredentials(s3Config.AccessKey, s3Config.SecretKey),
            new AmazonS3Config { ServiceURL = s3Config.ServiceUrl }
        ));

        services.AddSingleton(s3Config.MainBucket); //S3MainBucketConf
        services.AddScoped<IS3MainBucketClient, S3MainBucketClient>();
        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<IImageFileConverter, ImageFileConverter>();
        services.AddScoped<IAudioFileConverter, AudioFileConverter>();
        
        return services;
    }
}