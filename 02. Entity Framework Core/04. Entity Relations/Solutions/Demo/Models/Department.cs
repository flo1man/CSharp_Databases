using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Models
{
    public class Department
    {
        public Department()
        {
            this.Employees = new HashSet<Employee>(); // Use HashSet !!!
        }

        public int Id { get; set; }

        public string Name { get; set; }

        // Optional - inverse property
        public ICollection<Employee> Employees { get; set; }
    }
}
