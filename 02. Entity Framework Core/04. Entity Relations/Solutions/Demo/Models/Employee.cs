using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Demo.Models
{
    public class Employee
    {
        // To use Attributes
        //using System.ComponentModel.DataAnnotations;
        //using System.ComponentModel.DataAnnotations.Schema;

        public Employee()
        {
            this.EmployeesInClub = new HashSet<EmployeeInClub>();
        }

        [Key]
        public int Id { get; set; }

        public int EID { get; set; }

        [Required]
        [MaxLength(25)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(35)]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName => $"{this.FirstName} {this.LastName}";

        public DateTime? StartWorkDate { get; set; }

        public decimal Salary { get; set; }

        // Optional
        public int DepartmentId { get; set; }  // Good to add

        // Required
        public Department Department { get; set; }

        public int? AddressId { get; set; }

        public Address Address { get; set; }

        public ICollection<EmployeeInClub> EmployeesInClub { get; set; }
    }
}
