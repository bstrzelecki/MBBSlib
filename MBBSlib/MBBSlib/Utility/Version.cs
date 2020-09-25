using System;

namespace MBBSlib.Utility
{
    public struct Version
    {
        public int Major => _major;
        public int Minor => _minor;
        public int Patch => _patch;

        private int _major;
        private int _minor;
        private int _patch;

        public Version(string version)
        {
            string[] v = version.Split('.');
            if (v.Length != 3)
                throw new ArgumentException("Wrong string formatting.");

            _major = int.Parse(v[0]);
            _minor = int.Parse(v[1]);
            _patch = int.Parse(v[2]);
        }
        public Version(int major, int minor, int patch)
        {
            _major = major;
            _minor = minor;
            _patch = patch;
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
            return (a._major == b._major && a._minor == b._minor & a._patch == b._patch);
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
    }
}
