using AutoMapper;
using ProductShop.Models;
using ProductShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<GetProductsInRangeModel, Product>()
                .ForMember(x => x.Seller.FirstName + " " + x.Seller.LastName, y => y.MapFrom(s => s.SellerName));
        }
    }
}
