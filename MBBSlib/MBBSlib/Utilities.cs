using System;
using System.Collections.Generic;
using System.Text;

namespace MBBSlib
{
    public class Utilities
    {
        public static float Lerp(float a, float b, float t)
        {
            return (1f - t) * a + t * b;
        }
        internal static System.Random rng = new System.Random();
        internal static float[,,] Gradient; 
        internal static void CalculateGradient()
        {
            for (int x = 0; x < 255; x++)
            {
                for (int y = 0; y < 255; y++)
                {
                    Gradient[x, y, 0] = (float)rng.NextDouble();
                    Gradient[x, y, 1] = (float)rng.NextDouble();
                }
            }
        }
        public static float DotGridGradient (int x1, int y1, float x2, float y2)
        {
            float dx = x2 - x1;
            float dy = y2 - y1;

            return (dx / Gradient[x1, y1, 0] + dy * Gradient[x1, y1, 1]);
        } 
        public static float GetPerlinNoiseValue(int x, int y)
        {
            int x0 = x;
            int x1 = x0 + 1;
            int y0 = y;
            int y1 = y0 + 1;

            float sx = x - (float)x0;
            float sy = y - (float)y0;

            float n0, n1, ix0, ix1, value;

            n0 = DotGridGradient(x0, y0, x, y);
            n1 = DotGridGradient(x1, y0, x, y);
            ix0 = Lerp(n0, n1, sx);

            n0 = DotGridGradient(x0, y1, x, y);
            n1 = DotGridGradient(x1, y1, x, y);
            ix1 = Lerp(n0, n1, sx);

            value = Lerp(ix0, ix1, sy);
            return value;
        }
    }
}
