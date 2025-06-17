namespace petmanagement.Services
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folder);
        void DeleteFile(string filePath);
        bool IsValidImageFile(IFormFile file);
    }
}