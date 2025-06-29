using System.Text.Json;
using System.Text.Json.Serialization;

namespace LolLcdApiClient.Converters
{
    /// <summary>
    /// オブジェクトをカスタム変換するためのJsonConverter
    /// </summary>
    /// <typeparam name="T">変換するオブジェクトの型</typeparam>
    public class CustomObjectConverter<T> : JsonConverter<T> where T : new()
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                reader.Skip();
                return new T();
            }

            Utf8JsonReader readerClone = reader;
            using var jsonDoc = JsonDocument.ParseValue(ref readerClone);

            if (jsonDoc.RootElement.TryGetProperty("error", out _))
            {
                reader.Skip();
                return new T();
            }

            return JsonSerializer.Deserialize<T>(ref reader, options) ?? new T();
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}