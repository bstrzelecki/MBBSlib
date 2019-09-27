using System;
using System.Collections.Generic;
using System.Text;

namespace MBBSlib.MonoGame.UI
{
    class Window : ContentPane, IDisposable
    {
        public string Title { get; set; } = "Window.";
        public Sprite topBar;
        public Sprite exitBtn;
        public Window()
        {
            GameMain.RegisterRenderer(this);
        }

        public void Dispose()
        {
            GameMain.UnregisterRenderer(this);
        }
    }
}
