using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artillery.DataProcessor.ExportDto
{
    public class ExportShellModel
    {
        public double ShellWeight { get; set; }

        public string Caliber { get; set; }

        public ICollection<GunDto> Guns { get; set; }
    }

    [JsonObject("Guns")]
    public class GunDto
    {
        public string GunType { get; set; }

        public int GunWeight { get; set; }

        public double BarrelLength { get; set; }

        public string Range { get; set; }
    }
}
