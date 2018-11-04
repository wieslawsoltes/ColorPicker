using ThemeEditor.Colors;

namespace ThemeEditor.ColorMatch
{
    public interface IAlgorithm
    {
        Blend Match(HSV hsv);
    }
}
