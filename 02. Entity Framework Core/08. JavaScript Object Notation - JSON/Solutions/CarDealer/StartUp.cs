using CarDealer.Data;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CarDealer
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var db = new CarDealerContext();

            Console.WriteLine(GetSalesWithAppliedDiscount(db));
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var json = File.ReadAllText(inputJson);

            var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(json);

            context.Suppliers.AddRange(suppliers);

            context.SaveChanges();

            return $"Successfully imported {context.Suppliers.Count()}";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var count = context.Suppliers.Count();
            var json = File.ReadAllText(inputJson);

            var parts = JsonConvert.DeserializeObject<List<Part>>(json).Where(x => x.SupplierId <= count).ToList();

            context.Parts.AddRange(parts);

            context.SaveChanges();

            return $"Successfully imported {context.Parts.Count()}";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var json = File.ReadAllText(inputJson);

            var cars = JsonConvert.DeserializeObject<List<Car>>(json);

            context.Cars.AddRange(cars);

            context.SaveChanges();

            return $"Successfully imported {context.Cars.Count()}";
        }

        public static string ImportCustomer(CarDealerContext context, string inputJson)
        {
            var json = File.ReadAllText(inputJson);

            var customers = JsonConvert.DeserializeObject<List<Customer>>(json);

            context.Customers.AddRange(customers);

            context.SaveChanges();

            return $"Successfully imported {context.Customers.Count()}";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var json = File.ReadAllText(inputJson);

            var sales = JsonConvert.DeserializeObject<List<Sale>>(json);

            context.Sales.AddRange(sales);

            context.SaveChanges();

            return $"Successfully imported {context.Sales.Count()}";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .Select(x => new
                {
                    x.Name,
                    BirthDate = x.BirthDate,
                    x.IsYoungDriver
                })
                .OrderBy(x => x.BirthDate)
                .ThenByDescending(x => x.IsYoungDriver)
                .ToList();

            var settings = new JsonSerializerSettings
            {
                DateFormatString = "dd/MM/yyyy"
            };

            var json = JsonConvert.SerializeObject(customers, Formatting.Indented, settings);

            File.WriteAllText("../../../ordered-customers.json", json);

            return json;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(x => x.Make == "Toyota")
                .Select(x => new
                {
                    x.Id,
                    x.Make,
                    x.Model,
                    x.TravelledDistance
                })
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ToList();

            var json = JsonConvert.SerializeObject(cars, Formatting.Indented);

            File.WriteAllText("../../../toyota-cars.json", json);

            return json;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(x => !x.IsImporter)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    PartsCount = x.Parts.Count()
                })
                .ToList();

            var json = JsonConvert.SerializeObject(suppliers, Formatting.Indented);

            File.WriteAllText("../../../local-suppliers.json", json);

            return json;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Include(x => x.PartCars)
                .ThenInclude(x => x.Part)
                .Select(x => new
                {
                    x.Make,
                    x.Model,
                    x.TravelledDistance,
                    parts = x.PartCars.Select(x => new
                    {
                        x.Part.Name,
                        Price = x.Part.Price.ToString("f2")
                    })
                    .ToList()
                })
                .ToList();

            var json = JsonConvert.SerializeObject(cars, Formatting.Indented);

            File.WriteAllText("../../../cars-and-parts.json", json);

            return json;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Select(x => new
                {
                    fullName = x.Name,
                    boughtCars = x.Sales.Count(),
                    spentMoney = x.Sales.Sum(y => y.Car.PartCars.Sum(s => s.Part.Price))
                })
                .OrderByDescending(x => x.spentMoney)
                .ThenByDescending(x => x.boughtCars)
                .ToList();

            var json = JsonConvert.SerializeObject(customers, Formatting.Indented);

            File.WriteAllText("../../../customers-total-sales.json", json);

            return json;
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Take(10)
                .Select(x => new
                {
                    x.Car.Make,
                    x.Car.Model,
                    x.Car.TravelledDistance,
                    x.Customer.Name,
                    x.Discount,
                    price = x.Car.PartCars.Sum(y => y.Part.Price).ToString("f2"),
                    priceWithoutDiscount = (x.Car.PartCars.Sum(y => y.Part.Price) - x.Car.PartCars.Sum(y => y.Part.Price) * x.Discount / 100).ToString("f2")
                })
                .ToList();

            var json = JsonConvert.SerializeObject(sales, Formatting.Indented);

            File.WriteAllText("../../../sales-discounts.json", json);

            return json;
        }
    }
}
