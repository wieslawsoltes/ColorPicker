using Xunit;

namespace ThemeEditor.Colors.UnitTests
{
    public class CMYKTests
    {
        [Fact]
        public void CMYK_ToRGB()
        {
            var cmyk = new CMYK(46.4, 25.6, 0, 50.98);
            var actual = rgb.ToRGB();
            Assert.Equal(67, actual.R);
            Assert.Equal(93, actual.G);
            Assert.Equal(125, actual.B);
        }

        [Fact]
        public void CMYK_ToHSV()
        {
            var cmyk = new CMYK(46.4, 25.6, 0, 50.98);
            var actual = rgb.ToHSV();
            Assert.Equal(213.1, actual.H, 2);
            Assert.Equal(46.4, actual.S, 2);
            Assert.Equal(49.02, actual.V, 2);
        }
    }
}
