using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using TAB.Application.Core.Interfaces.Common;
using TAB.Contracts.Features.Shared;
using TAB.Infrastructure.Common.Options;

namespace TAB.Infrastructure.Common;

public class AzureBlobService : IUploadFileService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly AzureBlobOptions _options;
    private readonly IGeneratorService _generatorService;

    public AzureBlobService(
        BlobServiceClient blobServiceClient,
        IOptions<AzureBlobOptions> options,
        IGeneratorService generatorService
    )
    {
        _blobServiceClient = blobServiceClient;
        _generatorService = generatorService;
        _options = options.Value;
    }

    public async Task<FileResponse[]> UploadFilesAsync(
        FileRequest[] files,
        string? folderName = null
    )
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_options.ContainerName);

        var fileResponses = new List<FileResponse>();

        foreach (var file in files)
        {
            var fileName = _generatorService.GenerateUniqueFileName(
                Path.GetExtension(file.FileName)
            );

            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(new MemoryStream(file.Content));

            fileResponses.Add(new FileResponse(file.FileName, blobClient.Uri.AbsoluteUri));
        }

        return fileResponses.ToArray();
    }
}
