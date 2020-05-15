namespace MBBSlib.AI
{
    public struct Point
    {
        /// <summary>
        /// Integer coordinate
        /// </summary>
        public int X, Y;
        /// <summary>
        /// Defoult constructor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
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
        public override string ToString() => $"({X}, {Y})";

        public override bool Equals(object obj) => obj is Point point &&
                   X == point.X &&
                   Y == point.Y;

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}
