using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.Models
{
    public class Car
    {
        public Car()
        {
            this.PartCars = new HashSet<PartCars>();
        }

        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public decimal TravelledDistance { get; set; }

        public ICollection<PartCars> PartCars { get; set; }
    }
}
