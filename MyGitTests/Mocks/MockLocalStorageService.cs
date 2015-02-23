using System.Collections.Generic;
using MyGit.Services;

namespace MyGitTests.Mocks
{
    public class MockLocalStorageService : ILocalStorageService
    {
        private readonly Dictionary<string, object> _storage = new Dictionary<string, object>(); 
        public T Get<T>(string key)
        {
            return _storage.ContainsKey(key) ? (T)_storage[key] : default(T);
        }

        public void Set<T>(string key, T value)
        {
            if (_storage.ContainsKey(key))
            {
                _storage[key] = value;
            }
            else
            {
                _storage.Add(key,value);
            }
        }
    }
}
