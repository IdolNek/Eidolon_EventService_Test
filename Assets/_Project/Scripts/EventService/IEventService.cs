using _Project.Scripts.EventSender;
using _Project.Scripts.EventStorage;

namespace _Project.Scripts.EventService
{
    public interface IEventService
    {
        void TrackEvent(string type, string data);
        void Construct(IEventSender sender, IEventStorage storage, float cooldown);
        void Initialize();
    }
}