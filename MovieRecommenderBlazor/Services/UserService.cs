using MovieRecommenderBlazor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MovieRecommenderBlazor.Services
{
    public class UserService
    {
        public List<User> Users { get; set; }

        private readonly MovieService _movieServices;

        public UserService(MovieService movieServices)
        {
            User user1 = new User() { Id = 1, Name = "User 1", Movies = new List<Movie>() };
            User user2 = new User() { Id = 2, Name = "User 2", Movies = new List<Movie>() };
            User user3 = new User() { Id = 3, Name = "User 3", Movies = new List<Movie>() };

            Users = new List<User>();
            Users.Add(user1);
            Users.Add(user2);
            Users.Add(user3);

            _movieServices = movieServices;

            FillUserMovieList();
        }

        private readonly string MovieDataPath = Path.Combine(Directory.GetCurrentDirectory() + "\\Data\\" + "recommendation-ratings-test.csv");

        private void FillUserMovieList()
        {
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

                    foreach (var user in Users)
                    {
                        if (user.Id == Int32.Parse(fields[0].ToString().Trim()))
                        {
                            int movieId = Int32.Parse(fields[1].ToString().Trim());
                            user.Movies.Add(_movieServices.Movies.Single(movie => movie.Id == movieId));
                        }
                    }
                    index++;
                }
            }
        }
    }
}
