using Microsoft.Extensions.FileProviders;

namespace AngularMarketplace.Server.Services.Intefaces
{
    public interface IUploadImageService
    {
        public Task UploadImageAsync(IFormFile file,string folder,string fileName);
        public string GetFileExtension(string fileName);
        public string GetFileExtension(IFormFile file);
    }
}
