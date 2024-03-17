using Blazored.LocalStorage;
using Movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Movies.Services
{
    public class SavedMovieService
    {
        private const string StorageKey = "movies";
        private readonly ILocalStorageService _localStorage;

        public SavedMovieService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task SaveMovieAsync(Movie movie)
        {
            try
            {
                var movies = await GetMoviesAsync();
                if (!movies.Any(m => m.Id == movie.Id))
                {
                    movies.Add(movie);
                    await SaveMoviesAsync(movies);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save movie: {ex.Message}");
                // Handle the error appropriately, e.g. show user-friendly message
            }
        }

        private async Task SaveMoviesAsync(List<Movie> movies)
        {
            var moviesJson = JsonSerializer.Serialize(movies);
            await _localStorage.SetItemAsync(StorageKey, moviesJson);
        }

        public async Task<List<Movie>> GetMoviesAsync()
        {
            var moviesJson = await _localStorage.GetItemAsync<string>(StorageKey);
            return string.IsNullOrEmpty(moviesJson) ? new List<Movie>() : JsonSerializer.Deserialize<List<Movie>>(moviesJson);
        }

        public async Task RemoveMovieAsync(int movieId)
        {
            var movies = await GetMoviesAsync();
            var movie = movies.FirstOrDefault(m => m.Id == movieId);
            if (movie != null)
            {
                movies.Remove(movie);
                await SaveMoviesAsync(movies);
            }
        }

        public async Task UpdateMovieTitleAsync(int movieId, string newTitle)
        {
            var movies = await GetMoviesAsync();
            var movie = movies.FirstOrDefault(m => m.Id == movieId);
            if (movie != null)
            {
                movie.Title = newTitle;
                await SaveMoviesAsync(movies);
            }
        }
    }
}
