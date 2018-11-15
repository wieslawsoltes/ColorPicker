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
            Assert.Equal(213.103448275862, actual.H);
            Assert.Equal(46.4, actual.S);
            Assert.Equal(49.0196078431373, actual.V);
        }
    }
}
