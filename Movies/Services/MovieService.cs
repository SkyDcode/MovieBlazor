using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Movies.Models; // Assurez-vous que cet espace de noms est correct
using System.Linq;
using Movies.Services;


public class MovieService
{
    private readonly HttpClient _httpClient;
    private readonly string apiKey = "598ef6e9834f67fd9a8bb03c46c07f80";
    private readonly List<Movie> savedMovies = new List<Movie>();

    public MovieService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Movie>> SearchMoviesAsync(string query)
    {
        var response = await _httpClient.GetFromJsonAsync<TmdbResponse>(
            $"https://api.themoviedb.org/3/search/movie?api_key={apiKey}&query={query}"
        );
        return response?.Results ?? new List<Movie>();
    }

    public void SaveMovie(Movie movie)
    {
        if (savedMovies.All(m => m.Id != movie.Id))
        {
            savedMovies.Add(movie);
        }
    }

    public List<Movie> GetSavedMovies()
    {
        return savedMovies;
    }

    private class TmdbResponse
    {
        public List<Movie> Results { get; set; }
    }

    public void UpdateMovieTitle(Movie movieToUpdate)
    {
        var movie = savedMovies.FirstOrDefault(m => m.Id == movieToUpdate.Id);
        if (movie != null)
        {
            movie.Title = movieToUpdate.Title;
            // Ici vous pouvez ajouter un code pour persister les modifications si nécessaire
        }
    }
}
