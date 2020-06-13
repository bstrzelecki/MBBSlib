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
        /// <summary>
        /// Gets the value at given index
        /// </summary>
        /// <param name="x">Data at row</param>
        /// <param name="y">Data at column</param>
        /// <returns>Value at given index</returns>
        public T GetValue(int x, int y)
        {
            if(x < 0 || y < 0 || x >= _width || y >= _height) throw new IndexOutOfRangeException();
            return _array[x, y];
        }
        /// <summary>
        /// Checks if vaule exist in the grid
        /// </summary>
        /// <param name="obj">Value reference that will be checked</param>
        /// <returns>True if value exist in grid, othervise false</returns>
        public bool Contains(T obj)
        {
            foreach(T t in _array)
            {
                if(obj.Equals(t)) return true;
            }
            return false;
        }
        /// <summary>
        /// Gets the values at given row
        /// </summary>
        /// <param name="y">Index of row</param>
        /// <returns><see cref="Array"/> of values at given row</returns>
        public T[] GetRow(int y)
        {
            var ar = new T[_width];
            for(int i = 0; i < y; i++)
            {
                ar[i] = _array[i, y];
            }
            return ar;
        }
        /// <summary>
        /// Gets the values at given column
        /// </summary>
        /// <param name="x">Index of column</param>
        /// <returns><see cref="Array"/> of values at given column</returns>
        public T[] GetColumn(int x)
        {
            var ar = new T[_height];
            for(int i = 0; i < x; i++)
            {
                ar[i] = _array[x, i];
            }
            return ar;
        }
        /// <summary>
        /// Gets the first occurance of given value
        /// </summary>
        /// <param name="obj">Value to be found</param>
        /// <returns>Index of first occurence of value</returns>
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
        /// <summary>
        /// Gets the first occurance of given value
        /// </summary>
        /// <param name="obj">Value to be found</param>
        /// <param name="x">Row index of first occurence of value</param>
        /// <param name="y">Column index of first occurence of value</param>
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
    /// <summary>
    /// Supports iteration over <see cref="Grid{T}"/>
    /// </summary>
    /// <typeparam name="T">Value type to iterate</typeparam>
    public class GridEnumerator<T> : IEnumerator<T>
    {
        /// <summary>
        /// Currently selected element
        /// </summary>
        public T Current { get; set; }

        object IEnumerator.Current => Current;
        private T[,] _array;
        int _x, _y;
        internal GridEnumerator(T[,] array)
        {
            _array = array;
            Current = array[0, 0];
        }
        public void Dispose()
        {
            _array = null;
            _x = 0;
            _y = 0;
        }

        public bool MoveNext()
        {
            _x++;
            if(_x > _array.GetLength(0))
            {
                _x = 0;
                _y++;
                if(_y > _array.GetLength(1))
                    return false;
            }
            Current = _array[_x, _y];
            return true;
        }

        public void Reset()
        {
            _x = 0;
            _y = 0;
            Current = _array[_x, _y];
        }
    }
}
