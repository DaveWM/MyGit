using Windows.Storage;

namespace MyGit.Services
{
    public interface ILocalStorageService
    {
        T Get<T>(string key);
        void Set<T>(string key, T value);
    }

    public class LocalStorageService : ILocalStorageService
    {
        public T Get<T>(string key)
        {
            var settings = ApplicationData.Current.LocalSettings;
            return settings.Values.ContainsKey(key) ? (T)settings.Values[key] : default(T);
        }

        public void Set<T>(string key, T value)
        {
            ApplicationData.Current.LocalSettings.Values[key] = value;
        }
    }
}
