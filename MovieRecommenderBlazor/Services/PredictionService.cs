using Microsoft.ML;
using MovieRecommender;
using MovieRecommenderBlazor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MovieRecommenderBlazor.Services
{
    public class PredictionService
    {
        private readonly MovieService _movieServices;

        public List<Movie> MoviesToPredict { get; set; }

        public PredictionService(MovieService movieServices)
        {
            _movieServices = movieServices;
            MoviesToPredict = new List<Movie>();
            MoviesToPredict = GetTwentyRandomMovies();
        }

        private readonly string ModelPath = Path.Combine(Directory.GetCurrentDirectory() + "\\Data\\" + "MovieRecommenderModel.zip");

        public float PredictMovie(int movieId, int userId)
        {
            MLContext mlContext = new MLContext();
            ITransformer mlModel = mlContext.Model.Load(ModelPath, out var modelInputSchema);

            var predictionEngine = mlContext.Model.CreatePredictionEngine<MovieRating, MovieRatingPrediction>(mlModel);

            var movie = _movieServices.Movies.FirstOrDefault(movie => movie.Id == movieId);
            var movieToPredict = new MovieRating { movieId = movie.Id, userId = userId };

            var movieRatingPrediction = predictionEngine.Predict(movieToPredict);

            return movieRatingPrediction.Score;
        }

        private List<Movie> GetTwentyRandomMovies()
        {
            var result = new List<Movie>();
            Random rnd = new Random();

            for (int i = 0; ; i++)
            {
                int movieId = rnd.Next(80000);

                Movie movie = _movieServices.Movies.FirstOrDefault(movie => movie.Id == movieId);

                if (movie != null)
                    result.Add(movie);

                if (result.Count() == 20)
                    break;
            }

            return result;
        }
    }
}
