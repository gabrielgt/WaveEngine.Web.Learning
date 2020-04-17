using System.Globalization;
using System.IO;
using System.Text.Json;
using WaveEngine.Mathematics;

namespace BetiJaiDemo
{
    public static class JsonHelper
    {
        public static T Deserialize<T>(string path)
        {
            var json = File.ReadAllText(path);
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var @object = JsonSerializer.Deserialize<T>(json, options);

            return @object;
        }

        public static float Parse(string value) => float.Parse(value, CultureInfo.InvariantCulture);

        public static Vector3 ParseVector3(string value)
        {
            var valueSplit = value.Split(',');

            return new Vector3(
                Parse(valueSplit[0]),
                Parse(valueSplit[1]),
                Parse(valueSplit[2]));
        }
    }
}