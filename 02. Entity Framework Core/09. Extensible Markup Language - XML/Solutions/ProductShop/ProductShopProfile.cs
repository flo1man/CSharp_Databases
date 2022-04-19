using AutoMapper;
using ProductShop.Dtos.Export;
using ProductShop.Models;
using System.Linq;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<Product, ExportProductModel>()
                .ForMember(x => x.BuyerName, y => y.MapFrom(x => x.Buyer.FirstName + " " + x.Buyer.LastName));

            CreateMap<User, GetSoldProductsModel>()
                .ForMember(x => x.ProductsSold, y => y.MapFrom(x => x.ProductsSold));

            CreateMap<Category, GetCategoriesProductsModel>()
                .ForMember(x => x.CategoryProductsAverage, y => y.MapFrom(s => s.CategoryProducts.Select(x => x.Product.Price).Average()))
                .ForMember(x => x.CategoryProductsSum, y => y.MapFrom(s => s.CategoryProducts.Select(x => x.Product.Price).Sum()));
        }
    }
}
