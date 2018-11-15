using Xunit;

namespace ThemeEditor.Colors.UnitTests
{
    public class HSVTests
    {
        [Fact]
        public void HSV_ToRGB()
        {
            var hsv = new HSV(213.0, 46.0, 49.0);
            var actual = hsv.ToRGB();
            Assert.Equal(67.0, actual.R);
            Assert.Equal(93.0, actual.G);
            Assert.Equal(125.0, actual.B);
        }

        [Fact]
        public void HSV_ToCMYK()
        {
            var hsv = new HSV(213.1, 46.0, 49.0);
            var actual = hsv.ToCMYK();
            Assert.Equal(46.4, actual.C, 2);
            Assert.Equal(25.6, actual.M, 2);
            Assert.Equal(0.0, actual.Y, 2);
            Assert.Equal(50.98, actual.K, 2);
        }
    }
}
