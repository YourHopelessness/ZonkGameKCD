using System.Reflection;

namespace ZonkGameCore.ApiUtils
{
    /// <summary>
    /// Helper extensions for converting objects to dictionaries of parameters.
    /// </summary>
    public static class ParameterExtensions
    {
        public static Dictionary<string, object?> ToDictionaryBodyParameters<T>(this T obj) where T : class
        {
            ArgumentNullException.ThrowIfNull(obj);

            var dict = new Dictionary<string, object?>();
            foreach (PropertyInfo property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = property.GetValue(obj);
                dict[property.Name] = value;
            }

            return dict;
        }

        public static Dictionary<string, string> ToDictionaryQueryParameters<T>(this T obj) where T : class
        {
            ArgumentNullException.ThrowIfNull(obj);

            var dict = new Dictionary<string, string>();
            foreach (PropertyInfo property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = property.GetValue(obj);
                if (!string.IsNullOrEmpty(value?.ToString()))
                {
                    dict[property.Name] = value.ToString();
                }
            }

            return dict;
        }
    }
}