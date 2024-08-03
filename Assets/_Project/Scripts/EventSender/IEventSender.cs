using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Scripts.Data;

namespace _Project.Scripts.EventSender
{
    public interface IEventSender
    {
        Task<bool> SendEventsAsync(List<EventData> events);
    }
}