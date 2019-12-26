using Microsoft.Xna.Framework.Graphics;

namespace MBBSlib.MonoGame
{
    public class Font
    {
        readonly string font = string.Empty;
        public Font(string fontName)
        {
            font = fontName;
        }

        public static implicit operator SpriteFont(Font font)
        {
            return GameMain.fonts[font.font];
        }
    }
}
