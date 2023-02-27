using BWRecipeBook.Services.Interfaces;
using Microsoft.Build.Framework;
using System.Drawing.Text;

namespace ContactPro.Services
{
    public class ImageService : IImageService
    {
        private readonly string defaultImage = "/img/DefaultRecipeImage.png";

        public string ConvertBiteArrayToFile(byte[] fileData, string extension)
        {
            if (fileData == null)
            {
                return defaultImage;
            }


            try
            {
                string imageBase64Data = Convert.ToBase64String(fileData);
                imageBase64Data = string.Format($"data:{extension};base64,{imageBase64Data}");

                return imageBase64Data;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            try
            {
                using MemoryStream memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                byte[] bytefile = memoryStream.ToArray();
                memoryStream.Close();

                return bytefile;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
