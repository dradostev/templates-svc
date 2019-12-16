using System.Threading.Tasks;
using Upsaleslab.Templates.App.Events;

namespace Upsaleslab.Templates.App.Services
{
    public interface IEventService
    {
        Task PublishAsync<T>(Event<T> e);
    }
}