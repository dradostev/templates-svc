using System.Threading.Tasks;
using Upsaleslab.Templates.App.Events;
using Upsaleslab.Templates.App.Models;

namespace Upsaleslab.Templates.App.Services
{
    public interface IEventListener
    {
        Task<Result> On(Event<ProjectCreated> e);
        Task<Result> On(Event<ProjectCompositionCreated> e);
    }
}