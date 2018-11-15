using Xunit;

namespace ThemeEditor.Colors.UnitTests
{
    public class RGBTests
    {
        [Fact]
        public void RGB_ToHSV()
        {
            var rgb = new RGB(67.0, 93.0, 125.0);
            var actual = rgb.ToHSV();
            Assert.Equal(213.10, actual.H, 2);
            Assert.Equal(46.40, actual.S, 2);
            Assert.Equal(49.02, actual.V, 2);
        }
    }
}
