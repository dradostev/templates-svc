using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Upsaleslab.Templates.App.Events;
using Upsaleslab.Templates.App.Models;
using Upsaleslab.Templates.App.Services;

namespace Upsaleslab.Templates.App.Controllers
{
    [ApiController, Route("events")]
    [ApiExplorerSettings(IgnoreApi=true)]
    public class EventsController : Controller
    {
        private readonly IEventListener _eventListener;

        public EventsController(IEventListener eventListener)
        {
            _eventListener = eventListener;
        }

        [HttpPost("project-created:v1")]
        public async Task<ActionResult> On([FromBody] Event<ProjectCreated> e) =>
            await _eventListener.On(e) switch
            {
                Result.Successful => Accepted(),
                Result.NotFound => NotFound(),
                _ => (ActionResult) BadRequest()
            };

        [HttpPost("project-composition-created:v1")]
        public async Task<ActionResult> On([FromBody] Event<ProjectCompositionCreated> e) =>
            await _eventListener.On(e) switch
            {
                Result.Successful => Accepted(),
                Result.NotFound => NotFound(),
                _ => (ActionResult) BadRequest()
            };
    }
}
