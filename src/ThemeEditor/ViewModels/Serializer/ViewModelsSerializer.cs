#nullable disable
using Newtonsoft.Json;
using ThemeEditor.ViewModels.Serializer.Converters;

namespace ThemeEditor.ViewModels.Serializer
{
    public class ViewModelsSerializer
    {
        private readonly JsonSerializerSettings _settings;

        public ViewModelsSerializer()
        {
            _settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                Converters =
                {
                    new ColorConverter(),
                    new RgbColorViewModelConverter(),
                    new ArgbColorViewModelConverter(),
                    new ThicknessViewModelConverter()
                }
            };
        }

        public string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value, _settings);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }
    }
}
