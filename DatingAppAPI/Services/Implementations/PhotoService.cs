using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingAppAPI.Helpers;
using Microsoft.Extensions.Options;

namespace DatingAppAPI.Services.Implementations
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettings> configuration)
        {
            Account account = new Account(
                configuration.Value.CloudName,
                configuration.Value.ApiName,
                configuration.Value.ApiSecret
            );
            this._cloudinary = new Cloudinary(account);
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            return await this._cloudinary.DestroyAsync(new DeletionParams(publicId));
        }

        public async Task<ImageUploadResult> UploadPhotoAsync(IFormFile file)
        {
            if(file.Length > 0) {
                using Stream stream = file.OpenReadStream();
                return await this._cloudinary.UploadAsync(new ImageUploadParams {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                    Folder = ".NET6"
                });
            }

            return new ImageUploadResult();
        }
    }
}