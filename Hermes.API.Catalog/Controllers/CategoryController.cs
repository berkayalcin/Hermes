using System.Threading.Tasks;
using Hermes.API.Catalog.Domain.Authentication;
using Hermes.API.Catalog.Domain.Constants;
using Hermes.API.Catalog.Domain.Models;
using Hermes.API.Catalog.Domain.Requests;
using Hermes.API.Catalog.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.Catalog.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        [HermesAuthorize(UserRoles.Administrator)]
        public async Task<IActionResult> Create([FromBody] CategoryDto category)
        {
            var categoryDto = await _categoryService.Create(category);
            return Ok(categoryDto);
        }

        [HttpPut("{id}")]
        [HermesAuthorize(UserRoles.Administrator)]
        public async Task<IActionResult> Update(long id, [FromBody] CategoryDto category)
        {
            var categoryDto = await _categoryService.Update(category);
            return Ok(categoryDto);
        }

        [HttpDelete("{id}")]
        [HermesAuthorize(UserRoles.Administrator)]
        public async Task<IActionResult> Delete(long id)
        {
            await _categoryService.Delete(id);
            return Ok();
        }

        [HttpGet("{id}")]
        [HermesAuthorize]
        public async Task<IActionResult> Get(long id)
        {
            var category = await _categoryService.Get(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpGet]
        [HermesAuthorize]
        public async Task<IActionResult> GetAll([FromQuery] SearchCategoryRequest searchCategoryRequest)
        {
            var categories = await _categoryService.GetAll(searchCategoryRequest);
            if (categories.Items == null || categories.Items.Count == 0)
            {
                return NotFound();
            }

            return Ok(categories);
        }
    }
}