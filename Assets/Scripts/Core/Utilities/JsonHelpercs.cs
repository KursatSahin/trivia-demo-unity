using Newtonsoft.Json;

namespace Core.Utilities
{
    public static class JsonHelper
    {
        public static T ReadJson<T>(string jsonString)
        {
            // Read the json file
            // string jsonString = File.ReadAllText(filePath);

            // Deserialize the json string into a list of the specified generic class
            T deserializedData = JsonConvert.DeserializeObject<T>(jsonString);

            // Return the list
            return deserializedData;
        }

        public static string WriteJson(object data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}