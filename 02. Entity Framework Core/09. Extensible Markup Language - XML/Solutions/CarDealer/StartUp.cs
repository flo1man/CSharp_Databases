using CarDealer.Data;
using CarDealer.Dtos.Export;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using CarDealer.XMLHelper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var db = new CarDealerContext();
            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();

            //var inputXml = File.ReadAllText("./Datasets/sales.xml");
            //var result = ImportSales(db, inputXml);

            //Console.WriteLine(result);

            Console.WriteLine(GetTotalSalesByCustomer(db));
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(x => new SalesDiscountModel
                {
                    Discount = x.Discount,
                    CustomerName = x.Customer.Name,
                    Price = x.Car.PartCars.Select(x => x.Part.Price).Sum(),
                    PriceWithDiscount = x.Car.PartCars.Select(s => s.Part.Price).Sum() - 
                        (x.Car.PartCars.Sum(x => x.Part.Price) * x.Discount / 100),
                    Car = new CarsWithPartsModel
                    {
                        Make = x.Car.Make,
                        Model = x.Car.Model,
                        TravelledDistance = x.Car.TravelledDistance,
                    }
                })
                .ToList();

            return XmlConverter.Serialize(sales, "sales");
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var sales = context.Customers
                .Where(x => x.Sales.Any())
                .Select(x => new TotalSalesModel
                {
                    FullName = x.Name,
                    BoughtCars = x.Sales.Count,
                    SpentMoney = x.Sales.Sum(x => x.Car.PartCars.Sum(x => x.Part.Price))
                })
                .OrderByDescending(x => x.SpentMoney)
                .ToList();


            return XmlConverter.Serialize(sales, "customers");
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(x => new CarsWithPartsModel
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance,
                    Parts = x.PartCars.Where(y => x.PartCars.Select(x => x.PartId).Contains(y.PartId)).Select(x => new PartsDto
                    {
                        Name = x.Part.Name,
                        Price = x.Part.Price,
                    })
                    .OrderByDescending(x => x.Price)
                    .ToList()
                })
                .OrderByDescending(x => x.TravelledDistance)
                .ThenBy(x => x.Model)
                .Take(5)
                .ToList();

            return XmlConverter.Serialize(cars, "cars");
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(x => !x.IsImporter)
                .Select(x => new LocalSuppliersModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count
                })
                .ToList();

            return XmlConverter.Serialize(suppliers, "suppliers");
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(x => x.Make == "BMW")
                .Select(x => new CarsFromBmwModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ToList();

            return XmlConverter.Serialize(cars, "cars");
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(x => x.TravelledDistance > 2000000)
                .Select(x => new CarsWithDistanceModel
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .OrderBy(x => x.Make)
                .ThenBy(x => x.Model)
                .Take(10)
                .ToList();

            return XmlConverter.Serialize(cars, "cars");
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var salesModel = XmlConverter.Deserializer<ImportSalesModel>(inputXml, "Sales");

            var carIds = context.Cars.Select(x => x.Id).ToList();

            var sales = salesModel
                .Where(x => carIds.Contains(x.CarId))
                .Select(x => new Sale
                {
                    CarId = x.CarId,
                    CustomerId = x.CustomerId,
                    Discount = x.Discount,
                })
                .ToList();

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var customersModel = XmlConverter.Deserializer<ImportCustomersModel>(inputXml, "Customers");

            var customers = customersModel
                .Select(x => new Customer
                {
                    Name = x.Name,
                    BirthDate = x.BirthDate,
                    IsYoungDriver = x.IsYoungDriver,
                })
                .ToList();
                

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var carsModel = XmlConverter.Deserializer<ImportCarsModel>(inputXml, "Cars");

            var cars = new List<Car>();

            foreach (var car in carsModel)
            {
                var partsIds = car.Parts
                    .Select(x => x.PartId)
                    .Distinct()
                    .Where(id => context.Parts.Any(x => x.Id == id))
                    .ToArray();

                var currCar = new Car()
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TraveledDistance,
                    PartCars = partsIds.Select(id => new PartCar()
                    {
                        PartId = id,
                    })
                    .ToArray()
                };

                cars.Add(currCar);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var partsModel = XmlConverter.Deserializer<ImportPartsModel>(inputXml, "Parts");

            var validIds = context.Suppliers.Select(x => x.Id).ToList();

            var parts = partsModel
                .Where(x => validIds.Contains(x.SupplierId))
                .Select(x => new Part
                {
                    Name = x.Name,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    SupplierId = x.SupplierId,
                })
                .ToList();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var suppliersModel = XmlConverter.Deserializer<ImportSuppliersModel>(inputXml, "Suppliers");

            var suppliers = suppliersModel
                .Select(x => new Supplier
                {
                    Name = x.Name,
                    IsImporter = x.IsImporter,
                })
                .ToList();

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }

    }
}