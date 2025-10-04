using ECommerce.Application.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.IServices
{
    public interface IFileService
    {
        //Task<string> UploadFileAsync(IFormFile file, string targetFolder, string expectedType);
        Task<Result<string>> UploadFileAsync(IFormFile file, string targetFolder, string expectedType);
        Task<string> CalculateFileHashAsync(IFormFile file);
    }
}
