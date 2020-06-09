using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int  Id { get; set; }
        [Required]
        [MaxLength(25 , ErrorMessage = "Employee name cannot exceed 25 characters")]
        [MinLength(5, ErrorMessage = "Employee name cannot be less than 5 characters")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "This is not a Valid email Format")]
        [Display(Name = "Office Email" )]
        public string Email { get; set; }
        [Required]
        public Dept?  Department { get; set; }

        public string PhotoPath { get; set; }
    }
}
