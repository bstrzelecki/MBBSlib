using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MBBSlib.MonoGame.UI
{
    public class Window : ContentPane, IDisposable, IUpdateable
    {
        public string Title { get; set; } = "Window.";
        public Sprite topBar;
        public Sprite exitBtn;
        public SpriteFont font;
        public bool IsVisible { get; set; } = true;
        public int TopBarWidth => Transform.Width - exitBtn.Size.Width;
        public Window() => GameMain.RegisterRenderer(this);

        public void Dispose() => GameMain.UnregisterRenderer(this);
        public override void Draw(RenderBatch sprite)
        {
            if(!IsVisible) return;
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
            if(!IsVisible) return;
            //if(Input.GetMousePosition() > new Vector2(TopBarWidth, 0) && Input.GetMousePosition() < new Vector2(TopBarWidth + 12, 12))
            {

            }
        }
    }
}
