using TAB.Contracts.Features.Shared;

namespace TAB.Application.Core.Interfaces.Common;

public interface IUploadFileService
{
    Task<FileResponse[]> UploadFilesAsync(FileRequest[] files, string? folderName = null);
}
