using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Xml;

namespace Demo
{
    public class Program
    {
        /* .NET built-in JSON support -> System.Text.Json (NuGet Package)
           * using namespaces are -> 
           * using System.Text.Json
           * using System.Text.Json.Serialization
        */
        // Second JSON package is -> Newtonsoft.Json

        static void Main(string[] args)
        {
            var car = new Car
            {
                Extras = new List<string> { "4x4", "Klimatronik" },
                ManufacturedOn = DateTime.Now,
                Model = "Golf",
                Manufacturer = "VW",
                Price = 12345.67m,
                Engine = new Engine { HorsePower = 322, Volume = 1.6m }
            };

            // Write the object into Json File
            //File.WriteAllText("myCar.json", JsonSerializer.Serialize(car));

            // Create options for JsonSerializer
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            //Console.WriteLine(JsonSerializer.Serialize(car, options));

            // Read Json File
            var json = File.ReadAllText("myCar.json");

            var jObj = JObject.Parse(json);

            // Create car from Deserialize
            //Car newCar = JsonSerializer.Deserialize<Car>(json);

            var xmlDoc = JsonConvert.DeserializeXmlNode(json);
            var xml = new XmlDocument();
            var root = xml.CreateElement("test");
            xml.AppendChild(root);
            root.AppendChild(xml.CreateElement("test2"));
            root.AppendChild(xml.CreateElement("test3"));

            JsonConvert.SerializeXmlNode(xml,Newtonsoft.Json.Formatting.Indented);

            // If you want to parse JSON/XML file to classes ->
            // Edit -> Paste Special -> Paste JSON/XML as classes

            /* Newtonsoft.Json !
            var set = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new DefaultNamingStrategy(),
                }
                DateFormatString = "yyyy-MM-dd",
            };

            Console.WriteLine(JsonConvert.SerializeObject());
            */

            // CSV NuGet Package -> CsvHelper
        }
    }
}
