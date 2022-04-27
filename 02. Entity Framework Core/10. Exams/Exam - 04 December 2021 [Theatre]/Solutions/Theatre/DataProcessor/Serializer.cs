namespace Theatre.DataProcessor
{
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;
    using Theatre.XmlHelper;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theaters = context.Theatres
                .ToList()
                .Where(x => x.NumberOfHalls >= numbersOfHalls
                && x.Tickets.Count() >= 20)
                .Select(x => new ExportTheatersModel
                {
                    Name = x.Name,
                    Halls = x.NumberOfHalls,
                    TotalIncome = x.Tickets.Where(x => x.RowNumber >= 1 && x.RowNumber <= 5).Sum(x => x.Price),
                    Tickets = x.Tickets.Where(x => x.RowNumber >= 1 && x.RowNumber <= 5).Select(x => new TicketsModel
                    {
                        Price = x.Price,
                        RowNumber = x.RowNumber
                    })
                    .OrderByDescending(x => x.Price)
                    .ToList()

                })
                .OrderByDescending(x => x.Halls)
                .ThenBy(x => x.Name)
                .ToList();

            var json = JsonConvert.SerializeObject(theaters, Formatting.Indented);

            return json;
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            var plays = context.Plays
                .Include(x => x.Casts)
                .ToList()
                .Where(x => x.Rating <= rating)
                .Select(x => new ExportPlaysModel
                {
                    Title = x.Title,
                    Duration = x.Duration.ToString("c"),
                    Rating = x.Rating == 0 ? "Rating" : x.Rating.ToString(),
                    Genre = x.Genre.ToString(),
                    Actors = context.Casts.Where(c => c.IsMainCharacter && c.Play.Title == x.Title).Select(s => new ActorDto
                    {
                        FullName = s.FullName,
                        MainCharacter = $"Plays main character in '{x.Title}'."
                    })
                    .OrderByDescending(x => x.FullName)
                    .ToList()
                })
                .OrderBy(x => x.Title)
                .ThenByDescending(x => x.Genre)
                .ToList();

            var xml = XmlConverter.Serialize(plays, "Plays");

            return xml;
        }
    }
}
