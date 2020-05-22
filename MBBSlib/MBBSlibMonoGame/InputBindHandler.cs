using Microsoft.Xna.Framework.Input;

namespace MBBSlib.MonoGame
{
    class InputBindHandler : IUpdateable
    {
        public InputBindHandler() => GameMain.RegisterUpdate(this);
        public void Update()
        {
            foreach(Keys key in Input._actions.Keys)
            {
                if(Input.IsKeyClicked(key))
                {
                    Input._actions[key].Invoke();
                }
            }
        }
    }
}
