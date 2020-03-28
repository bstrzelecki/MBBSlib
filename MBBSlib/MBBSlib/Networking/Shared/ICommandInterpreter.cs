namespace MBBSlib.Networking.Shared
{
    /// <summary>
    /// Interpreter that responds to client request at given id.
    /// </summary>
    public interface ICommandInterpreter
    {
        /// <summary>
        /// Code that will be executed during client request
        /// </summary>
        /// <param name="sender">Client id</param>
        /// <param name="data">byte array of non compressed data stream</param>
        void ExecuteCommand(XMLCommand cmd);
    }
}
