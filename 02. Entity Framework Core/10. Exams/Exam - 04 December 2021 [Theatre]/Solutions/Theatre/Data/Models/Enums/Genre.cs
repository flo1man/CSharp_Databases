using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Theatre.Data.Models.Enums
{
    public enum Genre
    {
        [XmlEnum("Drama")]
        Drama = 1,
        [XmlEnum("Comedy")]
        Comedy = 2,
        [XmlEnum("Romance")]
        Romance = 3,
        [XmlEnum("Musical")]
        Musical = 4,
    }
}
