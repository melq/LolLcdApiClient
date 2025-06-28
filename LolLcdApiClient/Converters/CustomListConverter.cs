using System.Text.Json;
using System.Text.Json.Serialization;

namespace LolLcdApiClient.Converters
{
    public class CustomListConverter<T> : JsonConverter<List<T>>
    {
        public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                return JsonSerializer.Deserialize<List<T>>(ref reader, options) ?? [];
            }

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                reader.Skip();
            }

            return [];
        }

        public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
        {
            // 今回は書き込み処理は使わないので、デフォルトの処理を呼び出す
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}