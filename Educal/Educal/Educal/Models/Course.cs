namespace Educal.Models
{
    public class Course:BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
      
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<CourseImage> CourseImages { get; set; }

    }
}
