using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Movies;
using Movies.Services;
using System;
using System.Net.Http;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// Configurez un client HTTP pour l'API externe
builder.Services.AddHttpClient("MovieAPI", client =>
{
    client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
});

// Enregistrez le MovieService en tant que Scoped, pour qu'il soit compatible avec HttpClient
builder.Services.AddScoped<MovieService>();

// Enregistrez d'autres services à portée si nécessaire
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<SavedMovieService>();

builder.Services.AddBlazoredLocalStorage();


await builder.Build().RunAsync();
