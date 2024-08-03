using System;

namespace _Project.Scripts.Data
{
    [Serializable]
    public class EventData
    {
        public string Type;
        public string Data;

        public EventData(string type, string data)
        {
            Type = type;
            Data = data;
        }
    }
}