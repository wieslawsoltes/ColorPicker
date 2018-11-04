using System.Collections.Generic;

namespace ThemeEditor.ColorMatch.UnitTests
{
    internal class BlendEqualityComparer : IEqualityComparer<Blend>
    {
        public bool Equals(Blend x, Blend y)
        {
            return x.Colors[0].H == y.Colors[0].H
                && x.Colors[0].S == y.Colors[0].S
                && x.Colors[0].V == y.Colors[0].V
                && x.Colors[1].H == y.Colors[1].H
                && x.Colors[1].S == y.Colors[1].S
                && x.Colors[1].V == y.Colors[1].V
                && x.Colors[2].H == y.Colors[2].H
                && x.Colors[2].S == y.Colors[2].S
                && x.Colors[2].V == y.Colors[2].V
                && x.Colors[3].H == y.Colors[3].H
                && x.Colors[3].S == y.Colors[3].S
                && x.Colors[3].V == y.Colors[3].V
                && x.Colors[4].H == y.Colors[4].H
                && x.Colors[4].S == y.Colors[4].S
                && x.Colors[4].V == y.Colors[4].V
                && x.Colors[5].H == y.Colors[5].H
                && x.Colors[5].S == y.Colors[5].S
                && x.Colors[5].V == y.Colors[5].V;
        }

        public int GetHashCode(Blend obj)
        {
            return 0;
        }
    }
}
