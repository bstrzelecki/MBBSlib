﻿namespace MBBSlib.Math
{
    public struct Vector3
    {
        public static Vector3 Down => new Vector3(0, -1, 0);
        public static Vector3 Up => new Vector3(0, 1, 0);
        public static Vector3 One => new Vector3(1, 1, 0);
        public static Vector3 Left => new Vector3(-1, 0, 0);
        public static Vector3 Right => new Vector3(1, 0, 0);
        public static Vector3 Zero => new Vector3(0, 0, 0);

        public float Magnitude => (float)(System.Math.Sqrt((x * x) + (y * y)));
        //TODO
        public float Normalized => (float)(System.Math.Sqrt((x * x) + (y * y)));
        public float x;
        public float y;
        public float z;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        public static Vector3 operator *(Vector3 a, float b) => new Vector3(a.x * b, a.y * b, a.z * b);
        public static Vector3 operator /(Vector3 a, float b) => new Vector3(a.x / b, a.y / b, a.z / b);
    }
}
