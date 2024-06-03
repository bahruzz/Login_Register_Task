using Educal.Models;
using Educal.ViewModels.Categories;
using Educal.ViewModels.Courses;
using Microsoft.EntityFrameworkCore;

namespace Educal.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseVM>> GetAllAsync();
        Task CreateAsync(Course course);
        Task<Course> GetByIdWithAllDatasAsync(int id);
        Task DeleteAsync(Course course);
        Task<Course> GetByIdAsync(int id);
        Task<IEnumerable<Course>> GetAllWithImagesAsync();


    }
}
