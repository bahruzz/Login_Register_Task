using System.ComponentModel.DataAnnotations;

namespace Educal.ViewModels.Courses
{
    public class CourseCreateVM
    {
        [Required(ErrorMessage = "This input can't be empty")]
        [StringLength(50)]
        public string Name { get; set; }


        public string Price { get; set; }
        public int CategoryId { get; set; }

        public List<IFormFile> Images { get; set; }
    }
}
