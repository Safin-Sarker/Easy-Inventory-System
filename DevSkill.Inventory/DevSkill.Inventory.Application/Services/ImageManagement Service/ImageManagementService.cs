using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DevSkill.Inventory.Application.Services.ImageManagement_Service
{
    public class ImageManagementService :IImageManagementService
    {
        private readonly Account _account;
        private readonly Cloudinary _cloudinary;

        public ImageManagementService(IConfiguration configuration)
        {
            _account = new Account(
                configuration["Cloudinary:CloudName"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:ApiSecret"]
            );
            _cloudinary = new Cloudinary(_account);
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                PublicId = Guid.NewGuid().ToString() 
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return uploadResult?.SecureUri?.ToString();
        }

    }
}
