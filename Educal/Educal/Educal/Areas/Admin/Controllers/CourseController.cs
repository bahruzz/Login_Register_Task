using Educal.Helpers.Extensions;
using Educal.Models;
using Educal.Services.Interfaces;
using Educal.ViewModels.Categories;
using Educal.ViewModels.Courses;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace Educal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;

        public CourseController(ICategoryService categoryService,ICourseService courseService,IWebHostEnvironment env)
        {
            _categoryService = categoryService;
            _courseService = courseService;
            _env = env;
            
        }
        public async Task<IActionResult> Index()
        {
            return View(await _courseService.GetAllAsync());
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.categories = await _categoryService.GetAllSelectedAsync();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CourseCreateVM request)
        {
            ViewBag.categories = await _categoryService.GetAllSelectedAsync();


            if (!ModelState.IsValid)
            {
                return View();
            }
            

            foreach (var item in request.Images)
            {
                if (!item.CheckFileSize(500))
                {
                    ModelState.AddModelError("Image", "Image size must be max 500 KB");
                    return View();
                }

                if (!item.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Image", "File type must be only image");
                    return View();
                }

            }
            List<CourseImage> images = new();

            foreach (var item in request.Images)
            {
                string fileName = $"{Guid.NewGuid()}-{item.FileName}";
                string path = _env.GenerateFilePath("assets/images", fileName);
                await item.SaveFileToLocalAsync(path);
                images.Add(new CourseImage { Name = fileName });

            }

            images.FirstOrDefault().IsMain = true;

            Course course = new()
            {
                Name = request.Name,
                CategoryId = request.CategoryId,
                Price = decimal.Parse(request.Price),
                CourseImages = images

            };

            await _courseService.CreateAsync(course);


            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var existCourse = await _courseService.GetByIdWithAllDatasAsync((int)id);
            if (existCourse == null) return NotFound();

            foreach (var item in existCourse.CourseImages)
            {
                string path = _env.GenerateFilePath("assets/images", item.Name);
                path.DeleteFileFromLocal();
            }
            await _courseService.DeleteAsync(existCourse);
            return RedirectToAction(nameof(Index));

        }

      
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var existProduct = await _courseService.GetByIdWithAllDatasAsync((int)id);
            if (existProduct == null) return NotFound();

            List<CourseImageVM> images = new();

            foreach (var item in existProduct.CourseImages)
            {
                images.Add(new CourseImageVM
                {
                    Image = item.Name,
                    Ismain = item.IsMain
                });

            }
            CourseDetailVM response = new()
            {
                Name = existProduct.Name,
                Price = existProduct.Price,
                Category = existProduct.Category.Name,
                Images = images
            };
            return View(response);
        }

    }
}
