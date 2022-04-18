using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Demo
{
    [XmlType("doc")]
    public class Article
    {
        [XmlElement("abstract")]
        public string Abstract { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }
    }


    public class Program
    {
        static void Main(string[] args)
        {
            XDocument doc = XDocument.Load("{file name}");

            XDocument document = new XDocument();
            var root = new XElement("library");
            document.Add(root);

            for (int i = 0; i < 50; i++)
            {
                var book = new XElement("book");
                root.Add(book);
                book.Add(new XElement("title", i.ToString()));
                book.Add(new XElement("price", i + 10));
            }

            document.Save("library.xml");

            var serializer = new XmlSerializer(typeof(Program), new XmlRootAttribute("feed"));
            // var articles = (Program[])serializer.Deserialize(File.OpenRead("{file name}"));
            serializer.Serialize(File.OpenWrite("{file name}"), doc);

            var articles = document.Root.Elements()
                .Where(x => x.Element("{element}").Value.Contains("прог"));

            foreach (var item in articles)
            {
                Console.WriteLine(item.Element("{element}").Value);
            }
        }
    }
}
