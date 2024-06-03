
using Educal.Services.Interfaces;
using Educal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Educal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ICourseService _courseService;

        public HomeController(ICategoryService categoryService, ICourseService courseService)
        {
            _categoryService = categoryService;
            _courseService = courseService;
        }

        public async Task<IActionResult> Index()
        {

            HomeVM model = new HomeVM()
            {

                Categories = await _categoryService.GetAllAsync(),
                Courses = await _courseService.GetAllWithImagesAsync(),
            };

            return View(model);


        }




    }
}
