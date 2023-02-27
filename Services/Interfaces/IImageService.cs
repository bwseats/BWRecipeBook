namespace BWRecipeBook.Services.Interfaces
{
    public interface IImageService
    {
        public Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file);

        public string ConvertBiteArrayToFile(byte[] fileData, string extension);
    }
}
