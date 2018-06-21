namespace CryptoTrader.Service.Utilities.Extensions
{
    /// <summary>
    /// A collection of extensions for the string type.
    /// </summary>
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);
    }
}
