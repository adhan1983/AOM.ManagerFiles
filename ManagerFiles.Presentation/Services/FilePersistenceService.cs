using ManagerFiles.Presentation.Contants;
using ManagerFiles.Presentation.Models;
using ManagerFiles.Presentation.ServicesInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerFiles.Presentation.Services
{
    public class FilePersistenceService : IFilePersistenceService
    {
        private IHostEnvironment _environment;
        private string _filesFolder;
        private string _originFolder;
        private string _destinyFolder;
        public FilePersistenceService(IHostEnvironment environment)
        {
            _environment = environment;
            _filesFolder = $"{_environment.ContentRootPath}{ManagerFilesConstants.FILE}";
            _originFolder = Path.Combine(_filesFolder, ManagerFilesConstants.ORIGIN);
            _destinyFolder = Path.Combine(_filesFolder, ManagerFilesConstants.DESTINY);

        }
        public Task<FolderViewModel> GetFoldersAndFilesAsync()
        {
            var folderAndFiles = new FolderViewModel();

            var allFolders = Directory.GetDirectories(_filesFolder);

            foreach (var folder in allFolders)
            {
                DirectoryInfo obj = new DirectoryInfo(folder);

                switch (obj.Name)
                {
                    case ManagerFilesConstants.ORIGIN:
                        folderAndFiles.Origin.Name = obj.Name;
                        BuidFolderModel(obj, folderAndFiles.Origin);
                        break;
                    case ManagerFilesConstants.DESTINY:
                        folderAndFiles.Destiny.Name = obj.Name;
                        BuidFolderModel(obj, folderAndFiles.Destiny);
                        break;
                    default:
                        break;
                }
            }

            return Task.FromResult(folderAndFiles);
        }

        public async Task SaveFileToOriginAsync(IFormFile file)
        {
            using (FileStream filestream = new FileStream(Path.Combine(_originFolder, Path.GetFileName(file.FileName)), FileMode.Create))
            {
                await file.CopyToAsync(filestream);
            }
        }

        public Task<string> CopyOrMoveFilesAsync(bool justCopy, string[] fileNames)
        {
            foreach (var name in fileNames)
            {

                var file = Directory.GetFiles(_originFolder, "*.*", SearchOption.AllDirectories).Where(a => a.Contains(name)).FirstOrDefault();
                
                File.Copy(file, file.Replace(_originFolder, _destinyFolder), true);
                
                if (!justCopy)
                {
                    File.Delete(file);
                }
            }            

            return Task.FromResult(_destinyFolder);

        }

        private FolderModel BuidFolderModel(DirectoryInfo obj, FolderModel folderModel)
        {
            var filesFromFolder = obj.GetFiles();

            foreach (var file in filesFromFolder)
            {
                folderModel.Files.Add(new FileFolderModel
                {
                    Name = file.Name,
                    TypeFile = file.Extension,
                });
            }

            return folderModel;

        }

        private static byte[] ToByteArray(Stream stream)
        {
            using var ms = new MemoryStream();

            stream.CopyTo(ms);

            return ms.ToArray();
        }





    }
}
