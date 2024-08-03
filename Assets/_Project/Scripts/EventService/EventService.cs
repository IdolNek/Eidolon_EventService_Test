using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Scripts.Data;
using _Project.Scripts.EventSender;
using _Project.Scripts.EventStorage;
using UnityEngine;

namespace _Project.Scripts.EventService
{
    public class EventService : MonoBehaviour, IEventService
    {
        private IEventSender eventSender;
        private IEventStorage eventStorage;
        private float cooldownBeforeSend;

        private readonly Queue<EventData> eventQueue = new Queue<EventData>();
        private readonly List<EventData> pendingEvents = new List<EventData>();
        private bool isSending = false;

        public void Construct(IEventSender sender, IEventStorage storage, float cooldown)
        {
            eventSender = sender;
            eventStorage = storage;
            cooldownBeforeSend = cooldown;
        }

        public void Initialize()
        {
            List<EventData> loadedEvents = eventStorage.LoadPendingEvents();
            foreach (var evt in loadedEvents)
            {
                eventQueue.Enqueue(evt);
            }

            if (eventQueue.Count > 0) 
                SendEventsWithCooldownAsync();
        }

        public void TrackEvent(string type, string data)
        {
            var newEvent = new EventData(type, data);
            eventQueue.Enqueue(newEvent);

            if (!isSending) 
                SendEventsWithCooldownAsync();
        }

        private async Task SendEventsWithCooldownAsync()
        {
            isSending = true;

            while (eventQueue.Count > 0)
            {
                await Task.Delay((int)(cooldownBeforeSend * 1000));
                List<EventData> eventsToSend = new List<EventData>();

                while (eventQueue.Count > 0)
                {
                    eventsToSend.Add(eventQueue.Dequeue());
                }

                bool success = await eventSender.SendEventsAsync(eventsToSend);
                if (!success)
                {
                    pendingEvents.AddRange(eventsToSend);
                    eventStorage.SavePendingEvents(pendingEvents);
                }
                else
                    pendingEvents.Clear();
            }

            isSending = false;
        }

        private void OnApplicationQuit() => 
            eventStorage.SavePendingEvents(new List<EventData>(eventQueue));
    }
}