using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Notification
{
    public class ConnectionMapping<T>
    {
        private readonly Dictionary<T, List<UserConnection>> _connections =
            new Dictionary<T, List<UserConnection>>();

        public int Count
        {
            get { return _connections.Count; }
        }

        public void Add(T key, string connectionId)
        {
            lock (_connections)
            {
                List<UserConnection> userConnections;
                if (!_connections.TryGetValue(key, out userConnections))
                {
                    userConnections = new List<UserConnection> {new UserConnection(connectionId)};

                    _connections.Add(key, userConnections);
                }

                lock (userConnections)
                {
                    userConnections.Add(new UserConnection(connectionId));
                }
            }
        }

        public List<UserConnection> GetUserConnections(T key)
        {
            List<UserConnection> userConnections;
            if (_connections.TryGetValue(key, out userConnections))
            {
                return userConnections;
            }

            return new List<UserConnection>();
        }

        public void Remove(T key, string connectionId)
        {
            lock (_connections)
            {
                List<UserConnection> userConnections;
                if (!_connections.TryGetValue(key, out userConnections))
                {
                    return;
                }

                lock (userConnections)
                {
                    try
                    {
                        var indexConnectionInList = userConnections.FindIndex(c => c.ConnectionId == connectionId);
                        userConnections.RemoveAt(indexConnectionInList);
                    }
                    catch (ArgumentNullException)
                    {
                        return;
                    }
                    if (userConnections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }

        public class UserConnection { 
        
            public string ConnectionId;

            public DateTime ConnecTime;

            public UserConnection(string connectionId)
            {
                this.ConnectionId = connectionId;
                this.ConnecTime = DateTime.Now;
            }
        }

    }
}
