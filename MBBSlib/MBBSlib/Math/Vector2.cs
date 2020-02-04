using System;
using System.Collections.Generic;
using System.Text;

namespace MBBSlib.Math
{
    public struct Vector2
    {
        public static Vector2 Down { get { return new Vector2(0, -1); } }
        public static Vector2 Up { get { return new Vector2(0, 1); } }
        public static Vector2 One { get { return new Vector2(1, 1); } }
        public static Vector2 Left { get { return new Vector2(-1, 0); } }
        public static Vector2 Right { get { return new Vector2(1, 0); } }
        public static Vector2 Zero { get { return new Vector2(0, 0); } }

        public float Magnitude { get { return (float)(System.Math.Sqrt((x * x) + (y * y))); } }

        public float x;
        public float y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
