
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.DataProcessor.ExportDto;
    using Artillery.XmlHelper;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System;
    using System.Linq;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shells = context.Shells
                .Include(x => x.Guns)
                .ToList()
                .Where(x => x.ShellWeight > shellWeight)
                .Select(x => new ExportShellModel
                {
                    ShellWeight = x.ShellWeight,
                    Caliber = x.Caliber,
                    Guns = x.Guns.Where(x => x.GunType.ToString() == "AntiAircraftGun").Select(x => new GunDto
                    {
                        GunType = x.GunType.ToString(),
                        GunWeight = x.GunWeight,
                        BarrelLength = x.BarrelLength,
                        Range = x.Range > 3000 ? "Long-range" : "Regular range",
                    })
                    .OrderByDescending(x => x.GunWeight)
                    .ToList()
                })
                .OrderBy(x => x.ShellWeight)
                .ToList();

            var json = JsonConvert.SerializeObject(shells, Formatting.Indented);

            return json;
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            var guns = context.Guns
                .Include(x => x.Manufacturer)
                .Include(x => x.CountriesGuns)
                .ThenInclude(x => x.Country)
                .ToArray()
                .Where(x => x.Manufacturer.ManufacturerName == manufacturer)
                .Select(x => new ExportGunModel
                {
                    Manufacturer = x.Manufacturer.ManufacturerName,
                    GunType = x.GunType.ToString(),
                    GunWeight = x.GunWeight,
                    BarrelLength = x.BarrelLength,
                    Range = x.Range,
                    Countries = x.CountriesGuns
                    .Where(x => x.Country.ArmySize > 4500000)
                    .Select(c => new CountryDto
                    {
                        Country = c.Country.CountryName,
                        ArmySize = c.Country.ArmySize,
                    })
                    .OrderBy(x => x.ArmySize)
                    .ToArray()
                })
                .OrderBy(x => x.BarrelLength)
                .ToArray();

            var xml = XmlConverter.Serialize(guns, "Guns");

            return xml;
        }
    }
}
