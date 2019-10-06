using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MBBSlib.MonoGame.UI
{
    class Window : ContentPane, IDisposable, IUpdateable
    {
        public string Title { get; set; } = "Window.";
#pragma warning disable CS0649 // Field 'Window.topBar' is never assigned to, and will always have its default value null
        public Sprite topBar;
#pragma warning restore CS0649 // Field 'Window.topBar' is never assigned to, and will always have its default value null
#pragma warning disable CS0649 // Field 'Window.exitBtn' is never assigned to, and will always have its default value null
        public Sprite exitBtn;
#pragma warning restore CS0649 // Field 'Window.exitBtn' is never assigned to, and will always have its default value null
#pragma warning disable CS0649 // Field 'Window.font' is never assigned to, and will always have its default value null
        public SpriteFont font;
#pragma warning restore CS0649 // Field 'Window.font' is never assigned to, and will always have its default value null
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
            if(topBar != null && exitBtn != null)
            {
                sprite.Draw(topBar, new Rectangle(Transform.Location, new Point(TopBarWidth, 12)));
                sprite.Draw(exitBtn, new Rectangle(TopBarWidth, 0, exitBtn.Size.Width, exitBtn.Size.Height));
                sprite.DrawString(font, Title, Transform.Location.ToVector2(), Color.White);
            }
            base.Draw(sprite);

        }

        public void Update()
        {
            if (!IsVisible) return;
            //if(Input.GetMousePosition() > new Vector2(TopBarWidth, 0) && Input.GetMousePosition() < new Vector2(TopBarWidth + 12, 12))
            {

            }
        }
    }
}
