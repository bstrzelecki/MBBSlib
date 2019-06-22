using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MBBSlib.MonoGame
{
    public class Input
    {
        public static Vector2 cameraOffset;
        public static bool IsKeyDown(Keys key)
        {
            KeyboardState state = Keyboard.GetState();
            return state.IsKeyDown(key);
        }
        public static bool IsKeyUp(Keys key)
        {
            KeyboardState state = Keyboard.GetState();
            return state.IsKeyUp(key);
        }
        private static Dictionary<Keys, bool> keysClicked = new Dictionary<Keys, bool>();

        public static bool IsKeyClicked(Keys key)
        {
            if (!keysClicked.ContainsKey(key))
            {
                keysClicked.Add(key, false);
            }
            if (IsKeyDown(key) && !keysClicked[key])
            {
                keysClicked[key] = true;
                return true;
            }
            if (IsKeyUp(key) && keysClicked[key])
            {
                keysClicked[key] = false;
            }
            return false;
        }

        private static bool[] btnsClicked = new bool[3];
        public static bool IsMouseButtonClicked(int btn)
        {
            if (IsMouseKeyDown(btn) && !btnsClicked[btn])
            {
                btnsClicked[btn] = true;
                return true;
            }
            if (IsMouseKeyUp(btn) && btnsClicked[btn])
            {
                btnsClicked[btn] = false;
            }
            return false;
        }
        public static int GetMouseScrollDelta()
        {
            MouseState state = Mouse.GetState();
            return state.ScrollWheelValue;
        }
        public static bool IsMouseKeyDown(int btn)
        {
            MouseState state = Mouse.GetState();
            switch (btn)
            {
                case 0:
                    return state.LeftButton == ButtonState.Pressed;
                case 1:
                    return state.MiddleButton == ButtonState.Pressed;
                case 2:
                    return state.RightButton == ButtonState.Pressed;
            }
            return false;
        }
        public static bool IsMouseKeyUp(int btn)
        {
            MouseState state = Mouse.GetState();
            switch (btn)
            {
                case 0:
                    return state.LeftButton == ButtonState.Released;
                case 1:
                    return state.MiddleButton == ButtonState.Released;
                case 2:
                    return state.RightButton == ButtonState.Released;
            }
            return false;
        }
        public static Vector2 GetMousePosition()
        {
            MouseState mouse = Mouse.GetState();
            return mouse.Position.ToVector2();
        }
        public static Vector2 ToWorldPosition(Vector2 pos)
        {
            return pos - cameraOffset;
        }
    }
}
