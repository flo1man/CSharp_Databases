using Demo.Models;
using System;
using System.Linq;

namespace Demo
{
    public class Program
    {
        static void Main(string[] args)
        {
            var db = new MusicXContext();

            // LEFT JOIN
            var songs = db.Songs
                .Select(x => new
                {
                    SongName = x.Name,
                    SourceName = x.Source.Name,
                })
                .ToList();

            // INNER JOIN
            var songsJoin = db.Songs.Join(
                db.Sources,
                x => x.SourceId,
                x => x.Id,
                (song, source) => new
                {
                    SongName = song.Name,
                    SourceName = source.Name,
                }).ToList();

            // GROUP BY
            var groups = db.Artists
                .GroupBy(x => x.Name.Substring(0, 1))
                .Select(x => new
                {
                    FirstLetter = x.Key,
                    Count = x.Count(),
                })
                .ToList();

            /* With .Select() => anonymous type
             * 1. (+) Navigation properties access in the lambda expression
             * 2. (+) Get only the columns we need
             * 3. (-) No Update/Delete
             * 
             * No .Select() => original entity
             * 1. (-) No access to navigational properties
             * 2. (-) Get all columns for entity
             * 3. (+) Can use Update/Delete / SaveChanges()
             */
        }
    }
}
