using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MBBSlib.MonoGame
{
    public class Sprite
    {
        public Texture2D Texture { get; protected set; }
        public Rectangle Size { get; protected set; }
        private string textureName = string.Empty;
        public static IGetTexture TextureStorage;
        public Sprite(Texture2D sprite)
        {
            Texture = sprite;
            Size = sprite.Bounds;
        }
        public Sprite(string sprite)
        {
            if (TextureStorage.ContainsTextureKey(sprite))
            {
                Texture = TextureStorage.GetTexture(sprite);
                //Size = Texture.Bounds;
            }
            else
            {
                textureName = sprite;
            }
        }
        public static implicit operator Texture2D(Sprite sprite)
        {
            if (sprite.Texture == null && sprite.textureName != string.Empty)
            {
                sprite.Texture = TextureStorage.GetTexture(sprite.textureName);
            }
            if (sprite.Texture == null)
            {
                sprite.Texture = new Texture2D(null , 1, 1);
            }

            return sprite.Texture;
        }
    }
}