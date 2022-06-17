using StoreModels;
using StoreModels.File;
using StoreModels.Messages.Client;
using StoreModels.Messages.Server;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HostStore
{
    public class SessionStore
    {

        private ConcurrentDictionary<Guid, Session> _sessions;

        public SessionStore()
        {
            _sessions = new ConcurrentDictionary<Guid, Session>();
        }

        public ISession OpenSession()
        {
            Guid sessionId = Guid.NewGuid();
            AuthenticationManager pwm = new AuthenticationManager(sessionId);
            Session session = new Session(sessionId, pwm);

            _sessions[sessionId] = session;
            return session;
        }

        internal Session GetSession(Guid sessionId)
        {
            return _sessions[sessionId];
        }

        internal bool SessionExists(Guid sessionId)
        {
            return _sessions.ContainsKey(sessionId);
        }

        public void CloseSession(ISession session)
        {
            _sessions[session.SessionId].Close();
            _sessions.TryRemove(session.SessionId, out _);
        }

    }
}
