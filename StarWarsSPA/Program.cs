using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StarWarsSPA;
using StarWarsSPA.Core.Interfaces;
using StarWarsSPA.Infrastructure.Services;
using StarWarsSPA.Presentation.ViewModels;
using Microsoft.JSInterop;

// Create the WebAssembly host builder
var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add the root component for the Blazor app to render in the DOM element with the ID "app"
builder.RootComponents.Add<App>("#app");

// Add a component that will render in the "head::after" slot in the HTML document to manage head elements
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register services with dependency injection
// Registering the SWAPI service (a service to interact with an external API)
builder.Services.AddScoped<ISwapiService, SwapiService>();

// Registering ViewModel services for different entities (Films, Planets, Species, Starships, Vehicles, and People)
builder.Services.AddScoped<FilmViewModel>();
builder.Services.AddScoped<FilmDetailsViewModel>();

builder.Services.AddScoped<PlanetViewModel>();
builder.Services.AddScoped<PlanetDetailsViewModel>();

builder.Services.AddScoped<SpeciesViewModel>();
builder.Services.AddScoped<SpecieDetailsViewModel>();

builder.Services.AddScoped<StarShipViewModel>();
builder.Services.AddScoped<StarShipDetailsViewModel>();

builder.Services.AddScoped<VehicleViewModel>();
builder.Services.AddScoped<VehicleDetailViewModel>();

builder.Services.AddScoped<PeopleViewModel>();
builder.Services.AddScoped<PeopleDetailViewModel>();

// Register the HttpClient service for making HTTP requests. It uses the base address of the application (the host environment)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


var host = builder.Build();
await host.RunAsync();

// Remove loader after app is fully initialized
var js = host.Services.GetRequiredService<IJSRuntime>();
var module = await js.InvokeAsync<IJSObjectReference>("import", "./js/loaderRemover.js");
await module.InvokeVoidAsync("removeLoader");



