using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF.UtilityBot.Module11.Models;

namespace SF.UtilityBot.Module11.Services
{
    public class MemoryStorage : IStorage
    {
        private readonly ConcurrentDictionary<long, Session> _sessions;

        public MemoryStorage()
        {
            _sessions = new ConcurrentDictionary<long, Session>();
        }
        public Session GetSession(long chatId)
        {
            if (_sessions.ContainsKey(chatId))
            {
                return _sessions[chatId];
            }

            var newSession = new Session() { taskCode = "tsk1"};
            _sessions.TryAdd(chatId, newSession);
            return newSession;
        }
    }
}
