namespace Online_Store_ASP.NET_Core_MVC.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _env;

        public FileUploadService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string Upload(IFormFile file, string uploadsFolder)
        {
            var filePath = Path.Combine(_env.WebRootPath, uploadsFolder, file.FileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                file.CopyTo(stream);
            }
            return file.FileName;
        }
    }
}
