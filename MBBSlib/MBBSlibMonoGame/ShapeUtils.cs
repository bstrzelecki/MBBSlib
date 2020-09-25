using Microsoft.Xna.Framework;
using System;

namespace MBBSlib.MonoGame
{
    public class ShapeUtils
    {
        public static void DrawArea(Vector2 start, Vector2 end, Action<Vector2> DrawObject)
        {
            FixOrigins(ref start, ref end);
            for (int x = (int)start.X; x < end.X; x++)
            {
                for (int y = (int)start.Y; y < end.Y; y++)
                {
                    DrawObject(new Vector2(x, y));
                }
            }
        }
        public static void DrawLine(Vector2 start, Vector2 end, Action<Vector2> DrawObject)
        {
            Vector2 origin = start;
            FixOrigins(ref start, ref end);
            if (end.Y - start.Y > end.X - start.X)
            {
                for (int y = (int)start.Y; y <= end.Y; y++)
                {
                    DrawObject(new Vector2(origin.X, y));
                }
            }
            else
            {
                for (int x = (int)start.X; x <= end.X; x++)
                {
                    DrawObject(new Vector2(x, origin.Y));
                }
            }
        }

        private static void FixOrigins(ref Vector2 start, ref Vector2 end)
        {
            if (end.X < start.X)
            {
                int b = (int)end.X;
                end.X = start.X;
                start.X = b;
            }
            if (end.Y < start.Y)
            {
                int b = (int)end.Y;
                end.Y = start.Y;
                start.Y = b;
            }
        }
    }
}
