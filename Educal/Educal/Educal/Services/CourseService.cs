using Educal.Data;
using Educal.Models;
using Educal.Services.Interfaces;
using Educal.ViewModels.Categories;
using Educal.ViewModels.Courses;
using Microsoft.EntityFrameworkCore;

namespace Educal.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;

        public CourseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Course course)
        {
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(Course course)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CourseVM>> GetAllAsync()
        {
            IEnumerable<Course> courses = await _context.Courses.Include(m => m.Category).Include(m => m.CourseImages).ToListAsync();
            return courses.Select(m => new CourseVM
            {
                Id = m.Id,
                Name = m.Name,
                CategoryName = m.Category.Name,
                Price = m.Price,
                CreatedDate= m.CreatedDate.ToString("MM.dd.yyyy"),


            });
        }

        public async Task<IEnumerable<Course>> GetAllWithImagesAsync()
        {
            return await _context.Courses
                .Include(m=>m.Category)
                .Include(m => m.CourseImages)
                .ToListAsync();
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            var b = await _context.Courses.Where(m => m.Id == id).FirstOrDefaultAsync();
            return b;
        }

        public async Task<Course> GetByIdWithAllDatasAsync(int id)
        {
            return await _context.Courses.Where(m => m.Id == id)
                                          .Include(m => m.Category)
                                          .Include(m => m.CourseImages)
                                          .FirstOrDefaultAsync();

        }
    }
}
