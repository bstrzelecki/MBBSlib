using Raylib_cs;

namespace MBBSlib.Raylib
{
    public interface IGetTexture
    {
        Texture2D GetTexture(string key);
        bool ContainsTextureKey(string key);
        //SpriteFont GetFont(string key);
    }
}
