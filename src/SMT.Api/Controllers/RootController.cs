using Microsoft.AspNetCore.Mvc;
using SMT.ViewModel.Models;
using SMT.ViewModel.Dto.RootDto;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = nameof(GetRoot))]
        [ProducesResponseType(200)]
        public IActionResult GetRoot()
        {
            var response = new RootResponse
            {
                Self = Link.To(nameof(GetRoot)),
                Products = Link.To(nameof(ProductController.GetAll)),
            };

            return Ok(response);
        }
    }
}
