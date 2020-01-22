namespace MBBSlib.Serialization
{
    public static class IOExtensions
    {
        public static void Register(this ISerializable serializable, string id)
        {
            Serializer.Register(id, serializable);
        }
    }
}
