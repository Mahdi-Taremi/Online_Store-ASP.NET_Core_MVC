namespace Online_Store_ASP.NET_Core_MVC.Services
{
    public interface IFileUploadService
    {
        string Upload(IFormFile file, string uploadsFolder);
    }
}
