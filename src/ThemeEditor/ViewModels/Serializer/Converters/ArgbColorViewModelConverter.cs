#nullable disable
using System;
using Avalonia.Media;
using Newtonsoft.Json;

namespace ThemeEditor.ViewModels.Serializer.Converters
{
    public class ArgbColorViewModelConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ArgbColorViewModel);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch (value)
            {
                case ArgbColorViewModel color:
                    writer.WriteValue(color.ToHexString());
                    break;
                default:
                    throw new NotSupportedException($"The {value.GetType()} type is not supported.");
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(ArgbColorViewModel))
            {
                return Color.Parse((string)reader.Value).ArgbFromColor();
            }
            throw new ArgumentException("objectType");
        }
    }
}
