using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Artillery.DataProcessor.ImportDto
{
    public class ImportGunModel
    {
        public int ManufacturerId { get; set; }

        [Range(100, 1_350_000)]
        public int GunWeight { get; set; }

        [Range(2.00, 35)]
        public double BarrelLength { get; set; }

        public int? NumberBuild { get; set; }

        [Range(1, 100_000)]
        public int Range { get; set; }

        public string GunType { get; set; }

        public int ShellId { get; set; }

        public ICollection<CountryDto> Countries { get; set; }
    }

    [JsonObject("Countries")]
    public class CountryDto
    {
        public int Id { get; set; }
    }
}
