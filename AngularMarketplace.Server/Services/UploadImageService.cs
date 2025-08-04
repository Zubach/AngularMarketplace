using AngularMarketplace.Server.Services.Intefaces;

namespace AngularMarketplace.Server.Services
{
    public class UploadImageService : IUploadImageService
    {
        public string GetFileExtension(string fileName)
        {
            if (fileName != "")
            {
                var extension = fileName.Split(".").Last();
                return extension;
            }
            return "";
        }

        public string GetFileExtension(IFormFile file)
        {
            if(file != null)
            {
                var extension = file.FileName.Split(".").Last();
                return extension;
            }
            return "";
        }

        public async Task UploadImageAsync(IFormFile file, string folder,string fileName)
        {
            string fullPath = Path.Combine(folder, fileName);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            using (MemoryStream ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                byte[] bytes = ms.ToArray();
                await System.IO.File.WriteAllBytesAsync(fullPath, bytes);

            }       
        }

    }
}
