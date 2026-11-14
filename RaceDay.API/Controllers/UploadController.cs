using Microsoft.AspNetCore.Mvc;
using RaceDay.API.Services;
namespace RaceDay.API.Controllers
{
  [Route("api/[controller]")] [ApiController]
  public class UploadController : ControllerBase
  {
    private readonly BlobService _blob;
    public UploadController(BlobService blob) { _blob = blob; }
    [HttpPost("banner")] public async Task<IActionResult> UploadBanner(IFormFile file)
    { if (file == null) return BadRequest("No file"); var url = await _blob.UploadBannerAsync(file); return Ok(new { url }); }
  }
}
