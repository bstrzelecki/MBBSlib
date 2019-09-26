using System;
using System.Collections.Generic;
using System.Text;

namespace MBBSlib.MonoGame.UI
{
    class Window : ContentPane, IDisposable
    {
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
