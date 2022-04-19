using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("Category")]
    public class GetCategoriesProductsModel
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("count")]
        public int CategoryProductsCount { get; set; }

        [XmlElement("averagePrice")]
        public decimal CategoryProductsAverage { get; set; }

        [XmlElement("totalRevenue")]
        public decimal CategoryProductsSum { get; set; }
    }
}
