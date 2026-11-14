using Azure.Storage.Blobs;
namespace RaceDay.API.Services
{
  public class BlobService
  {
    private readonly IConfiguration _config;
    public BlobService(IConfiguration config) { _config = config; }

    public async Task<string> UploadBannerAsync(IFormFile file)
    {
      try
      {
        var conn = _config["AzureBlob:ConnectionString"] ?? "UseDevelopmentStorage=true";
        var containerName = _config["AzureBlob:ContainerName"] ?? "raceday-images";

        // Short timeouts so a missing Azurite/Azure does not hang the upload before the local fallback kicks in
        var options = new BlobClientOptions();
        options.Retry.MaxRetries = 1;
        options.Retry.Delay = TimeSpan.FromMilliseconds(500);
        options.Retry.NetworkTimeout = TimeSpan.FromSeconds(5);

        var client = new BlobServiceClient(conn, options);
        var container = client.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var blob = container.GetBlobClient(fileName);
        using (var stream = file.OpenReadStream()) { await blob.UploadAsync(stream, true); }
        return blob.Uri.ToString();
      }
      catch
      {
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "banners", fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        using (var s = new FileStream(path, FileMode.Create)) { await file.CopyToAsync(s); }
        return "/banners/" + fileName;
      }
    }
  }
}
