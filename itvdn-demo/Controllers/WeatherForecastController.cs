using Microsoft.AspNetCore.Mvc;

namespace itvdn_demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        int BadNonAssigned;
        object BadAssignedNull = null;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {

            // BAD CODE
            ICollection<WeatherForecastController> list = new List<WeatherForecastController>();
            list.Add(null);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

                [HttpPost]
        public void Post([FromBody] string value)
        {
            // BAD: straight up controller redirect
            Redirect(value);

            // BAD: Setting response headers collection, location = redirect
            Response.Headers["location"] = value;

            // GOOD: Setting response header to a constant value
            Response.Headers["location"] = "SomeValue";

            // BAD: Setting response headers collection, location = redirect via add method
            Response.Headers.Add("location", value);

            // GOOD: Setting response header to a constant value
            Response.Headers.Add("location", "foo");

            // BAD: redirect via location
            Response.Headers.SetCommaSeparatedValues("location", value);

            // BAD = redirect via setting location value from tainted source
            Response.Headers.Append("location", value);

            // BAD: redirect via setting location header from comma-separated values
            Response.Headers.AppendCommaSeparatedValues("location", value);

            // BAD: tainted redirect to Action
            RedirectToActionPermanent("Error" + value);
        }
    }
}