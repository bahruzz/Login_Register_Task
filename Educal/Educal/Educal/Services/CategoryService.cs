using Educal.Data;
using Educal.Models;
using Educal.Services.Interfaces;
using Educal.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Educal.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Category category)
        {
            await _context.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(string name)
        {
            return await _context.Categories.AnyAsync(m => m.Name.Trim() == name.Trim());
        }

        public async Task<bool> ExistExceptByIdAsync(int id, string name)
        {
            return await _context.Categories.AnyAsync(m => m.Name == name && m.Id != id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<SelectList> GetAllSelectedAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return new SelectList(categories, "Id", "Name");
        }

        public async Task<IEnumerable<CategoryCourseVM>> GetAlWithProductCountAsync()
        {
            IEnumerable<Category> categories = await _context.Categories.Include(m => m.Courses).OrderByDescending(m => m.Id)
                                                                        .ToListAsync();
            return categories.Select(m => new CategoryCourseVM
            {
                Id = m.Id,
                CategoryName = m.Name,
                CategoryDescription = m.Description,
                CreatedDate = m.CreatedDate.ToString("MM.dd.yyyy"),
                CourseCount = m.Courses.Count,
            });
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var a = await _context.Categories.Where(m => m.Id == id).FirstOrDefaultAsync();
            return a;
        }

        public async Task<Category> GetByIdWithCoursesAsync(int id)
        {
            return await _context.Categories.Include(m => m.Courses).ThenInclude(m => m.CourseImages).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
