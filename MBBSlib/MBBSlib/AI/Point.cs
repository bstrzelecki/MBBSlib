using System;
using System.Collections.Generic;
using System.Text;

namespace MBBSlib.AI
{
    public struct Point
    {
        public int X, Y;
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public static bool operator ==(Point a, Point b)
        {
            if (a.X == b.X && a.Y == b.Y)
                return true;
            return false;
        }
        public static bool operator !=(Point a, Point b)
        {
            if (a.X != b.X || a.Y != b.Y)
                return true;
            return false;
        }
        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public override bool Equals(object obj)
        {
            return obj is Point point &&
                   X == point.X &&
                   Y == point.Y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}
