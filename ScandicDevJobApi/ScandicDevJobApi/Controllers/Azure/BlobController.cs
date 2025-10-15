using Microsoft.AspNetCore.Mvc;
using ScandicDevJobApi.Services;  
using System.IO;
using System.Threading.Tasks;

[ApiController]
[Route("api/blob")]
public class BlobController : ControllerBase
{
    private readonly AzureBlobStorageService _blobService;

    public BlobController(AzureBlobStorageService blobService)
    {
        _blobService = blobService;
    }

    // GET: /api/blob/logo/{fileName}
    [HttpGet("logo/{fileId}")]
    public async Task<IActionResult> GetLogo(Guid fileId)
    {
        try
        {
            // Download the file from Azure Blob Storage
            var (blobStream, contentType) = await _blobService.DownloadFileAsync(fileId);

            if (blobStream == null)
                return NotFound();  // Return 404 if not found

            return File(blobStream, contentType);
        }
        catch (Exception ex)
        {
            // Log the exception (optional)
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    public async Task<IActionResult> UploadLogo(IFormFile file) // [FromForm]
    {

        if (file == null || file.Length == 0)
            return BadRequest("File is empty or missing.");

        try 
        {
            var stream = file.OpenReadStream();
            var fileId = await _blobService.UploadFileAsync(stream, file.ContentType);

            return Ok(new { fileId, fileName = file.FileName });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Upload failed: {ex.Message}");
        }


    }

    // POST: /api/blob/upload
    //[HttpPost("upload")]
    //[Consumes("multipart/form-data")]
    //public async Task<IActionResult> UploadLogo([FromForm] IFormFile file)
    //{
    //    if (file == null || file.Length == 0)
    //        return BadRequest("File is empty or missing.");

    //    try
    //    {
    //        // Open stream and pass to blob storage service
    //        using var stream = file.OpenReadStream();

    //        var resultUrl = await _blobService.UploadFileAsync(
    //            stream,
    //            file.FileName,
    //            file.ContentType
    //        );

    //        return Ok(new
    //        {
    //            FileName = file.FileName,
    //            BlobUrl = resultUrl
    //        });
    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(500, $"Failed to upload image: {ex.Message}");
    //    }
    //}

}
