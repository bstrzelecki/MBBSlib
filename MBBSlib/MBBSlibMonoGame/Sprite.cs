using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MBBSlib.MonoGame
{
    public class Sprite
    {
        public Texture2D Texture { get; protected set; }
        public Rectangle Size
        {
            get
            {
                if(Texture == null && textureName != string.Empty)
                {
                    Texture = TextureStorage.GetTexture(textureName);
                }
                if(Texture == null)
                {
                    Texture = new Texture2D(GameMain.graphics.GraphicsDevice, 1, 1);

                }
                return Texture.Bounds;
            }
        }
        protected string textureName = string.Empty;
        public static IGetTexture TextureStorage;
        public Sprite(Texture2D sprite) => Texture = sprite;
        public Sprite(string sprite)
        {
            if(TextureStorage.ContainsTextureKey(sprite))
            {
                Texture = TextureStorage.GetTexture(sprite);
                //Size = Texture.Bounds;
            }
            else
            {
                Debug.WriteLine("Cannot find " + sprite);
                var myAssembly = Assembly.GetExecutingAssembly();
                Stream myStream = myAssembly.GetManifestResourceStream("MBBSlib.MonoGame.Content.WhitePixel.png");
                if(myStream != null)
                    Texture = Texture2D.FromStream(GameMain.graphicsDevice, myStream);
            }
            textureName = sprite;
        }
        public static implicit operator Texture2D(Sprite sprite)
        {
            if(sprite.Texture == null && sprite.textureName != string.Empty)
            {
                sprite.Texture = TextureStorage.GetTexture(sprite.textureName);
            }
            if(sprite.Texture == null)
            {
                sprite.Texture = new Texture2D(GameMain.graphics.GraphicsDevice, 1, 1);

            }

            return sprite.Texture;
        }
        public override string ToString() => textureName;
    }
}