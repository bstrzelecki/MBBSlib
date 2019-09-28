using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MBBSlib.MonoGame.UI
{
    public abstract class ContentPane : IDrawable
    {
        public Sprite Background { get; set; }
        public Rectangle Transform { get; set; } = new Rectangle(0, 0, 250, 250);
        public Color BackgroundColor { get; set; } = Color.White;
        public bool Focused { get; set; }

        public virtual void Draw(SpriteBatch sprite)
        {
            if (sprite == null) return;
            sprite.Draw(Background, Transform, BackgroundColor);
        }
    }
}
