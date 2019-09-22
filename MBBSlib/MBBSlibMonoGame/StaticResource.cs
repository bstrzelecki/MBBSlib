using System;
using System.Collections.Generic;
using System.Text;

namespace MBBSlib.MonoGame
{
    class StaticResource<T>
    {
        public string Key { get; set; }
        public T Resource
        {
            get
            {
                if (_resource == null)
                {
                    _resource = (T)StaticResources.GetResource(Key);
                }
                if(_resource != null)
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
