using FreeCourse.Services.Catalog.Dto;
using FreeCourse.Shared.Dtos;

namespace FreeCourse.Services.Catalog.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Response<List<CategoryDto>>> GetAllAsync();

        Task<Response<CategoryDto>> CreateAsync(CategoryCreateDto categoryCreateDto);

        Task<Response<CategoryDto>> GetByIdAsync(string id);
    }
}
