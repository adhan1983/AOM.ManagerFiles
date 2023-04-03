using ManagerFiles.Presentation.Models;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ManagerFiles.Presentation.ServicesInterfaces
{
    public interface IFilePersistenceService
    {
        Task CopyOrMoveFilesAsync(bool justCopy, string[] fileNames);

        Task<FolderViewModel> GetFoldersAndFilesAsync();

        Task SaveFileToOriginAsync(IFormFile file);

        Task RollBackFiles(IFormFile file);

    }
}
