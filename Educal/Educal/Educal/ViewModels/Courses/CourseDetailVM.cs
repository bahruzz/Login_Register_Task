﻿using Educal.Models;

namespace Educal.ViewModels.Courses
{
    public class CourseDetailVM
    {
        public string Name { get; set; }
        
        public decimal Price { get; set; }
        public string Category { get; set; }
        public List<CourseImageVM> Images { get; set; }
    }
}
