using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace RoutesStation.Interface.Upload
{
	public class UploudRep:IUploudRep
	{
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UploudRep(IWebHostEnvironment webHostEnvironment)
		{
            _webHostEnvironment = webHostEnvironment;
            

        }

        public async Task<string> AddImageProfile(IFormFile image, string Folder, string UserName)
        {
            string PathImage;
            await DeleteFileProfile(Folder, UserName);
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Files/" + Folder + "/" + UserName);
            bool exists = System.IO.Directory.Exists(uploadsFolder);
            if (!exists)
                System.IO.Directory.CreateDirectory(uploadsFolder);
            string uniqueFileName =UserName + ".webp";

            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            PathImage = "Files/" + Folder + "/" + UserName + "/" + uniqueFileName;
           
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {

                image.CopyTo(fileStream);
            }
            return PathImage;
        }

        public async Task<bool> DeleteFileProfile(string Folder, string UserName)
        {
            try
            {
                string StringPath = Path.Combine(_webHostEnvironment.WebRootPath, "Files/" + Folder + "/" + UserName);
                string[] files = Directory.GetFiles(StringPath);
                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }
                Directory.Delete(StringPath);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}

