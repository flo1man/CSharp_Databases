using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Models;
using ProductShop.XmlHelper;

namespace ProductShop
{
    public class StartUp
    {
        static IMapper mapper;

        public static void Main(string[] args)
        {
            var db = new ProductShopContext();

            //string xml = File.ReadAllText("../../../Datasets/categories-products.xml");
            //Console.WriteLine(ImportCategoryProducts(db, xml));
            Console.WriteLine(GetUsersWithProducts(db));

        }

        private static void InitializeMapper()
        {
            var config = new MapperConfiguration(c =>
                            c.AddProfile(new ProductShopProfile()));

            mapper = config.CreateMapper();
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var users = XmlConverter.Deserializer<User>(inputXml, "Users");

            context.Users.AddRange(users);

            context.SaveChanges();

            return $"Successfully imported {users.Count()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var products = XmlConverter.Deserializer<Product>(inputXml, "Products");

            context.Products.AddRange(products);

            context.SaveChanges();

            return $"Successfully imported {products.Count()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var categories = XmlConverter.Deserializer<Category>(inputXml, "Categories");

            context.Categories.AddRange(categories);

            context.SaveChanges();

            return $"Successfully imported {categories.Count()}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var categoryProduct = XmlConverter.Deserializer<CategoryProduct>(inputXml, "CategoryProducts")
                .Where(x => context.Products.Any(y => y.Id == x.ProductId)
                       && context.Categories.Any(y => y.Id == x.CategoryId)).ToList();

            context.CategoryProducts.AddRange(categoryProduct);

            context.SaveChanges();

            return $"Successfully imported {categoryProduct.Count()}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .Select(x => new ExportProductModel
                {
                    Name = x.Name,
                    Price = x.Price,
                    BuyerName = x.Buyer.FirstName + " " + x.Buyer.LastName,
                })
                .OrderBy(x => x.Price)
                .Take(10)
                .ToList();

            return XmlConverter.Serialize(products, "Products");
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Any())
                .Select(x => new GetSoldProductsModel
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    ProductsSold = x.ProductsSold.Select(y => new SoldProductDto
                    {
                        Name = y.Name,
                        Price = y.Price
                    })
                    .ToList()
                })
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Take(5)
                .ToList();

            return XmlConverter.Serialize(users, "Users");
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var users = context.Categories
                .Select(x => new GetCategoriesProductsModel
                {
                    Name = x.Name,
                    CategoryProductsCount = x.CategoryProducts.Count,
                    CategoryProductsAverage = x.CategoryProducts.Average(a => a.Product.Price),
                    CategoryProductsSum = x.CategoryProducts.Sum(s => s.Product.Price),
                })
                .OrderByDescending(x => x.CategoryProductsCount)
                .ThenBy(x => x.CategoryProductsSum)
                .ToList();

            return XmlConverter.Serialize(users, "Categories");
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .ToList()
                .Where(x => x.ProductsSold.Any())
                .OrderByDescending(x => x.ProductsSold.Count)
                .Select(x => new GetUsersProductsModel
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Age = x.Age,
                    SoldProducts = new UserSoldProduct
                    {
                        Count = x.ProductsSold.Count,
                        Products = x.ProductsSold.Select(s => new SoldProductDto
                        {
                            Name = s.Name,
                            Price = s.Price
                        })
                        .OrderByDescending(s => s.Price)
                        .ToList()
                    }
                })
                .Take(10)
                .ToList();

            var mainResult = new FullModel
            {
                Count = context.Users.Where(x => x.ProductsSold.Any()).Count(),
                Users = users
            };

            return XmlConverter.Serialize(mainResult, "Users");

        }
    }
}