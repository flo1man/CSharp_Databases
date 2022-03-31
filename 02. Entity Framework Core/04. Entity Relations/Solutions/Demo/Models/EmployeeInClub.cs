using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Models
{
    public class EmployeeInClub
    {
        
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public int ClubId { get; set; }

        public Club Club { get; set; }

    }
}
