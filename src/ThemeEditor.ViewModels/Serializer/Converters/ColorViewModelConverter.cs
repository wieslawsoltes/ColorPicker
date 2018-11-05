using System;
using Avalonia.Media;
using Newtonsoft.Json;

namespace ThemeEditor.ViewModels.Serializer.Converters
{
    public class ColorViewModelConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ColorViewModel);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch (value as ColorViewModel)
            {
                case ColorViewModel color:
                    writer.WriteValue(color.ToHexString());
                    break;
                default:
                    throw new NotSupportedException($"The {value.GetType()} type is not supported.");
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(ColorViewModel))
            {
                return Color.Parse((string)reader.Value).FromColor();
            }
            throw new ArgumentException("objectType");
        }
    }
}
