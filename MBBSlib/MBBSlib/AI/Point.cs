namespace MBBSlib.AI
{
    /// <summary>
    /// Classes that storeas x and y integer coordinates
    /// </summary>
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
        /// <summary>
        /// Checks if points are equal
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Wiil retun true if both x and y coordinates are equal</returns>
        public static bool operator ==(Point a, Point b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Point a, Point b) => a.X != b.X || a.Y != b.Y;
        /// <summary>
        /// Parses both coordinates to string
        /// </summary>
        /// <returns> $"({X}, {Y})"</returns>
        public override string ToString() => $"({X}, {Y})";
        /// <summary>
        /// Checks if points are equal
        /// </summary>
        /// <param name="obj">Should be instance of <see cref="Point"/> otherwise will return false</param>
        /// <returns>Wiil retun true if both x and y coordinates are equal</returns>
        public override bool Equals(object obj) => obj is Point point &&
                   X == point.X &&
                   Y == point.Y;
        /// <summary>
        /// Default calculate hashcode method
        /// </summary>
        /// <returns>Hashcode</returns>
        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}
