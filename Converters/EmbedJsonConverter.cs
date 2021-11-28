using GMABot.Models.Discord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMABot.Converters
{
    internal class EmbedJsonConverter : JsonConverter<EmbedType>
    {
        public override void WriteJson(JsonWriter writer, EmbedType value, JsonSerializer serializer) =>
            writer.WriteValue(EmbedConverter.GetText(value));

        public override EmbedType ReadJson(JsonReader reader, Type objectType, EmbedType existingValue, bool hasExistingValue, JsonSerializer serializer) =>
            EmbedConverter.GetType(reader.Value as String);

    }
}
