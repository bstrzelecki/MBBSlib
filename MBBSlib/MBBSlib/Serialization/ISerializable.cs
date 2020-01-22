namespace MBBSlib.Serialization
{
    public interface ISerializable
    {
        void Load(NBTCompund compund);
        void Save(NBTCompund compund);
    }
}
