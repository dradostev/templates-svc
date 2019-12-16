using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Upsaleslab.Templates.App.Events;

namespace Upsaleslab.Templates.App.Services
{
    public class EventService : IEventService
    {
        private readonly ILogger<EventService> _logger;
        private readonly string _endpoint;

        public EventService(IConfiguration config, ILogger<EventService> logger)
        {
            _logger = logger;
            _endpoint = config["MQ_SIDECAR_ENDPOINT"];
        }

        public async Task PublishAsync<T>(Event<T> e)
        {
            using var http = new HttpClient();

            var content = new StringContent(JsonConvert.SerializeObject(e));

            var res = await http.PostAsync(
                new Uri(_endpoint), 
                content);

            if (!res.IsSuccessStatusCode)
            {
                _logger.LogError(
                    $"Error publishing event {e.Type} v{e.Version} with error {res.StatusCode}");
                return;
            }
            
            _logger.LogInformation($"Event {e.Type} v{e.Version} has been published");
        }
    }
}