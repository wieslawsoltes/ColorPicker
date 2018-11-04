using ThemeEditor.ColorMatch.Algorithms;
using ThemeEditor.Colors;
using Xunit;

namespace ThemeEditor.ColorMatch.UnitTests
{
    public class AlgorithmsTests
    {
        [Fact]
        public void Classic_Match()
        {
            var hsv = new HSV(213, 46, 49);
            var algorithm = new Classic();
            var actual = algorithm.Match(hsv);
            var expected = new Blend()
            {
                Colors = new[]
                {
                    new HSV(213, 46, 49),
                    new HSV(213, 46, 79),
                    new HSV(53, 46, 49),
                    new HSV(43, 46, 79),
                    new HSV(0, 0, 51),
                    new HSV(0, 0, 49)
                }
            };
            Assert.Equal(expected, actual, new BlendEqualityComparer());
        }

        [Fact]
        public void ColorExplorer_Match()
        {
            var hsv = new HSV(213, 46, 49);
            var algorithm = new ColorExplorer();
            var actual = algorithm.Match(hsv);
            var expected = new Blend()
            {
                Colors = new[]
                {
                    new HSV(213, 46, 49),
                    new HSV(213, 14, 64),
                    new HSV(153, 55, 24),
                    new HSV(153, 46, 49),
                    new HSV(153, 0, 99),
                    new HSV(153, 0, 49)
                }
            };
            Assert.Equal(expected, actual, new BlendEqualityComparer());
        }

        [Fact]
        public void SingleHue_Match()
        {
            var hsv = new HSV(213, 46, 49);
            var algorithm = new SingleHue();
            var actual = algorithm.Match(hsv);
            var expected = new Blend()
            {
                Colors = new[]
                {
                    new HSV(213, 46, 49),
                    new HSV(213, 46, 69),
                    new HSV(213, 46, 89),
                    new HSV(213, 66, 49),
                    new HSV(213, 86, 49),
                    new HSV(213, 86, 89)
                }
            };
            Assert.Equal(expected, actual, new BlendEqualityComparer());
        }

        [Fact]
        public void Complementary_Match()
        {
            var hsv = new HSV(213, 46, 49);
            var algorithm = new Complementary();
            var actual = algorithm.Match(hsv);
            var expected = new Blend()
            {
                Colors = new[]
                {
                    new HSV(213, 46, 49),
                    new HSV(213, 92, 73.5),
                    new HSV(47, 46, 49),
                    new HSV(47, 92, 73.5),
                    new HSV(0, 0, 49),
                    new HSV(0, 0, 51)
                }
            };
            Assert.Equal(expected, actual, new BlendEqualityComparer());
        }

        [Fact]
        public void SplitComplementary_Match()
        {
            var hsv = new HSV(213, 46, 49);
            var algorithm = new SplitComplementary();
            var actual = algorithm.Match(hsv);
            var expected = new Blend()
            {
                Colors = new[]
                {
                    new HSV(213, 46, 49),
                    new HSV(27, 46, 49),
                    new HSV(67, 46, 49),
                    new HSV(67, 0, 46),
                    new HSV(67, 0, 49),
                    new HSV(67, 0, 51)
                }
            };
            Assert.Equal(expected, actual, new BlendEqualityComparer());
        }

        [Fact]
        public void Analogue_Match()
        {
            var hsv = new HSV(213, 46, 49);
            var algorithm = new Analogue();
            var actual = algorithm.Match(hsv);
            var expected = new Blend()
            {
                Colors = new[]
                {
                    new HSV(213, 46, 49),
                    new HSV(253, 46, 49),
                    new HSV(293, 46, 49),
                    new HSV(0, 0, 51),
                    new HSV(0, 0, 64),
                    new HSV(0, 0, 38)
                }
            };
            Assert.Equal(expected, actual, new BlendEqualityComparer());
        }

        [Fact]
        public void Triadic_Match()
        {
            var hsv = new HSV(213, 46, 49);
            var algorithm = new Triadic();
            var actual = algorithm.Match(hsv);
            var expected = new Blend()
            {
                Colors = new[]
                {
                    new HSV(213, 46, 49),
                    new HSV(213, 46, 51),
                    new HSV(7, 46, 49),
                    new HSV(7, 46, 51),
                    new HSV(87, 46, 49),
                    new HSV(87, 46, 51)
                }
            };
            Assert.Equal(expected, actual, new BlendEqualityComparer());
        }

        [Fact]
        public void Square_Match()
        {
            var hsv = new HSV(213, 46, 49);
            var algorithm = new Square();
            var actual = algorithm.Match(hsv);
            var expected = new Blend()
            {
                Colors = new[]
                {
                    new HSV(213, 46, 49),
                    new HSV(333, 46, 49),
                    new HSV(47, 46, 49),
                    new HSV(107, 46, 49),
                    new HSV(107, 0, 49),
                    new HSV(107, 0, 51)
                }
            };
            Assert.Equal(expected, actual, new BlendEqualityComparer());
        }
    }
}
