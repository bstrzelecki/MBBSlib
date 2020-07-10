using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MBBSlib.MonoGame
{
    public class Input
    {
        public static Vector2 cameraOffset;
        public static Vector2 MousePosition => GetMousePosition();
        public static Vector2 RelativeMousePosition => MousePosition + GameMain.Instance.camera2D.Position;
        public static int MouseScrollDelta => GetMouseScrollDelta();
        private static bool _mouseDrag;
        public static bool MouseDrag
        {
            get => _mouseDrag; set
            {
                _mouseDrag = value;
                GameMain.Instance.IsMouseVisible = !value;
            }
        }
        public static Vector2 MouseDragDelta => _mouseDrag ? _mouseDragController.Drag : throw new MemberAccessException("Mouse drag calculation is not enabled");
        private static readonly MouseDragController _mouseDragController = new MouseDragController();
        internal static readonly Dictionary<Keys, Action> _actions = new Dictionary<Keys, Action>();
        public static void BindKey(Keys key, Action action) => _actions.Add(key, action);
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
        private static readonly Dictionary<Keys, bool> _keysClicked = new Dictionary<Keys, bool>();

        public static bool IsKeyClicked(Keys key)
        {
            if(!_keysClicked.ContainsKey(key))
            {
                _keysClicked.Add(key, false);
            }
            if(IsKeyDown(key) && !_keysClicked[key])
            {
                _keysClicked[key] = true;
                return true;
            }
            if(IsKeyUp(key) && _keysClicked[key])
            {
                _keysClicked[key] = false;
            }
            return false;
        }

        private static readonly bool[] _btnsClicked = new bool[3];
        public static bool IsMouseButtonClicked(int btn)
        {
            if(IsMouseKeyDown(btn) && !_btnsClicked[btn])
            {
                _btnsClicked[btn] = true;
                return true;
            }
            if(IsMouseKeyUp(btn) && _btnsClicked[btn])
            {
                _btnsClicked[btn] = false;
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
            switch(btn)
            {
                case 0:
                    return state.LeftButton == ButtonState.Pressed;
                case 1:
                    return state.MiddleButton == ButtonState.Pressed;
                case 2:
                    return state.RightButton == ButtonState.Pressed;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public static bool IsMouseKeyUp(int btn)
        {
            MouseState state = Mouse.GetState();
            switch(btn)
            {
                case 0:
                    return state.LeftButton == ButtonState.Released;
                case 1:
                    return state.MiddleButton == ButtonState.Released;
                case 2:
                    return state.RightButton == ButtonState.Released;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return false;
        }
        public static Vector2 GetMousePosition()
        {
            MouseState mouse = Mouse.GetState();
            return mouse.Position.ToVector2();
        }
        public static Vector2 ToWorldPosition(Vector2 pos) => pos - cameraOffset;

        private class MouseDragController : IUpdateable
        {
            public Vector2 Drag { get; set; }
            public MouseDragController() => GameMain.RegisterUpdate(this);
            public void Update()
            {
                if(!MouseDrag) return;

                Drag = MousePosition - new Vector2(GameMain.Instance.Resolution.Width / 2, GameMain.Instance.Resolution.Height / 2);
                Mouse.SetPosition(GameMain.Instance.Resolution.Width / 2, GameMain.Instance.Resolution.Height / 2);
            }
        }
    }
}
