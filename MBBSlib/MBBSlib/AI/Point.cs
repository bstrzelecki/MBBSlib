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
    }
}
