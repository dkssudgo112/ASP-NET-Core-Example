using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;

namespace GrpcSessionService2.Helpers
{
    public class SessionManager
    {
        private readonly IMemoryCache _cache;
        private readonly ConcurrentDictionary<string, string> _sessionUserMap;

        public SessionManager(IMemoryCache cache)
        {
            _cache = cache;
            _sessionUserMap = new ConcurrentDictionary<string, string>();
        }

        public string CreateSession(string username)
        {
            var sessionId = Guid.NewGuid().ToString();
            _sessionUserMap[sessionId] = username;
            _cache.Set(sessionId, username, TimeSpan.FromMinutes(30));
            return sessionId;
        }

        public string GetUserFromSession(string sessionId)
        {
            if (_sessionUserMap.TryGetValue(sessionId, out var username) && _cache.TryGetValue(sessionId, out _))
            {
                return username;
            }
            return null;
        }
    }
}
