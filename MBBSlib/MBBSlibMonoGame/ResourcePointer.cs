using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MBBSlib.MonoGame
{
    public class ResourcePointer <T>
    {
        public static T operator &(ResourcePointer<T> pointer, string key)
        {
            try
            {
                if (StaticResources.Resources.ContainsKey(key))
                {
                    return (T)StaticResources.Resources[key];
                }
                else
                {
                    return default;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return default;
            }
        }
    }
}
