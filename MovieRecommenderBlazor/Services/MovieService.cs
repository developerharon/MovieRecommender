using MovieRecommenderBlazor.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace MovieRecommenderBlazor.Services
{
    public class MovieService
    {
        public List<Movie> Movies { get; set; }

        public MovieService()
        {
            Movies = GetMovies();
        }

        private readonly string MovieDataPath = Path.Combine(Directory.GetCurrentDirectory() + "\\Data\\" + "recommendation-movies.csv");

        private List<Movie> GetMovies()
        {
            var result = new List<Movie>();
            Stream fileReader = File.OpenRead(MovieDataPath);

            using (StreamReader reader = new StreamReader(fileReader))
            {
                bool header = true;
                int index = 0;
                var line = "";

                while (!reader.EndOfStream)
                {
                    if (header)
                    {
                        line = reader.ReadLine();
                        header = false;
                    }

                    line = reader.ReadLine();
                    string[] fields = line.Split(",");
                    int movieId = Int32.Parse(fields[0].ToString().TrimStart(new char[] { '0' }));
                    string movieTitle = fields[1].ToString();
                    string movieGenre = fields[2].ToString();
                    result.Add(new Movie() { Id = movieId, Title = movieTitle, Genre = movieGenre });
                    index++;
                }
            }

            return result;
        }
    }
}
