using System.IO;
using System.Threading.Tasks;
using System.IO.Abstractions;
using ManagerFiles.Presentation.ServicesInterfaces;
using System.Collections.Generic;
using ManagerFiles.Presentation.Models;

namespace ManagerFiles.Presentation.Services
{

   

    public class FilePersistenceService : IFilePersistenceService
    {       
        public static IFileSystem FileSystem { get; set; } = new FileSystem();

        public Task<List<FolderModel>> GetFoldersAndFilesAsync()
        {

            var folderAndFiles = new List<FolderModel>();
            
            
            
            string currentDirectory = Directory.GetCurrentDirectory();

            string filesPath = currentDirectory + "\\Files";

            var allFolders = Directory.GetDirectories(filesPath);
            
            foreach (var folder in allFolders)
            {              
                DirectoryInfo obj = new DirectoryInfo(folder);

                var folderModel = new FolderModel();
                
                folderModel.Name = obj.Name;

                var filesFromFolder = obj.GetFiles();

                foreach (var file in filesFromFolder)
                {
                    folderModel.Files.Add(new FileFolderModel 
                    {
                        Name = file.Name,
                        TypeFile = file.Extension,
                        
                    });
                }

                folderAndFiles.Add(folderModel);

            }

            return Task.FromResult(folderAndFiles);
        }


        public Task<string> GetFilesAsync()
        {

            string rootPath = Directory.GetCurrentDirectory();

            string origin = rootPath + "\\Files\\Origin";
            string destiny = rootPath + "\\Files\\Destiny";

            

            //var assembly = typeof(FilePersistence).Assembly;

            //var prefix = assembly.GetName().Name;            

            //var result = Encoding.UTF8.GetString(ToByteArray(stream));

            var allFiles = Directory.GetFiles(origin, "*.*", SearchOption.AllDirectories);

            var _myfile = Directory.GetFiles(origin, "*employes.csv");
            
            foreach (string newPath in allFiles)
            {
                File.Copy(newPath, newPath.Replace(origin, destiny), true);
            }

            return Task.FromResult(rootPath.ToString());

        }

        private static byte[] ToByteArray(Stream stream)
        {
            using var ms = new MemoryStream();

            stream.CopyTo(ms);

            return ms.ToArray();
        }





    }
}
