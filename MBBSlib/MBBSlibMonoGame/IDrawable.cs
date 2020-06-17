namespace MBBSlib.MonoGame
{
    /// <summary>
    /// Interface for renderer tcicks
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Calls every <see cref="GameMain.Draw(Microsoft.Xna.Framework.GameTime)"/>
        /// </summary>
        /// <param name="sprite"></param>
        void Draw(RenderBatch sprite);
    }
}
