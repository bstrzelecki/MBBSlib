using Microsoft.Xna.Framework.Graphics;

namespace MBBSlib.MonoGame
{
    public interface IGetTexture
    {
        Texture2D GetTexture(string key);
        bool ContainsTextureKey(string key);
        SpriteFont GetFont(string key);
    }
}
