using System;
using System.Text.RegularExpressions;

namespace EscapeMines.IO.Extensions
{
    public static class ConfigStringExtensions
    {
        public static string NormalizeConfigLine(this string configLine, bool removeLineBreaks = false)
        {
            string cfg = removeLineBreaks ? Regex.Replace(configLine, @"\t|\n|\r", " ") : configLine;
            return Regex.Replace(cfg, "[ ]{2,}", " ").Trim();
        }

        public static T ParseToEnum<T>(this string raw)
        {
            if (raw.Length != 1 || !Enum.IsDefined(typeof(T), (int)raw[0]))
                throw new ArgumentException($"Unknown Turtle direction '{raw}'");

            return (T)Enum.ToObject(typeof(T), raw[0]);
        }
    }
}
