using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MBBSlib.MonoGame
{
    class InputBindHandler : IUpdateable
    {
        public InputBindHandler()
        {
            GameMain.RegisterUpdate(this);
        }
        public void Update()
        {
            foreach(Keys key in Input.actions.Keys)
            {
                if (Input.IsKeyClicked(key))
                {
                    Input.actions[key].Invoke();
                }
            }
        }
    }
}
