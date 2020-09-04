using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace TheWorld.Services
{
    public class GeoCoordsService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<GeoCoordsService> _logger;
        public GeoCoordsService(IConfiguration config, ILogger<GeoCoordsService> logger)
        {
            _logger = logger;
            _config = config;
        }

        public async Task<GeoCoordsResult> GetCoordsAsync(string name)
        {
            var result = new GeoCoordsResult()
            {
                Success = false,
                Message = "Failed to get coordinates"
            };

            var apiKey = "ArVqnri5vBUElVnXKc7QzZ3-xnC4lWkSOF3e2kMjTP3piP-EplkDw3h_5TXbVNG5";//_config["Keys:BingKey"];
            var encodedName = WebUtility.UrlEncode(name);
            var url = $"http://dev.virtualearth.net/REST/v1/Locations/US/WA/98052/Redmond/1%20Microsoft%20Way?o=xml&key={apiKey}";

            try
            {
                // Read out the results
                // Fragile, might need to change if the Bing API changes
                var client = new HttpClient();
                var json = await client.GetStringAsync(url);

                json = @"{
                        CPU: 'Intel',
                        Drives: [
                            'DVD read/writer',
                            '500 gigabyte hard drive'
                        ]
                        }";

                var results = JObject.Parse(json);
                var resources = results["resourceSets"][0]["resources"];
                if (!resources.HasValues)
                {
                    result.Message = $"Could not find '{name}' as a location";
                }
                else
                {
                    var confidence = (string)resources[0]["confidence"];
                    if (confidence != "High")
                    {
                        result.Message = $"Could not find a confident match for '{name}' as a location";
                    }
                    else
                    {
                        var coords = resources[0]["geocodePoints"][0]["coordinates"];
                        result.Latitude = (double)coords[0];
                        result.Longitude = (double)coords[1];
                        result.Success = true;
                        result.Message = "Success";
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return result;
            }

            return result;
        }
    }
}