using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MirrorOfErised.models.Repos;

namespace MirrorOfErised.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageEntryRepo _imageEntryRepo;

        public ImageController(IImageEntryRepo imageEntryRepo)
        {
            _imageEntryRepo = imageEntryRepo;
        }

        // GET api/image/{fileName}
        [HttpGet("{fileName}")]
        public async Task<IActionResult> ValidateImage(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return BadRequest();

            var image = await _imageEntryRepo.GetImageByName(fileName);
            if (image == null)
                return NotFound();

            if (image.IsValid)
                return Ok(true); // Image can be used for training
            
            return Ok(false);
        }
    }
}