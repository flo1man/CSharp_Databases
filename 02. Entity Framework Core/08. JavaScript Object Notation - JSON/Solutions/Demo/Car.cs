using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Demo
{
    public class Car
    {
        [JsonPropertyName("MF")]
        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public decimal Price { get; set; }

        public DateTime ManufacturedOn { get; set; }

        [JsonIgnore]
        public List<string> Extras { get; set; }

        public Engine Engine { get; set; }
    }
}
