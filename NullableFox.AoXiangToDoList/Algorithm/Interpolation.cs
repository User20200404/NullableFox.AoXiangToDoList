using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Algorithm
{
    internal class Interpolation
    {
        public static (byte, byte, byte, byte) ArgbBetween((byte, byte, byte, byte) start, (byte, byte, byte, byte) end, float progress)
        {
            return (
                (byte)(start.Item1 + progress * (end.Item1 - start.Item1)),
                (byte)(start.Item2 + progress * (end.Item2 - start.Item2)),
                (byte)(start.Item3 + progress * (end.Item3 - start.Item3)),
                (byte)(start.Item4 + progress * (end.Item4 - start.Item4))
                );
        }

        public static Windows.UI.Color WinUIColorBetween(Windows.UI.Color start, Windows.UI.Color end, float progress)
        {
            var bStart = (start.A, start.R, start.G, start.B);
            var bEnd = (end.A, end.R, end.G, end.B);
            var argb = ArgbBetween(bStart, bEnd, progress);
            return Windows.UI.Color.FromArgb(argb.Item1, argb.Item2, argb.Item3, argb.Item4);
        }
    }
}
