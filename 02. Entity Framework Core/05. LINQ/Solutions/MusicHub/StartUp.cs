namespace MusicHub
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = 
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            int producerId = 9;
            Console.WriteLine(ExportAlbumsInfo(context, producerId));

            int duration = 4;
            //Console.WriteLine(ExportSongsAboveDuration(context, duration));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Producers
                .FirstOrDefault(x => x.Id == producerId)
                .Albums
                .Select(x => new
                {
                    AlbumName = x.Name,
                    ReleaseDate = x.ReleaseDate,
                    ProducerName = x.Producer.Name,
                    Songs = x.Songs.Select(s => new
                    {
                        SongName = s.Name,
                        SongPrice = s.Price,
                        SongWriter = s.Writer.Name,
                    })
                    .OrderByDescending(x => x.SongName)
                    .ThenBy(x => x.SongWriter)
                    .ToList(),
                    AlbumPrice = x.Price,
                })
                .OrderByDescending(x => x.AlbumPrice)
                .ToList();

            StringBuilder sb = new StringBuilder();


            foreach (var album in albums)
            {
                int count = 1;
                sb.AppendLine($"-AlbumName: {album.AlbumName}")
                  .AppendLine($"-ReleaseDate: {album.ReleaseDate.ToString("MM/dd/yyyy")}")
                  .AppendLine($"-ProducerName: {album.ProducerName}")
                  .AppendLine("-Songs:");

                foreach (var song in album.Songs)
                {
                    sb.AppendLine($"---#{count}")
                      .AppendLine($"---SongName: {song.SongName}")
                      .AppendLine($"---Price: {song.SongPrice:f2}")
                      .AppendLine($"---Writer: {song.SongWriter}");

                    count++;
                }

                sb.AppendLine($"-AlbumPrice: {album.AlbumPrice:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .Select(x => new
                {
                    SongName = x.Name,
                    WriterName = x.Writer.Name,
                    AlbumProducer = x.Album.Producer.Name,
                    PerformerName = x.SongPerformers
                        .Select(s => s.Performer.FirstName + " " + s.Performer.LastName)
                        .FirstOrDefault(),
                    Duration = x.Duration,
                })
                .ToList()
                .Where(x => x.Duration > TimeSpan.FromSeconds(duration))
                .OrderBy(x => x.SongName)
                .ThenBy(x => x.WriterName)
                .ThenBy(x => x.PerformerName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            int count = 1;

            foreach (var song in songs)
            {
                sb.AppendLine($"-Song #{count}");
                sb.AppendLine($"---SongName: {song.SongName}");
                sb.AppendLine($"---Writer: {song.WriterName}");
                sb.AppendLine($"---Performer: {song.PerformerName}");
                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                sb.AppendLine($"---Duration: {song.Duration.ToString("c")}");

                count++;
            }

            return sb.ToString().TrimEnd();
        }
    }
}
