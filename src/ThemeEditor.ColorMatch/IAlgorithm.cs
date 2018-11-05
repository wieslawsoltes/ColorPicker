using ThemeEditor.Colors;

namespace ThemeEditor.ColorMatch
{
    public interface IAlgorithm
    {
        string Name { get; }
        Blend Match(HSV hsv);
    }
}
