#nullable disable
using System;
using Avalonia;
using Newtonsoft.Json;

namespace ThemeEditor.ViewModels.Serializer.Converters
{
    public class ThicknessViewModelConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ThicknessViewModel);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch (value as ThicknessViewModel)
            {
                case ThicknessViewModel thickness:
                    writer.WriteValue(thickness.ToTextString());
                    break;
                default:
                    throw new NotSupportedException($"The {value.GetType()} type is not supported.");
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(ThicknessViewModel))
            {
                return Thickness.Parse((string)reader.Value).FromThickness();
            }
            throw new ArgumentException("objectType");
        }
    }
}
