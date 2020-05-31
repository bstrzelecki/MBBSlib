using Microsoft.Xna.Framework.Graphics;

namespace MBBSlib.MonoGame
{
    /// <summary>
    /// Wrapper for <see cref="SpriteFont"/>
    /// </summary>
    public class Font
    {
        readonly string _font = string.Empty;
        /// <summary>
        /// Creates pointer to <see cref="SpriteFont"/> in <see cref="GameMain._fonts"/>
        /// </summary>
        /// <param name="fontName">File name without extension</param>
        public Font(string fontName) => _font = fontName;

        /// <summary>
        /// Calls <see cref="GameMain.GetFont(string)"/>
        /// </summary>
        /// <param name="font"><see cref="Font"/> object</param>
        public static implicit operator SpriteFont(Font font) => GameMain.Instance.GetFont(font._font);
    }
}
