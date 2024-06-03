using Educal.Helpers.Extensions;
using Educal.Models;
using Educal.Services.Interfaces;
using Educal.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Educal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
       private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;
        public CategoryController(ICategoryService categoryService, IWebHostEnvironment env)
        {
            _categoryService = categoryService;

            _env = env;


        }
        public async Task<IActionResult> Index()
        {
            return View(await _categoryService.GetAlWithProductCountAsync());
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CategoryCreateVM category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool existCategory = await _categoryService.ExistAsync(category.Name);
            if (existCategory)
            {
                ModelState.AddModelError("Name", "This category already exist");
                return View();
            }
            if (!category.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Input accept only image format");
                return View();
            }
            if (!category.Image.CheckFileSize(500))
            {
                ModelState.AddModelError("Image", "Image size must be 500 kb");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "-" + category.Image.FileName;
            string path = _env.GenerateFilePath("assets/images", fileName);
            await category.Image.SaveFileToLocalAsync(path);
            await _categoryService.CreateAsync(new Category { Name = category.Name, Description = category.Description, Image = fileName });

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var category = await _categoryService.GetByIdAsync((int)id);
            if (category is null) return NotFound();
            string path = _env.GenerateFilePath("assets/images", category.Image);
            path.DeleteFileFromLocal();
            await _categoryService.DeleteAsync(category);
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            Category category = await _categoryService.GetByIdAsync((int)id);


            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var category = await _categoryService.GetByIdAsync((int)id);
            if (category == null)
            {
                return NotFound();
            }

            var viewModel = new CategoryEditVM
            {
                Image = category.Image,
                Description = category.Description,
                Name = category.Name
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, CategoryEditVM request)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var category = await _categoryService.GetByIdAsync((int)id);
            if (category == null)
            {
                return NotFound();
            }

            if (request.NewImage != null)
            {

                if (!request.NewImage.CheckFileType("image/"))
                {
                    ModelState.AddModelError("NewImage", "Accept only image format");
                    return View(request);
                }
                if (!request.NewImage.CheckFileSize(200))
                {
                    ModelState.AddModelError("NewImage", "Image size must be max 200 KB");
                    return View(request);
                }

                string oldPath = _env.GenerateFilePath("assets/images", category.Image);
                oldPath.DeleteFileFromLocal();
                string fileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;
                string newPath = _env.GenerateFilePath("assets/images", fileName);
                await request.NewImage.SaveFileToLocalAsync(newPath);
                category.Image = fileName;
            }

            category.Description = request.Description;
            category.Name = request.Name;

            await _categoryService.EditAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
