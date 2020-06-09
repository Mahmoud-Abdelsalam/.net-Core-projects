using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "Eddie Collier",
                    Email = "Eddie@gmail.com",
                    Department = Dept.Development

                },
            new Employee
                {
                    Id = 2,
                    Name = "Nell Baldwin",
                    Email = "Nell@gmail.com",
                    Department = Dept.Design

                },
            new Employee
                {
                    Id = 3,
                    Name = "Adrian Murray",
                    Email = "Adrian@gmail.com",
                    Department = Dept.Security

                }
            );
        }
    }
}
