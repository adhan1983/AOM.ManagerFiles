using System.Collections.Generic;

namespace ManagerFiles.Presentation.Models
{
    public class FolderModel
    {
        public string Name { get; set; }
        public List<FileFolderModel> Files { get; set; } = new List<FileFolderModel>();
    }
}
