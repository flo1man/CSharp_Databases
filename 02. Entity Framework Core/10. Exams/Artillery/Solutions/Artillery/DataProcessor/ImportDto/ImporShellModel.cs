using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Shell")]
    public class ImporShellModel
    {
        [XmlElement("ShellWeight")]
        [Range(2, 1_680)]
        public double ShellWeight { get; set; }

        [XmlElement("Caliber")]
        [MinLength(4)]
        [MaxLength(30)]
        public string Caliber { get; set; }
    }
}
