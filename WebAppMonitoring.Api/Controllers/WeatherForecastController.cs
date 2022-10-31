using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;

namespace WebAppMontoring.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly BlobServiceClient _blobServiceClient;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, BlobServiceClient blobServiceClient)
    {
        _logger = logger;
        _blobServiceClient = blobServiceClient;
    }

    [HttpGet]
    [Route("/")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    
    [HttpGet]
    [Route("/Failing")]
    public IEnumerable<WeatherForecast> GetFailingWeatherForecast()
    {
        return new[]
        {
            new WeatherForecast { Date = DateTime.Now, TemperatureC = 15, Summary = "Slecht weer"}
        };
    }
    
    [HttpGet]
    [Route("/news")]
    public string? GetNews()
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("news");
        var blobClient = containerClient.GetBlobClient("news.txt");

        var newsFile = blobClient.DownloadContent();
        var content = newsFile.Value.Content;

        return content.ToString();
    }
}
