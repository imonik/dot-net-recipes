using Microsoft.AspNetCore.Mvc;

namespace recipes_api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };


    private readonly ILogger<WeatherForecastController> _logger;
    private static List<Recipe> _recipesInMemoryStore = new List<Recipe>();

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost(Name = "CreateNewRecipe")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Recipe> Create(Recipe recipe)
    {
        var newRecipe = new Recipe
        {
            Id = _recipesInMemoryStore.Any() ? _recipesInMemoryStore.Max(r => r.Id) + 1 : 1,
            Name = recipe.Name,
            Description = recipe.Description,
            Ingredients = recipe.Ingredients,
            Directions = recipe.Directions
        };

        _recipesInMemoryStore.Add(recipe);

        return CreatedAtAction("CreateNewRecipe", new { id = recipe.Id }, recipe);
    }

}
