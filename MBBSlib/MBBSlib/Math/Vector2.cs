using System;

namespace MBBSlib.Math
{
    public struct Vector2
    {
        public static Vector2 Down => new Vector2(0, -1);
        public static Vector2 Up => new Vector2(0, 1);
        public static Vector2 One => new Vector2(1, 1);
        public static Vector2 Left => new Vector2(-1, 0);
        public static Vector2 Right => new Vector2(1, 0);
        public static Vector2 Zero => new Vector2(0, 0);

        public float Magnitude => (float)(System.Math.Sqrt((x * x) + (y * y)));
        //TODO
        public Vector2 Normalized => (Magnitude != 0) ? new Vector2(x / Magnitude, y / Magnitude) : One;
        public float x;
        public float y;
        /// <summary>
        /// Creates vector with magnitude of 1 and given angle
        /// </summary>
        /// <param name="rotation">Direction of vector in radians</param>
        public Vector2(float rotation)
        {
            x = (float)System.Math.Cos(rotation);
            y = (float)System.Math.Sin(rotation);
        }
        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public Vector2(byte[] array)
        {
            x = BitConverter.ToInt32(array[0..4], 0);
            y = BitConverter.ToInt32(array[4..8], 0);
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
        public override bool Equals(object obj) => obj is Vector2 b ? x == b.x && y == b.y : false;
        public override int GetHashCode() => HashCode.Combine(this);

        public override string ToString() => $"{x} {y}";
        public static bool operator ==(Vector2 a, Vector2 b) => (a.x == b.x && a.y == b.y);
        public static bool operator !=(Vector2 a, Vector2 b) => !(a.x == b.x && a.y == b.y);
        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.x + b.x, a.y + b.y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.x - b.x, a.y - b.y);
        public static Vector2 operator *(Vector2 a, float b) => new Vector2(a.x * b, a.y * b);
        public static Vector2 operator *(int b, Vector2 a) => new Vector2(a.x * b, a.y * b);
        public static Vector2 operator /(Vector2 a, float b) => new Vector2(a.x / b, a.y / b);
    }
}
