namespace MBBSlib.MonoGame.UI
{
    public class Style<T> where T : Panel, new()
    {
        public T Prefab { get; }

        public Style()
        {
            Prefab = new T();
        }

        public Style(T prefab)
        {
            Prefab = prefab;
        }

        public static implicit operator T(Style<T> style) => style.Prefab;
    }
}