using MBBSlib.Math;
using Raylib_cs;

namespace MBBSlib.Raylib {
    public class RenderBatch {
        
        internal RenderBatch() { }

        public void Draw(Texture2D texture, Vector2 position) =>
            Raylib_cs.Raylib.DrawTexture(texture,  (int)position.x, (int)position.y, Color.WHITE);

        public void Draw(Texture2D texture, Vector2 position, Color color) =>
            Raylib_cs.Raylib.DrawTexture(texture, (int)position.x, (int)position.y, color);
        
        public void Draw(string texture, Vector2 position) =>
            Raylib_cs.Raylib.DrawTexture(new Sprite(texture), (int)position.x, (int)position.y, Color.WHITE);

        public void Draw(Texture2D texture, Math.Vector2 position, float rotation) =>
            Raylib_cs.Raylib.DrawTextureEx(texture, position, rotation, 1, Color.WHITE);
        //public void Draw(string textureName, Vector2 position, Color color) => _spriteBatch.Draw(new Sprite(textureName), position, color);

        //public void DrawString(SpriteFont font, string text, Vector2 position, Color color) => _spriteBatch.DrawString(font, text, position, color);
        //public void DrawString(string text, Vector2 position, Color color) => _spriteBatch.DrawString(new Font("font"), text, position, color);

    }
}