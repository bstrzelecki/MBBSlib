using System;

namespace MBBSlib.Utility
{
    /// <summary>
    /// Semantic versioning struct
    /// </summary>
    public struct Version
    {
        /// <summary>
        /// MAJOR version when you make incompatible API changes
        /// </summary>
        public int Major => _major;
        /// <summary>
        /// MINOR version when you add functionality in a backwards compatible manner
        /// </summary>
        public int Minor => _minor;
        /// <summary>
        /// PATCH version when you make backwards compatible bug fixes
        /// </summary>
        public int Patch => _patch;

        private int _major;
        private int _minor;
        private int _patch;

        /// <summary>
        /// Parses string to version struct MAJOR.MINOR.PATCH
        /// </summary>
        /// <param name="version">MAJOR.MINOR.PATCH</param>
        public Version(string version)
        {
            string[] v = version.Split('.');
            if (v.Length != 3)
                throw new ArgumentException("Wrong string formatting.");

            _major = int.Parse(v[0]);
            _minor = int.Parse(v[1]);
            _patch = int.Parse(v[2]);
        }
        /// <summary>
        /// Parses versions
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <param name="patch"></param>
        public Version(int major, int minor, int patch)
        {
            _major = major;
            _minor = minor;
            _patch = patch;
        }
        public Version(byte[] array)
        {
            _major = BitConverter.ToInt32(array, 0);
            _minor = BitConverter.ToInt32(array, sizeof(int));
            _patch = BitConverter.ToInt32(array, 2 * sizeof(int));
        }
        public byte[] ToByteArray()
        {
            var arr = BitConverter.GetBytes(Major);
            BitConverter.GetBytes(Minor).CopyTo(arr, sizeof(int));
            BitConverter.GetBytes(Patch).CopyTo(arr, 2 * sizeof(int));
            return arr;
        }
        public static bool operator >(Version a, Version b)
        {
            if (a._major > b._major)
            {
                return true;
            }
            if (a._major == b._major)
            {
                if (a._minor > b._minor)
                {
                    return true;
                }
                if (a._minor == b._minor && a._patch > b._patch)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool operator <(Version a, Version b)
        {
            if (a._major < b._major)
            {
                return true;
            }
            if (a._major == b._major)
            {
                if (a._minor < b._minor)
                {
                    return true;
                }
                if (a._minor == b._minor && a._patch < b._patch)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool operator >=(Version a, Version b)
        {
            if (a._major >= b._major)
            {
                return true;
            }
            if (a._major == b._major)
            {
                if (a._minor >= b._minor)
                {
                    return true;
                }
                if (a._minor == b._minor && a._patch >= b._patch)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool operator <=(Version a, Version b)
        {
            if (a._major <= b._major)
            {
                return true;
            }
            if (a._major == b._major)
            {
                if (a._minor <= b._minor)
                {
                    return true;
                }
                if (a._minor == b._minor && a._patch <= b._patch)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool operator ==(Version a, Version b)
        {
            return (a._major == b._major && a._minor == b._minor && a._patch == b._patch);
        }
        public static bool operator !=(Version a, Version b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return this == (Version)obj;
        }

        public override string ToString()
        {
            return $"{_major}.{_minor}.{_patch}";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_major, _minor, _patch);
        }
    }
}
