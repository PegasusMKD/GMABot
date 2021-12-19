using Newtonsoft.Json;
using System.Globalization;

namespace GMABot.Converters
{
    // For parsing TimeOnly from the JSON
    public class TimeOnlyConverter : JsonConverter<TimeOnly>
    {
        public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer) =>
            writer.WriteValue(value.ToString("hh:mm:ss", CultureInfo.InvariantCulture));

        public override TimeOnly ReadJson(JsonReader reader, Type objectType, TimeOnly existingValue, bool hasExistingValue, JsonSerializer serializer) =>
            TimeOnly.ParseExact(reader.Value as String, "hh:mm:ss", CultureInfo.InvariantCulture);
        
    }
}
