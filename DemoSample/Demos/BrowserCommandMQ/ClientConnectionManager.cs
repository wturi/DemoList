using System;
using System.Collections.Concurrent;

using Fleck;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrowserCommandMQ
{
    public class ClientConnectionManager
    {
        private ConcurrentDictionary<int, IWebSocketConnection> connections = new ConcurrentDictionary<int, IWebSocketConnection>();

        public void AddConnection(int port, IWebSocketConnection connection)
        {
            connections.TryAdd(port, connection);
        }

        public void RemoveConnection(int port)
        {
            if (connections.TryRemove(port, out var socketConnection))
                socketConnection?.Close();
        }

        public string GenerateMessageToClient(long returnId, string valueName, object obj)
        {
            var returnObj = new JObject { { "RetCode", 1 }, { "ReturnId", returnId }, { valueName, JToken.FromObject(obj) } };
            var returnStr = JsonConvert.SerializeObject(returnObj);
            return returnStr;
        }

        public void CloseConnections()
        {
            foreach (var item in connections)
            {
                try
                {
                    item.Value?.Close();
                }
                catch (Exception e)
                {
                }
            }
        }
    }
}