using MBBSlib.Math;

namespace MBBSlib.Utility
{
    interface IGrid<T>
    {
        T this[int x, int y] { get; set; }
        void SetValue(int x, int y, T value);
        T GetValue(int x, int y);
        bool Contains(T obj);
        Vector2 IndexOf(T obj);
        void IndexOf(T obj, out int x, out int y);
    }
}
