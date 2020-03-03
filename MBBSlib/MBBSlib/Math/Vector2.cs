using System;

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
        //TODO
        public float Normalized { get { return (float)(System.Math.Sqrt((x * x) + (y * y))); } }
        public float x;
        public float y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public Vector2(byte[] array)
        {
            this.x = BitConverter.ToInt32(array[0..4], 0);
            this.y = BitConverter.ToInt32(array[4..8], 0);
        }
        public byte[] Bytes
        {
            get
            {
                byte[] b = new byte[8];
                Array.Copy(BitConverter.GetBytes(x), 0, b, 0, 4);
                Array.Copy(BitConverter.GetBytes(y), 0, b, 4, 4);
                return b;
            }
        }
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }
        public static Vector2 operator *(Vector2 a, float b)
        {
            return new Vector2(a.x * b, a.y * b);
        }
        public static Vector2 operator /(Vector2 a, float b)
        {
            return new Vector2(a.x / b, a.y / b);
        }
    }
}
