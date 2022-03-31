using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Models
{
    public class Address
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }
    }
}
