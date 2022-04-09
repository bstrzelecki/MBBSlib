using System;
using MBBSlib.Math;
using Raylib_cs;

namespace MBBSlib.Raylib {
    public class Sprite {
        public Texture2D Texture { get; protected set; }

        public static IGetTexture TextureStorage;
        
        private string _textureName = string.Empty;

        public Sprite(Texture2D sprite) {
            Texture = sprite;
            _textureName = "initializedByStructReference";
        }
        
        public Sprite(string sprite)
        {
            Texture = TextureStorage.GetTexture(sprite);
            
            _textureName = sprite;
        }
        
        private void _initialize() {
            if (Texture.id != 0) return;
            Texture = TextureStorage.GetTexture(_textureName);
        }

        public Vector2 Size {
            get
            {
                _initialize();
                return new Vector2(Texture.width, Texture.height);
            }
        }
        
        public static implicit operator Texture2D(Sprite sprite) {
            sprite._initialize();
            return sprite.Texture;
        }
        
        public override string ToString() => _textureName;
    }
}