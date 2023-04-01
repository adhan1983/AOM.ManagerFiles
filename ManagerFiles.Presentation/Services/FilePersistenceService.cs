using ManagerFiles.Presentation.Contants;
using ManagerFiles.Presentation.Hubs;
using ManagerFiles.Presentation.Models;
using ManagerFiles.Presentation.ServicesInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerFiles.Presentation.Services
{
    public class FilePersistenceService : IFilePersistenceService
    {
        private IHostEnvironment _environment;
        private readonly IHubContext<BroadCastHubService> _broadCastHubService;
        private string _filesFolder;
        private string _originFolder;
        private string _destinyFolder;
        
        public FilePersistenceService(IHostEnvironment environment, IHubContext<BroadCastHubService> broadCastHubService)
        {
            _environment = environment;
            _broadCastHubService = broadCastHubService;
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
            int filesTransfed = 0;
            
            await SendFeedBack(1, filesTransfed, $"Starting Uploading file: {file.FileName}");
            
            await Task.Delay(2000);

            

            using (FileStream filestream = new FileStream(Path.Combine(_originFolder, Path.GetFileName(file.FileName)), FileMode.Create))
            {
                filesTransfed++;
                
                await SendFeedBack(1, filesTransfed, "Uploading.......");
                
                await file.CopyToAsync(filestream);

                await Task.Delay(2000);
            }

            await SendFeedBack(1, filesTransfed, $"Finishing Uploading file: {file.FileName}");

            await Task.Delay(2000);
        }

        public async Task CopyOrMoveFilesAsync(bool justCopy, string[] fileNames)
        {
            int totalFiles = fileNames.Count();
            int filesTransfed = 0;
            
            await SendFeedBack(totalFiles, filesTransfed, "Starting Copy Files");            
            await Task.Delay(2000);

            foreach (var name in fileNames)
            {
                var file = Directory.GetFiles(_originFolder, "*.*", SearchOption.AllDirectories).Where(a => a.Contains(name)).FirstOrDefault();
                
                File.Copy(file, file.Replace(_originFolder, _destinyFolder), true);
                filesTransfed++;
                
                await SendFeedBack(totalFiles, filesTransfed, name);
                
                await Task.Delay(2000);
                
                if (!justCopy)
                {
                    File.Delete(file);
                }                
            }

            await SendFeedBack(totalFiles, filesTransfed, "Finish coping files");
            
            await Task.Delay(2000);
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

        private async Task SendFeedBack(int currentCount, int UploadCount, string message)
        {            
            var feedBackModel = new FeedbackModel()
            {
                currentCount = currentCount,
                currentPercent = (UploadCount * 100 / currentCount).ToString(),
                UploadCount = UploadCount,
                nameFile = message
            };
            await _broadCastHubService.Clients.All.SendAsync("feedBack", feedBackModel);            
        }

    }
}
