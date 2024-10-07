using System.Text.Json;
using System.Text.Json.Serialization;

public class StringToEnumConverter<T> : JsonConverter<T> where T : struct, Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var enumText = reader.GetString();
            if (Enum.TryParse<T>(enumText, true, out var enumValue))
            {
                return enumValue;
            }
        }

        if (reader.TokenType == JsonTokenType.Number)
        {
            var enumValue = reader.GetInt32();
            if (Enum.IsDefined(typeof(T), enumValue))
            {
                return (T)Enum.ToObject(typeof(T), enumValue);
            }
        }

        // Caso o valor seja inválido, retorne o valor padrão do enum
        return default;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}