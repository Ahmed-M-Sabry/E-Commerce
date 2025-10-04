//using ECommerce.Application.Common;
//using ECommerce.Application.IServices;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Threading.Tasks;

//namespace ECommerce.Infrastructure.Services
//{
//    public class FileService : IFileService
//    {
//        private readonly IWebHostEnvironment _webHostEnvironment;
//        public FileService(IWebHostEnvironment webHostEnvironment)
//        {
//            _webHostEnvironment = webHostEnvironment;
//        }
//        public async Task<Result<string>> UploadFileAsync(IFormFile file, string targetFolder, string expectedType)
//        {
//            // التأكد إن الملف موجود ومش فاضي
//            if (file == null || file.Length == 0)
//                return Result<string>.Failure("File is empty.", ErrorType.BadRequest);

//            // التحقق من امتداد الملف (jpg, png, mp4 ...)
//            var extValidation = ValidateExtension(file, expectedType);
//            if (!extValidation.IsSuccess)
//                return extValidation;

//            // التحقق من نوع الملف (image/png مثلاً)
//            var mimeValidation = ValidateMimeType(file, expectedType);
//            if (!mimeValidation.IsSuccess)
//                return mimeValidation;

//            ValidateFileSize(file , 10 * 1024 * 1024); // 5 MB

//            // تحديد مكان حفظ الملف داخل مجلد wwwroot/uploads/targetFolder
//            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), _webHostEnvironment.WebRootPath, "uploads", targetFolder);

//            // إنشاء المجلد لو مش موجود
//            if (!Directory.Exists(uploadsFolder))
//                Directory.CreateDirectory(uploadsFolder);

//            // إنشاء اسم فريد للملف باستخدام Guid + التاريخ + الامتداد
//            string uniqueFileName = $"{Guid.NewGuid()}_{DateTime.UtcNow:yyyyMMddHHmmss}{Path.GetExtension(file.FileName)}";

//            // المسار الكامل اللي هنحفظ فيه الملف فعليًا
//            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

//            // حفظ الملف على السيرفر
//            using (var stream = new FileStream(filePath, FileMode.Create))
//            {
//                await file.CopyToAsync(stream);
//            }

//            // إرجاع المسار النسبي للملف (للاستخدام في API أو رابط صورة مثلاً)
//            return Result<string>.Success($"/uploads/{targetFolder}/{uniqueFileName}");
//        }

//        // دالة التحقق من امتداد الملف حسب نوعه المتوقع
//        private Result<string> ValidateExtension(IFormFile file, string expectedType)
//        {
//            var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
//            var allowedVideoExtensions = new[] { ".mp4", ".avi", ".mov" };
//            var fileExtension = Path.GetExtension(file.FileName).ToLower();

//            if (expectedType == "image" && !allowedImageExtensions.Contains(fileExtension))
//                return Result<string>.Failure("Invalid file type. Only JPG, JPEG, PNG, GIF, or WEBP files are allowed.", ErrorType.BadRequest);

//            if (expectedType == "video" && !allowedVideoExtensions.Contains(fileExtension))
//                return Result<string>.Failure("Invalid file type. Only MP4, AVI, or MOV files are allowed.", ErrorType.BadRequest);

//            return Result<string>.Success("Extension is valid.");
//        }

//        // دالة التحقق من MIME Type (اللي هو نوع الملف من الهيدر)
//        private Result<string> ValidateMimeType(IFormFile file, string expectedType)
//        {
//            var allowedImageMimeTypes = new[] { "image/jpeg", "image/png", "image/jpg", "image/gif", "image/webp" };
//            var allowedVideoMimeTypes = new[] { "video/mp4", "video/avi", "video/quicktime" };
//            var mimeType = file.ContentType.ToLowerInvariant();

//            if (expectedType == "image" && !allowedImageMimeTypes.Contains(mimeType))
//                return Result<string>.Failure("Invalid MIME type. Only image files are allowed.", ErrorType.BadRequest);

//            if (expectedType == "video" && !allowedVideoMimeTypes.Contains(mimeType))
//                return Result<string>.Failure("Invalid MIME type. Only video files are allowed.", ErrorType.BadRequest);

//            return Result<string>.Success("MIME type is valid.");
//        }

//        public Result<bool> ValidateFileSize(IFormFile file, long maxSizeInBytes)
//        {
//            if (file == null)
//                return Result<bool>.Failure("File is null", ErrorType.BadRequest);

//            if (file.Length > maxSizeInBytes)
//                return Result<bool>.Failure($"File size exceeds the maximum allowed ({maxSizeInBytes / 1024 / 1024} MB)", ErrorType.BadRequest);

//            return Result<bool>.Success(true);
//        }

//    }
//}

//using ECommerce.Application.IServices;
//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ECommerce.Infrastructure.Services
//{
//    public class FileService : IFileService
//    {
//        public async Task<string> UploadFileAsync(IFormFile file, string targetFolder, string expectedType)
//        {
//            if (file == null || file.Length == 0)
//                throw new BadHttpRequestException("File is empty.");

//            ValidateExtension(file, expectedType);

//            ValidateMimeType(file, expectedType);

//            //ValidateFileSize(file);

//            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", targetFolder);
//            if (!Directory.Exists(uploadsFolder))
//                Directory.CreateDirectory(uploadsFolder);

//            string uniqueFileName = $"{Guid.NewGuid()}_{DateTime.UtcNow:yyyyMMddHHmmss}{Path.GetExtension(file.FileName)}";
//            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

//            using (var stream = new FileStream(filePath, FileMode.Create))
//            {
//                await file.CopyToAsync(stream);
//            }

//            return $"/uploads/{targetFolder}/{uniqueFileName}";
//        }
//        private void ValidateExtension(IFormFile file, string expectedType)
//        {
//            var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
//            var allowedVideoExtensions = new[] { ".mp4", ".avi", ".mov" };
//            var fileExtension = Path.GetExtension(file.FileName).ToLower();

//            if (expectedType == "image" && !allowedImageExtensions.Contains(fileExtension))
//                throw new BadHttpRequestException("Invalid file type. Only JPG, JPEG, PNG, or GIF files are allowed.");
//            if (expectedType == "video" && !allowedVideoExtensions.Contains(fileExtension))
//                throw new BadHttpRequestException("Invalid file type. Only MP4, AVI, or MOV files are allowed.");
//        }
//        private void ValidateMimeType(IFormFile file, string expectedType)
//        {
//            var allowedImageMimeTypes = new[] { "image/jpeg", "image/png", "image/jpg", "image/gif", "image/webp" };
//            var allowedVideoMimeTypes = new[] { "video/mp4", "video/avi", "video/quicktime" };
//            var mimeType = file.ContentType.ToLowerInvariant();
//            if (expectedType == "image" && !allowedImageMimeTypes.Contains(file.ContentType))
//                throw new BadHttpRequestException("Invalid MIME type. Only image files are allowed.");
//            if (expectedType == "video" && !allowedVideoMimeTypes.Contains(file.ContentType))
//                throw new BadHttpRequestException("Invalid MIME type. Only video files are allowed.");

//        }


//    }
//}


//-------------

using ECommerce.Application.Common;
using ECommerce.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<Result<string>> UploadFileAsync(IFormFile file, string targetFolder, string expectedType)
        {
            if (file == null || file.Length == 0)
                return Result<string>.Failure("File is empty.", ErrorType.BadRequest);

            var extValidation = ValidateExtension(file, expectedType);
            if (!extValidation.IsSuccess)
                return extValidation;

            var mimeValidation = ValidateMimeType(file, expectedType);
            if (!mimeValidation.IsSuccess)
                return mimeValidation;

            ValidateFileSize(file, 10 * 1024 * 1024); // 10 MB

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), _webHostEnvironment.WebRootPath, "Uploads", targetFolder);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = $"{Guid.NewGuid()}_{DateTime.UtcNow:yyyyMMddHHmmss}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Result<string>.Success($"/Uploads/{targetFolder}/{uniqueFileName}");
        }

        public async Task<string> CalculateFileHashAsync(IFormFile file)
        {
            using (var md5 = MD5.Create())
            using (var stream = file.OpenReadStream())
            {
                var hash = await md5.ComputeHashAsync(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        private Result<string> ValidateExtension(IFormFile file, string expectedType)
        {
            var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var allowedVideoExtensions = new[] { ".mp4", ".avi", ".mov" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (expectedType == "image" && !allowedImageExtensions.Contains(fileExtension))
                return Result<string>.Failure("Invalid file type. Only JPG, JPEG, PNG, GIF, or WEBP files are allowed.", ErrorType.BadRequest);

            if (expectedType == "video" && !allowedVideoExtensions.Contains(fileExtension))
                return Result<string>.Failure("Invalid file type. Only MP4, AVI, or MOV files are allowed.", ErrorType.BadRequest);

            return Result<string>.Success("Extension is valid.");
        }

        private Result<string> ValidateMimeType(IFormFile file, string expectedType)
        {
            var allowedImageMimeTypes = new[] { "image/jpeg", "image/png", "image/jpg", "image/gif", "image/webp" };
            var allowedVideoMimeTypes = new[] { "video/mp4", "video/avi", "video/quicktime" };
            var mimeType = file.ContentType.ToLowerInvariant();

            if (expectedType == "image" && !allowedImageMimeTypes.Contains(mimeType))
                return Result<string>.Failure("Invalid MIME type. Only image files are allowed.", ErrorType.BadRequest);

            if (expectedType == "video" && !allowedVideoMimeTypes.Contains(mimeType))
                return Result<string>.Failure("Invalid MIME type. Only video files are allowed.", ErrorType.BadRequest);

            return Result<string>.Success("MIME type is valid.");
        }

        public Result<bool> ValidateFileSize(IFormFile file, long maxSizeInBytes)
        {
            if (file == null)
                return Result<bool>.Failure("File is null", ErrorType.BadRequest);

            if (file.Length > maxSizeInBytes)
                return Result<bool>.Failure($"File size exceeds the maximum allowed ({maxSizeInBytes / 1024 / 1024} MB)", ErrorType.BadRequest);

            return Result<bool>.Success(true);
        }
    }
}