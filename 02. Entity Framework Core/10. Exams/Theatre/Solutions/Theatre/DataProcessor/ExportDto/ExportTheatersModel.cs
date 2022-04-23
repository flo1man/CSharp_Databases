using System;
using System.Collections.Generic;
using System.Text;

namespace Theatre.DataProcessor.ExportDto
{
    public class ExportTheatersModel
    {
        public string Name { get; set; }

        public sbyte Halls { get; set; }

        public decimal TotalIncome { get; set; }

        public ICollection<TicketsModel> Tickets { get; set; }
    }
    
    public class TicketsModel
    {
        public decimal Price { get; set; }

        public int RowNumber { get; set; }
    }
}
