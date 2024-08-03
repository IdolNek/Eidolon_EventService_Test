using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using _Project.Scripts.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace _Project.Scripts.EventSender
{
    public class EventSender : IEventSender
    {
        private readonly string serverUrl;

        public EventSender(string url)
        {
            serverUrl = url;
        }

        public async Task<bool> SendEventsAsync(List<EventData> events)
        {
            string jsonData = JsonUtility.ToJson(new EventList { Events = events });

            using (UnityWebRequest request = new UnityWebRequest(serverUrl, "POST"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                var operation = request.SendWebRequest();

                while (!operation.isDone)
                {
                    await Task.Yield();
                }

                return request.result == UnityWebRequest.Result.Success;
            }
        }
    }
}