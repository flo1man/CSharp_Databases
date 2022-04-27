namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;
    using Theatre.XmlHelper;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {

            var models = XmlConverter.Deserializer<ImportPlaysModel>(xmlString, "Plays");

            var plays = models
                .Select(x => new Play
                {
                    Title = x.Title,
                    Duration = TimeSpan.Parse(x.Duration, CultureInfo.InvariantCulture),
                    Rating = x.Rating,
                    Genre = Enum.TryParse(x.Genre, out Genre result) ? result : 0,
                    Description = x.Description,
                    Screenwriter = x.Screenwriter,
                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var item in plays)
            {
                if (IsPlayInvalid(item))
                {
                    sb.AppendLine(ErrorMessage);
                }
                else
                {
                    sb.AppendLine(String.Format(SuccessfulImportPlay, item.Title, item.Genre.ToString(), item.Rating));
                    context.Plays.Add(item);
                    context.SaveChanges();
                }
            }
            return sb.ToString().TrimEnd();
        }

        private static bool IsPlayInvalid(Play item)
        {
            return item.Title.Length < 4 || item.Title.Length > 50
                        || item.Duration.Hours < 1 || item.Rating < 0
                        || item.Rating > 10.00 || item.Description.Length > 700
                        || item.Description.Length < 1 || item.Screenwriter.Length < 4 
                        || item.Screenwriter.Length > 30 || item.Genre == 0;
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            var models = XmlConverter.Deserializer<ImportCastsModel>(xmlString, "Casts");

            var casts = models
                .Select(x => new Cast
                {
                    FullName = x.FullName,
                    IsMainCharacter = x.IsMainCharacter,
                    PhoneNumber = x.PhoneNumber,
                    PlayId = x.PlayId,
                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            Regex regex = new Regex(@"^\+44-\d{2}-\d{3}-\d{4}$");

            foreach (var item in casts)
            {
                Match match = regex.Match(item.PhoneNumber);

                if (item.FullName.Length < 4 || item.FullName.Length > 30
                    || !match.Success)
                {
                    sb.AppendLine(ErrorMessage);
                }
                else
                {
                    sb.AppendLine(String.Format(SuccessfulImportActor, item.FullName, item.IsMainCharacter ? "main" : "lesser"));
                    context.Casts.Add(item);
                    context.SaveChanges();
                }
            }
            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            var models = JsonConvert.DeserializeObject<List<ImportTheatreTicketsModel>>(jsonString);

            StringBuilder sb = new StringBuilder();

            foreach (var item in models)
            {
                var tickets = item.Tickets.Where(x => IsTicketValid(x, sb)).Select(x => new Ticket
                {
                    Price = x.Price,
                    RowNumber = x.RowNumber,
                    PlayId = x.PlayId,
                })
                .ToList();

                var theatre = new Theatre
                {
                    Name = item.Name,
                    NumberOfHalls = item.NumberOfHalls,
                    Director = item.Director,
                    Tickets = tickets,
                };

                if (theatre.Name.Length < 4 || theatre.Name.Length > 30
                    || theatre.NumberOfHalls < 1 || theatre.NumberOfHalls > 10
                    || theatre.Director.Length < 4 || theatre.Director.Length > 30)
                {
                    sb.AppendLine(ErrorMessage);
                }
                else
                {
                    sb.AppendLine(String.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count));
                    context.Tickets.AddRange(tickets);
                    context.Theatres.Add(theatre);
                    context.SaveChanges();
                }
            }
            return sb.ToString().TrimEnd();
        }

        public static bool IsTicketValid(TicketsDto ticket, StringBuilder sb)
        {
            bool isValid = ticket.Price >= 1 && ticket.Price <= 100
                    && ticket.RowNumber >= 1 && ticket.RowNumber <= 10;

            if (!isValid)
            {
                sb.AppendLine(ErrorMessage);
            }

            return isValid;
        }

        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
