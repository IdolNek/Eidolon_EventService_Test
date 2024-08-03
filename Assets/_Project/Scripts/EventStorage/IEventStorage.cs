using System.Collections.Generic;
using _Project.Scripts.Data;

namespace _Project.Scripts.EventStorage
{
    public interface IEventStorage
    {
        void SavePendingEvents(List<EventData> events);
        List<EventData> LoadPendingEvents();
    }
}