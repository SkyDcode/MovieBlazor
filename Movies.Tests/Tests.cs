using Xunit;
using Bunit;
using Moq;
using Blazored.LocalStorage;
using Movies.Services;
using Movies.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using Movies.Pages;


public class SaveComponentTests : TestContext
{
    public SaveComponentTests()
    {
        var mockLocalStorage = new Mock<ILocalStorageService>();
        var moviesList = new List<Movie>(); // Liste pour stocker les films simulés

        // Simuler GetItemAsync pour renvoyer la liste des films sérialisée
        mockLocalStorage.Setup(x => x.GetItemAsync<string>("movies", default))
                        .ReturnsAsync(() => JsonSerializer.Serialize(moviesList));

        // Simuler SetItemAsync pour mettre à jour la liste des films simulés
        mockLocalStorage.Setup(x => x.SetItemAsync("movies", It.IsAny<string>(), default))
                        .Callback<string, string, CancellationToken>((key, value, token) =>
                        {
                            moviesList = JsonSerializer.Deserialize<List<Movie>>(value) ?? new List<Movie>();
                        })
                        .Returns(new ValueTask());

        Services.AddSingleton<ILocalStorageService>(mockLocalStorage.Object);
        Services.AddSingleton<SavedMovieService>();
    }


    [Fact]
    public async Task SaveComponentRendersEmptyStateWhenNoMovies()
    {
        await Task.Yield();
        // Act
        var component = RenderComponent<Save>();

        // Assert
        Assert.Contains("No saved movies yet.", component.Markup);
    }

    [Fact]
    public async Task SavedMovieService_AddsMovieCorrectly()
    {
        // Arrange
        var movieToAdd = new Movie { Id = 3, Title = "Interstellar", Overview = "A team of explorers travel through a wormhole in space..." };
        var savedMovieService = Services.GetRequiredService<SavedMovieService>();

        // Act
        await savedMovieService.SaveMovieAsync(movieToAdd);

        // Assert
        var savedMovies = await savedMovieService.GetMoviesAsync();
        var savedMovie = savedMovies.FirstOrDefault(m => m.Id == movieToAdd.Id);

        Assert.NotNull(savedMovie); // Vérifiez que le film est bien ajouté
        Assert.Equal(movieToAdd.Title, savedMovie.Title); // Vérifiez que le titre du film ajouté est correct
        Assert.Equal(movieToAdd.Overview, savedMovie.Overview); // Vérifiez que le résumé du film ajouté est correct
    }
}
