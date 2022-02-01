using Newtonsoft.Json;

namespace SimpleAPIGateway.Utils
{
    /// <summary>
    /// Route Loader
    /// </summary>
    public class RouteLoader
    {
        /// <summary>
        /// Loads from file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static T LoadFromFile<T>(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string json = reader.ReadToEnd();
                T result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }
        }

        /// <summary>
        /// Deserializes the specified json object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonObject">The json object.</param>
        /// <returns></returns>
        public static T Deserialize<T>(object jsonObject)
        {
            return JsonConvert.DeserializeObject<T>(Convert.ToString(jsonObject));
        }
    }
}