using MBBSlib.Math;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MBBSlib.Utility
{
    /// <summary>
    /// Grid data collection
    /// </summary>
    /// <typeparam name="T">Type of stored data</typeparam>
    public class Grid<T> : IEnumerable<T>, IGrid<T>, ISerializable
    {
        private readonly T[,] _array;
        private readonly int _width;
        private readonly int _height;
        /// <summary>
        /// Creates grid with given dimensions
        /// </summary>
        /// <param name="width">Number of rows</param>
        /// <param name="height">Number of columns</param>
        public Grid(int width, int height)
        {
            _array = new T[width, height];
            this._width = width;
            this._height = height;
        }
        /// <summary>
        /// Access to data at given index
        /// </summary>
        /// <param name="x">Data at row</param>
        /// <param name="y">Data at column</param>
        /// <returns>Data at given indexes</returns>
        public T this[int x, int y]
        {
            get => GetValue(x, y);
            set => SetValue(x, y, value);
        }
        /// <summary>
        /// Sets the value at given index
        /// </summary>
        /// <param name="x">Data at row</param>
        /// <param name="y">Data at column</param>
        /// <param name="value">Value that will be set</param>
        public void SetValue(int x, int y, T value)
        {
            if(x < 0 || y < 0 || x >= _width || y >= _height) throw new IndexOutOfRangeException();
            _array[x, y] = value;
        }
        public T GetValue(int x, int y)
        {
            if(x < 0 || y < 0 || x >= _width || y >= _height) throw new IndexOutOfRangeException();
            return _array[x, y];
        }
        public bool Contains(T obj)
        {
            foreach(T t in _array)
            {
                if(obj.Equals(t)) return true;
            }
            return false;
        }
        public T[] GetRow(int y)
        {
            var ar = new T[_width];
            for(int i = 0; i < y; i++)
            {
                ar[i] = _array[i, y];
            }
            return ar;
        }
        public T[] GetColumn(int x)
        {
            var ar = new T[_height];
            for(int i = 0; i < x; i++)
            {
                ar[i] = _array[x, i];
            }
            return ar;
        }
        public Vector2 IndexOf(T obj)
        {
            for(int i = 0; i < _width; i++)
            {
                for(int j = 0; j < _height; j++)
                {
                    if(obj.Equals(_array[i, j]))
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
        public IEnumerator<T> GetEnumerator() => new GridEnumerator<T>(_array);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public Grid(SerializationInfo info, StreamingContext context)
        {
            //TOOD
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            for(int i = 0; i < _width; i++)
            {
                for(int j = 0; j < _height; j++)
                {
                    info.AddValue($"{i}:{j}", _array[i, j], typeof(T));
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
                if(y > array.GetLength(1))
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
