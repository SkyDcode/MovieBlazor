using Xunit;
using Bunit;
using Moq;
using Blazored.LocalStorage;
using Movies.Services;
using Movies.Models;
using Movies.Pages;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SaveComponentTests : TestContext
{
    public SaveComponentTests()
    {
        // Créer un mock pour ILocalStorageService
        var mockLocalStorage = new Mock<ILocalStorageService>();

        // Configurer le mock pour renvoyer une liste vide pour GetItemAsync lorsqu'il est appelé avec "movies"
        mockLocalStorage.Setup(x => x.GetItemAsync<string>("movies", default))
                        .ReturnsAsync(string.Empty);

        // Configurer le mock pour gérer SetItemAsync lorsqu'il est appelé avec "movies"
        mockLocalStorage.Setup(x => x.SetItemAsync<string>("movies", It.IsAny<string>(), default))
                .Returns(new ValueTask()); // Utilisez ValueTask.CompletedTask si disponible

        // Utiliser le mock dans les services
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
    public async Task SaveComponentRendersMoviesCorrectly()
    {
        await Task.Yield();
        // Arrange
        var localStorage = Services.GetRequiredService<ILocalStorageService>();
        var moviesList = new List<Movie>
        {
            new Movie { Id = 1, Title = "Inception", Overview = "A thief with the rare ability..." },
            new Movie { Id = 2, Title = "The Matrix", Overview = "A computer hacker learns from mysterious rebels..." }
        };

        var moviesJson = System.Text.Json.JsonSerializer.Serialize(moviesList);

        // Configurer le mock pour renvoyer `moviesJson` lors de l'appel à GetItemAsync
        var mockLocalStorage = Mock.Get(localStorage);
        mockLocalStorage.Setup(x => x.GetItemAsync<string>("movies", default))
                        .ReturnsAsync(moviesJson);

        // Act
        var component = RenderComponent<Save>();

        // Assert
        Assert.Equal(2, component.FindAll("tbody tr").Count); // Vérifier que deux films sont affichés
        Assert.Contains("Inception", component.Markup);
        Assert.Contains("The Matrix", component.Markup);
    }
}
