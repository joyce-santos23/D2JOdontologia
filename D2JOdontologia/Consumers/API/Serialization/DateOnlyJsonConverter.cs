using System.Text.Json;
using System.Text.Json.Serialization;

namespace Consumers.API.Serialization
{
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private const string Format = "yyyy-MM-dd";

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    // Lê o objeto composto { year, month, day }
                    int year = 0, month = 0, day = 0;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject)
                        {
                            break;
                        }

                        if (reader.TokenType == JsonTokenType.PropertyName)
                        {
                            string propertyName = reader.GetString();
                            reader.Read();

                            switch (propertyName)
                            {
                                case "year":
                                    year = reader.GetInt32();
                                    if (year < 1000 || year > 9999)
                                        throw new JsonException("Invalid year. The year must have 4 digits.");
                                    break;
                                case "month":
                                    month = reader.GetInt32();
                                    if (month < 1 || month > 12)
                                        throw new JsonException("Invalid month. The month must be between 1 and 12.");
                                    break;
                                case "day":
                                    day = reader.GetInt32();
                                    if (day < 1 || day > 31)
                                        throw new JsonException("Invalid day. The day must be between 1 and 31.");
                                    break;
                                default:
                                    throw new JsonException($"Unexpected property name: {propertyName}");
                            }
                        }
                    }

                    if (year > 0 && month > 0 && day > 0)
                    {
                        return new DateOnly(year, month, day);
                    }

                    throw new JsonException("Incomplete DateOnly object.");
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    // Lê no formato "yyyy-MM-dd"
                    var dateString = reader.GetString();
                    if (dateString.Length != 10)
                        throw new JsonException("Invalid date string. Expected format: yyyy-MM-dd.");

                    return DateOnly.ParseExact(dateString, Format);
                }

                throw new JsonException($"Unexpected token parsing DateOnly. Expected StartObject or String, got {reader.TokenType}.");
            }
            catch (JsonException)
            {
                throw; 
            }
            catch (Exception ex)
            {
                throw new JsonException("Error deserializing DateOnly value.", ex);
            }
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }
}
