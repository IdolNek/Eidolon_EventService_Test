using System.Collections.Generic;
using System.IO;
using _Project.Scripts.Data;
using UnityEngine;

namespace _Project.Scripts.EventStorage
{
    public class EventStorage : IEventStorage
    {
        private readonly string _filePath;

        public EventStorage()
        {
            _filePath = Path.Combine(Application.persistentDataPath, "pendingEvents.json");
        }

        public void SavePendingEvents(List<EventData> events)
        {
            try
            {
                string json = JsonUtility.ToJson(new EventList { Events = events }, true);
                File.WriteAllText(_filePath, json);
            }
            catch (IOException ex)
            {
                Debug.LogError($"Failed to save events to disk: {ex.Message}");
            }
        }

        public List<EventData> LoadPendingEvents()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);
                    EventList eventList = JsonUtility.FromJson<EventList>(json);
                    return eventList.Events;
                }
            }
            catch (IOException ex)
            {
                Debug.LogError($"Failed to load events from disk: {ex.Message}");
            }

            return new List<EventData>();
        }
    }
}