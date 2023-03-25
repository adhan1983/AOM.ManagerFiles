using ManagerFiles.Presentation.ServicesInterfaces;
using System.IO;


namespace ManagerFiles.Presentation.Services
{
    public class FilePersistence : IFilePersistence
    {
        public void GetFilesAsync()
        {

            string dirname = Directory.GetCurrentDirectory();

            //var assembly = typeof(FilePersistence).Assembly;

            //var prefix = assembly.GetName().Name;            

            //var result = Encoding.UTF8.GetString(ToByteArray(stream));

        }

        private static byte[] ToByteArray(Stream stream)
        {
            using var ms = new MemoryStream();

            stream.CopyTo(ms);

            return ms.ToArray();
        }





    }
}
