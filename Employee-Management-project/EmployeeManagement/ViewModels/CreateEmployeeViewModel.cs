using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;

namespace EmployeeManagement.ViewModels
{
    public class CreateEmployeeViewModel
    {
        [Required]
        [MaxLength(25, ErrorMessage = "Employee name cannot exceed 25 characters")]
        [MinLength(4, ErrorMessage = "Employee name cannot be less than 5 characters")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "This is not a Valid email Format")]
        [Display(Name = "Office Email")]
        public string Email { get; set; }
        [Required]
        public Dept? Department { get; set; }
        //[Required ]
        public IFormFile Photo { get; set; }
    }
}
