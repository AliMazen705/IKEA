using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Common.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly List<string> AllowedExtensions = new() { ".jpg", ".jpeg", ".png" };
        private const int FileMaxSize = 2_097_152;//2mbs
        public string UploadFile(IFormFile file, string FolderName)
        {
            #region Validations
            var fileExtension=Path.GetExtension(file.FileName);
            if (!AllowedExtensions.Contains(fileExtension))
            {
                throw new Exception("Invaild File Extenstion Please Try Again");
            }
            if (file.Length > FileMaxSize)
            {
                throw new Exception("File Size Exceded The Limit Please Try Again");
            }
            #endregion
            //1-Get located folder path
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files",  FolderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            //2-Get file name and make it unique
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            //3-Get full path for file 1+2
            var fullPath = Path.Combine(folderPath, uniqueFileName);
            //4-Save file in the path use stream
            using var FileStream = new FileStream(fullPath, FileMode.Create);
            file.CopyTo(FileStream);
            //5-Return file relative path
            return fullPath;
        }
        public bool Delete(string filePath)
        {
           if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }

      
    }
}
