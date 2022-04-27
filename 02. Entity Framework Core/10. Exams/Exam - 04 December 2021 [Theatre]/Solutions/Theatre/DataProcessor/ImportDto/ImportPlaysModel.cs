using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Theatre.Data.Models.Enums;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Play")]
    public class ImportPlaysModel
    {
        public string Title { get; set; }

        public string Duration { get; set; }

        public float Rating { get; set; }

        public string Genre { get; set; }

        public string Description { get; set; }

        public string Screenwriter { get; set; }
    }
}
