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
            Assert.Equal(213.0, actual.H, 2);
            Assert.Equal(46.40, actual.S, 2);
            Assert.Equal(49.02, actual.V, 2);
        }

        [Fact]
        public void RGB_ToCMYK()
        {
            var rgb = new RGB(67.0, 93.0, 125.0);
            var actual = rgb.ToCMYK();
            Assert.Equal(46.4, actual.C, 2);
            Assert.Equal(25.6, actual.M, 2);
            Assert.Equal(0.0, actual.Y, 2);
            Assert.Equal(50.98, actual.K, 2);
        }
    }
}
