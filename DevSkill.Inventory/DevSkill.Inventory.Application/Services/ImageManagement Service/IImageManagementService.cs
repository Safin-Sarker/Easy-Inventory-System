﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services.ImageManagement_Service
{
    public interface IImageManagementService
    {
        Task<string> UploadAsync(IFormFile file);
    }
}
