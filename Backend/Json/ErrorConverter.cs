using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Errors;

namespace Backend.Json;

public class ErrorConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
        => typeof(IError).IsAssignableFrom(typeToConvert);

    public override JsonConverter? CreateConverter(
        Type typeToConvert, JsonSerializerOptions options)
    {
        // create a JsonConverter<T> where T : IError
        var converterType = typeof(ErrorConverter<>)
            .MakeGenericType(typeToConvert);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}

public class ErrorConverter<TError> : JsonConverter<TError>
    where TError : IError
{
    public override TError? Read(ref Utf8JsonReader reader,
        Type typeToConvert, JsonSerializerOptions options)
        => throw new NotSupportedException();

    public override void Write(Utf8JsonWriter writer, TError value,
        JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("code",    value.Code);
        writer.WriteString("message", value.Message);
        writer.WriteEndObject();
    }
}