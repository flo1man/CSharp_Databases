namespace Artillery.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Artillery.XmlHelper;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage =
                "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            var models = XmlConverter.Deserializer<ImportCountryModel>(xmlString, "Countries").ToList();

            StringBuilder sb = new StringBuilder();

            List<Country> countries = new List<Country>();

            foreach (var item in models)
            {
                if (IsValid(item))
                {
                    sb.AppendLine(String.Format(SuccessfulImportCountry, item.CountryName, item.ArmySize));

                    countries.Add(new Country
                    {
                        CountryName = item.CountryName,
                        ArmySize = item.ArmySize
                    });
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            context.Countries.AddRange(countries);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            var models = XmlConverter.Deserializer<ImportManufacturerModel>(xmlString, "Manufacturers").ToList();

            StringBuilder sb = new StringBuilder();

            List<Manufacturer> manufacturers = new List<Manufacturer>();

            foreach (var item in models)
            {
                var manufacturer = new Manufacturer
                {
                    ManufacturerName = item.ManufacturerName,
                    Founded = item.Founded,
                };

                if (IsValid(manufacturer) && !manufacturers.Any(x => x.ManufacturerName == manufacturer.ManufacturerName))
                {
                    var foundedLocation = manufacturer.Founded.Split(", ");

                    sb.AppendLine(String.Format(SuccessfulImportManufacturer
                        , manufacturer.ManufacturerName
                        , foundedLocation[foundedLocation.Length - 2] + ", " + foundedLocation[foundedLocation.Length - 1]));

                    manufacturers.Add(manufacturer);
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            context.Manufacturers.AddRange(manufacturers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            var models = XmlConverter.Deserializer<ImporShellModel>(xmlString, "Shells").ToList();

            StringBuilder sb = new StringBuilder();

            List<Shell> shells = new List<Shell>();

            foreach (var item in models)
            {
                if (IsValid(item))
                {
                    sb.AppendLine(String.Format(SuccessfulImportShell, item.Caliber, item.ShellWeight));

                    shells.Add(new Shell
                    {
                        Caliber = item.Caliber,
                        ShellWeight = item.ShellWeight
                    });
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            context.Shells.AddRange(shells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            var models = JsonConvert.DeserializeObject<List<ImportGunModel>>(jsonString);

            StringBuilder sb = new StringBuilder();

            List<Gun> guns = new List<Gun>();

            foreach (var item in models)
            {
                if (IsValid(item) && Enum.TryParse(item.GunType, out GunType gunType))
                {
                    var gun = new Gun
                    {
                        ManufacturerId = item.ManufacturerId,
                        GunWeight = item.GunWeight,
                        BarrelLength = item.BarrelLength,
                        NumberBuild = item.NumberBuild,
                        Range = item.Range,
                        GunType = gunType,
                        ShellId = item.ShellId,
                        CountriesGuns = item.Countries.Select(x => new CountryGun
                        {
                            CountryId = x.Id
                        })
                        .ToList()
                    };

                    guns.Add(gun);

                    sb.AppendLine(String.Format(SuccessfulImportGun, gun.GunType.ToString(), gun.GunWeight, gun.BarrelLength));
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            context.Guns.AddRange(guns);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
