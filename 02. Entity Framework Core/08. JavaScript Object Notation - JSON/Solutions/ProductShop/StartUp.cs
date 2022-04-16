using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;
using ProductShop.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ProductShop
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var db = new ProductShopContext();
            Console.WriteLine(GetUsersWithProducts(db));
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var json = File.ReadAllText(inputJson);

            var users = JsonConvert.DeserializeObject<List<User>>(json);

            context.Users.AddRange(users);

            context.SaveChanges();

            return $"Successfully imported {context.Users.Count()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var json = File.ReadAllText(inputJson);

            var products = JsonConvert.DeserializeObject<List<Product>>(json);

            context.Products.AddRange(products);

            context.SaveChanges();

            return $"Successfully imported {context.Products.Count()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var json = File.ReadAllText(inputJson);

            var categories = JsonConvert.DeserializeObject<List<Category>>(json).Where(x => x.Name != null).ToList();

            context.Categories.AddRange(categories);

            context.SaveChanges();

            return $"Successfully imported {context.Categories.Count()}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var json = File.ReadAllText(inputJson);

            var categoryProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(json);

            context.CategoryProducts.AddRange(categoryProducts);

            context.SaveChanges();

            return $"Successfully imported {context.CategoryProducts.Count()}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .Select(x => new
                {
                    name = x.Name,
                    price = x.Price,
                    seller = x.Seller.FirstName + " " + x.Seller.LastName,
                })
                .OrderBy(x => x.price)
                .ToList();

            var json = JsonConvert.SerializeObject(products, Formatting.Indented);

            File.WriteAllText("../../../products-in-range.json", json);

            return json;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var products = context.Users
                .Where(x => x.SoldProducts.Count() > 0)
                .Select(x => new
                {
                    firstName = x.FirstName,
                    lastName = x.LastName,
                    SoldProducts = x.SoldProducts.Select(x => new
                    {
                        name = x.Name,
                        price = x.Price,
                        buyerFirstName = x.Buyer.FirstName,
                        buyerLastName = x.Buyer.LastName
                    })
                })
                .OrderBy(x => x.lastName)
                .ThenBy(x => x.firstName)
                .ToList();

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            };

            var json = JsonConvert.SerializeObject(products, Formatting.Indented, settings);

            File.WriteAllText("../../../users-sold-products.json", json);

            return json;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(x => new
                {
                    category = x.Name,
                    productsCount = x.CategoryProducts.Where(y => x.Id == y.CategoryId).Count(),
                    averagePrice = x.CategoryProducts.Where(y => x.Id == y.CategoryId).Select(x => x.Product.Price).Average().ToString("f2"),
                    totalRevenue = x.CategoryProducts.Where(y => x.Id == y.CategoryId).Select(x => x.Product.Price).Sum().ToString("f2"),
                })
                .OrderByDescending(x => x.productsCount)
                .ToList();

            var json = JsonConvert.SerializeObject(categories, Formatting.Indented);

            File.WriteAllText("../../../categories-by-products.json", json);

            return json;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.SoldProducts.Count() > 0)
                .Select(x => new
                {
                    usersCount = context.Users.Where(y => y.SoldProducts.Count() > 0).Count(),
                    users = new
                    {
                        firstName = x.FirstName,
                        lastName = x.LastName,
                        age = x.Age,
                        soldProcuts = x.SoldProducts.Select(y => new
                        {
                            count = x.SoldProducts.Count(),
                            products = x.SoldProducts.Select(s => new
                            {
                                name = s.Name,
                                price = s.Price.ToString("f2")
                            })
                        })
                    }
                })
                .OrderByDescending(x => x.users.soldProcuts.Count())
                .ToList();

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            };

            var json = JsonConvert.SerializeObject(users, Formatting.Indented, settings);

            File.WriteAllText("../../../users-and-products.json", json);

            return json;
        }
    }
}
