namespace CryptoTrader.Service.Services.Configuration
{
    /// <summary>
    /// A configuration management service interface that loads, saves, and configures keys in a Dictionary fashion.
    /// </summary>
    public interface IConfigService
    {
        bool TryLoad();
        bool TrySave();
        object this[string key] { get; }
    }
}
