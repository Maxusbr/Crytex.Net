namespace Crytex.Core.AppConfig
{
    public interface IAppConfig
    {
        string GetValue(string key);
        T GetValue<T>(string key);
    }
}
