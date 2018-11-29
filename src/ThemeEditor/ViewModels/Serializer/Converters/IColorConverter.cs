using System;
using Avalonia.Media;
using Newtonsoft.Json;

namespace ThemeEditor.ViewModels.Serializer.Converters
{
    public class IColorConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IColor);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch (value)
            {
                case RgbColorViewModel color:
                    writer.WriteValue(color.ToHexString());
                    break;
                case ArgbColorViewModel color:
                    writer.WriteValue(color.ToHexString());
                    break;
                default:
                    throw new NotSupportedException($"The {value.GetType()} type is not supported.");
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(IColor))
            {
                return Color.Parse((string)reader.Value).ArgbFromColor();
            }
            throw new ArgumentException("objectType");
        }
    }
}
