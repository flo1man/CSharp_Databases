using System;
using System.Collections.Generic;

#nullable disable

namespace Demo.Models
{
    public partial class Department
    {
        public Department()
        {
            DeletedEmployees = new HashSet<DeletedEmployee>();
            Employees = new HashSet<Employee>();
        }

        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public int ManagerId { get; set; }

        public virtual Employee Manager { get; set; }
        public virtual ICollection<DeletedEmployee> DeletedEmployees { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
