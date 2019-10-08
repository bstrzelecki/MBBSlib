using System;
using System.Collections.Generic;
using System.Text;

namespace MBBSlib.Serialization
{
    public interface ISerializable
    {
        void Load(NBTCompund compund);
        void Save(NBTCompund compund);
    }
}
