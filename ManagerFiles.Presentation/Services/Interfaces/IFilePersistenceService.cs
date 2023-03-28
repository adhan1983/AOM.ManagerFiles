using ManagerFiles.Presentation.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ManagerFiles.Presentation.ServicesInterfaces
{
    public interface IFilePersistenceService
    {
        Task<string> GetFilesAsync();

        Task<List<FolderModel>> GetFoldersAndFilesAsync();

    }
}
