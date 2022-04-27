using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Theatre.DataProcessor.ImportDto
{
    public class ImportTheatreTicketsModel
    {
        public string Name { get; set; }

        public sbyte NumberOfHalls { get; set; }

        public string Director { get; set; }

        public ICollection<TicketsDto> Tickets { get; set; }
    }

    [JsonObject("Tickets")]
    public class TicketsDto
    {
        public decimal Price { get; set; }

        public sbyte RowNumber { get; set; }

        public int PlayId { get; set; }
    }
}



