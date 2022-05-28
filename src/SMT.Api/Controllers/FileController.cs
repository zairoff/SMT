using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SMT.Services.Interfaces.FileSystem;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly IConfiguration _configuration;
        private readonly string _imagesFolder;
        private readonly string _requestPath;

        public FileController(IImageService imageService, IConfiguration configuration)
        {
            _imageService = imageService;
            _configuration = configuration;
            _imagesFolder = _configuration["AppSettings:ImagesFolder"];
            _requestPath = _configuration["AppSettings:RequestPath"];
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveImage(IFormFile file)
        {
            var result = await _imageService.SaveAsync(file, _imagesFolder);

            return CreatedAtAction(nameof(LoadImage), new { fileName = result }, result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult LoadImage(string fileName)
        {
            var result = _imageService.LoadUrl(_requestPath, fileName);

            if (string.IsNullOrEmpty(result))
                return NotFound();

            return Ok(new { Url = result });
        }
    }
}
