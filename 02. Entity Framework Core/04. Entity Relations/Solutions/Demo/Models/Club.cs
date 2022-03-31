using System.Collections.Generic;

namespace Demo.Models
{
    public class Club
    {
        public Club()
        {
            this.EmployeesInClub = new HashSet<EmployeeInClub>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<EmployeeInClub> EmployeesInClub { get; set; }
    }
}
