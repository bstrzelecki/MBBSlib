using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MBBSlib.MonoGame.UI
{
    class Window : ContentPane, IDisposable
    {
        public string Title { get; set; } = "Window.";
        public Sprite topBar;
        public Sprite exitBtn;
        public bool IsVisible { get; set; } = true;
        public int TopBarWidth
        {
            get
            {
                return Transform.Width - exitBtn.Size.Width;
            }
        }
        public Window()
        {
            GameMain.RegisterRenderer(this);
        }

        public void Dispose()
        {
            GameMain.UnregisterRenderer(this);
        }
        public override void Draw(SpriteBatch sprite)
        {
            if (!IsVisible) return;
            base.Draw(sprite);
            if(topBar != null && exitBtn != null)
            {
                sprite.Draw(topBar, new Rectangle(Transform.Location, new Point(TopBarWidth, 12)));
                sprite.Draw(exitBtn, new Rectangle(TopBarWidth, 0, exitBtn.Size.Width, exitBtn.Size.Height));
            }

        }
    }
}
