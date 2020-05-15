using MBBSlib.Math;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MBBSlib.Utility
{
    public class Grid<T> : IEnumerable<T>, IGrid<T>, ISerializable
    {
        private readonly T[,] array;
        private readonly int width;
        private readonly int height;
        public Grid(int width, int height)
        {
            array = new T[width, height];
            this.width = width;
            this.height = height;
        }
        public T this[int x, int y]
        {
            get => GetValue(x, y);
            set => SetValue(x, y, value);
        }
        public void SetValue(int x, int y, T value)
        {
            if (x < 0 || y < 0 || x >= width || y >= height) throw new IndexOutOfRangeException();
            array[x, y] = value; 
        }
        public T GetValue(int x, int y)
        {
            if (x < 0 || y < 0 || x >= width || y >= height) throw new IndexOutOfRangeException();
            return array[x, y];
        }
        public bool Contains(T obj)
        {
            foreach(T t in array)
            {
                if (obj.Equals(t)) return true;
            }
            return false;
        }
        public T[] GetRow(int y)
        {
            var ar = new T[width];
            for(int i = 0; i < y; i++)
            {
                ar[i] = array[i, y];
            }
            return ar;
        }
        public T[] GetColumn(int x)
        {
            var ar = new T[height];
            for (int i = 0; i < x; i++)
            {
                ar[i] = array[x,i];
            }
            return ar;
        }
        public Vector2 IndexOf(T obj)
        {
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    if (obj.Equals(array[i, j]))
                    {
                        return new Vector2(i, j);
                    }
                }
            }
            return new Vector2(-1, -1);
        }
        public void IndexOf(T obj, out int x, out int y)
        {
            var v = IndexOf(obj);
            x = (int)v.x;
            y = (int)v.y;
        }
        public IEnumerator<T> GetEnumerator() => new GridEnumerator<T>(array);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public Grid(SerializationInfo info, StreamingContext context)
        {
            //TOOD
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    info.AddValue($"{i}:{j}", array[i, j], typeof(T));
                }
            }
        }
    }
    public class GridEnumerator<T> : IEnumerator<T>
    {
        public T Current { get; set; }

        object IEnumerator.Current => Current;
        private T[,] array;
        int x, y;
        internal GridEnumerator(T[,] array)
        {
            this.array = array;
            Current = array[0, 0];
        }
        public void Dispose()
        {
            array = null;
            x = 0;
            y = 0;
        }

        public bool MoveNext()
        {
            x++;
            if(x > array.GetLength(0))
            {
                x = 0;
                y++;
                if (y > array.GetLength(1))
                    return false;
            }
            Current = array[x, y];
            return true;
        }

        public void Reset()
        {
            x = 0;
            y = 0;
            Current = array[x, y];
        }
    }
}
