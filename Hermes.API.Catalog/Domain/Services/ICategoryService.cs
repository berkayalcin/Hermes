using System.Threading.Tasks;
using Hermes.API.Catalog.Domain.Models;
using Hermes.API.Catalog.Domain.Requests;
using Hermes.API.Catalog.Domain.Responses;

namespace Hermes.API.Catalog.Domain.Services
{
    public interface ICategoryService
    {
        Task<CategoryDto> Create(CategoryDto categoryDto);
        Task<CategoryDto> Update(CategoryDto categoryDto);
        Task Delete(long id);
        Task<CategoryDto> Get(long id);
        Task<PagedResponse<CategoryDto>> GetAll(SearchCategoryRequest searchCategoryRequest);
    }
}