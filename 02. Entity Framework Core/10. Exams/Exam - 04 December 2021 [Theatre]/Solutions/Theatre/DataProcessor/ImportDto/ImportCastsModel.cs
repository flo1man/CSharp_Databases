using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Cast")]
    public class ImportCastsModel
    {
        public string FullName { get; set; }

        public bool IsMainCharacter { get; set; }

        public string PhoneNumber { get; set; }

        public int PlayId { get; set; }
    }
}
