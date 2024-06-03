using Educal.Models;
using Educal.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection.Metadata;

namespace Educal.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryCourseVM>> GetAlWithProductCountAsync();
        Task<bool> ExistAsync(string name);
        Task CreateAsync(Category category);
        Task<Category> GetByIdAsync(int id);
        Task DeleteAsync(Category category);
        Task<IEnumerable<Category>> GetAllAsync();

        Task<SelectList> GetAllSelectedAsync();
        Task<Category> GetByIdWithCoursesAsync(int id);
        Task<bool> ExistExceptByIdAsync(int id, string name);
        Task EditAsync();
    }
}
