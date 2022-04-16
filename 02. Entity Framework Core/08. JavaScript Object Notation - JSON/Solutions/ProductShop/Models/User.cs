using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductShop.Models
{
    public class User
    {
        public User()
        {
            this.BoughtProducts = new HashSet<Product>();
            this.SoldProducts = new HashSet<Product>();
        }

        public int Id { get; set; }

        public string FirstName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string LastName { get; set; }

        public int? Age { get; set; }

        public ICollection<Product> BoughtProducts { get; set; }

        public ICollection<Product> SoldProducts { get; set; }
    }
}
