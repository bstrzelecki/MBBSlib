namespace MBBSlib.Networking.Shared
{
    public interface ICommandInterpreter
    {
        void IsEnabled();
        void ExecuteCommand(int sender, byte[] data);
    }
}
