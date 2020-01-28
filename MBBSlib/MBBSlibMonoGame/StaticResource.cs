namespace MBBSlib.MonoGame
{
    public class StaticResource<T>
    {
        public string Key { get; set; }
        public T Resource
        {
            get
            {
                if (_resource == null)
                {
                    _resource = (T)StaticResources.GetResource(Key);
                    if (_resource == null && GameMain.textures[Key] is T t)
                    {
                        _resource = t;
                    }
                    if (_resource == null && GameMain.fonts[Key] is T y)
                    {
                        _resource = y;
                    }
                }
                if (_resource != null)
                {
                    return _resource;
                }
                return default;
            }
        }
        private T _resource;
        public StaticResource(string key)
        {
            Key = key;
        }
        public static implicit operator T(StaticResource<T> st)
        {
            return st.Resource;
        }
    }
}
