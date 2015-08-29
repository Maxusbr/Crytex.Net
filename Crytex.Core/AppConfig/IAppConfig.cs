namespace Crytex.Core.AppConfig
{
    public interface IAppConfig
    {
        string GetValue(string key);
        bool TryGetValue<T>(string key, out T result);
    }
}
