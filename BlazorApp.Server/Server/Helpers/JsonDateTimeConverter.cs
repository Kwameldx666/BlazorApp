using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace BlazorApp.Server.Helpers
{
    public class JsonDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Чтение даты в формате ISO 8601 (например, "2024-09-15T00:00:00Z")
            return DateTime.Parse(reader.GetString(), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Запись даты в формате ISO 8601
            writer.WriteStringValue(value.ToString("o"));
        }
    }
}
