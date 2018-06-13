using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Service.Utilities.Extensions
{
    /// <summary>
    /// A collection of extensions for the string type.
    /// </summary>
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string s) => s == null || s == String.Empty;
    }
}
