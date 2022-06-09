using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public static class Extensions
    {
        public static async Task<string> CreateImage(this IFormFile imageFile, string rootPath)
        {
            string fileName = imageFile.FileName;

            if (fileName.Length > 219)
            {
                fileName.Substring(fileName.Length - 219, 219);
            }

            fileName = Guid.NewGuid().ToString() + fileName;

            string path = Path.Combine(rootPath, "assets", "uploads", "images", "sliders", fileName);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return fileName;
        }
    }
}
