using System.Text.Json.Serialization;
using System.Text.Json;

namespace HospitalCrud.JSONConverters
{
    /// <summary>
    /// Converts a JSON string token into a DateTime object
    /// </summary>
    public class DateTimeConverter : JsonConverter<DateTime?>
    {
        /// <summary>
        /// Tries to read and parse the next JSON token as a DateTime object
        /// </summary>
        /// 
        /// <param name="reader">The injected JSON reader</param>
        /// <param name="typeToConvert">The type of object to convert</param>
        /// <param name="options">Options for the serializer</param>
        /// 
        /// <returns>
        /// The parsed DateTime object if parsing succeeds, DateTime.MinValue if parsing fails, null if token is not a string
        /// </returns>
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string? dateString = reader.GetString();

                if (DateTime.TryParse(dateString, out DateTime dateTime))
                    return dateTime;
                else
					// Needed to distinguish null from ill-formed date string, 
                    // usually contains "01/01/0001 00:00:00"
					return DateTime.MinValue; 
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteStringValue(value.Value);
            else
                writer.WriteNullValue();
        }
    }
}
