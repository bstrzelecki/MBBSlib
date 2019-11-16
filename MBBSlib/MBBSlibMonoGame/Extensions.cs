using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MBBSlib.MonoGame
{
    public static class Extensions
    {
        public static void Draw(this SpriteBatch sprite, Texture2D texture, Rectangle size)
        {
            sprite.Draw(texture, size, Color.White);
        }
        public static void Draw(this SpriteBatch sprite, Texture2D texture, Vector2 position)
        {
            sprite.Draw(texture, position, Color.White);
        }
        public static void Draw(this SpriteBatch sprite, string textureName, Vector2 position)
        {
            sprite.Draw(new Sprite(textureName), position, Color.White);
        }
        public static void Draw(this SpriteBatch sprite, string textureName, Vector2 position, Color color)
        {
            sprite.Draw(new Sprite(textureName), position, color);
        }
    }
}
