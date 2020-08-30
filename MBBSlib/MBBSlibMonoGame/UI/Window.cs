using System;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;

namespace MBBSlib.MonoGame.UI
{
    public class Window : Panel, MBBSlib.MonoGame.IUpdateable
    {
        private Point _dragStart;
        private bool _isDragging;
        public int DragMargin { get; set; } = 30;
        public int TopMargin { get; set; } = 0;
        private int _layer = 5;
        private static int _focusedLayer = 5;
        private static bool _isAnyWindowDragged = false;
        public Window()
        {
            GameMain.RegisterUpdate(this);
        }

        public virtual void Focus()
        {
            GameMain.SetRendererLayer(this, ++_focusedLayer);
            _layer = _focusedLayer;
        }

        private Rectangle _dragRect => new Rectangle(Position, new Point(Size.Width, DragMargin));

        public void Update()
        {
            WindowDrag();
        }

        private void WindowDrag()
        {
            if (_dragRect.Contains(Input.MousePosition) && Input.IsMouseKeyDown(0) && IsVisible && !_isDragging && !_isAnyWindowDragged)
            {
                Focus();
                _dragStart = Input.MousePosition.ToPoint() - Position;
                _isDragging = true;
                _isAnyWindowDragged = true;
            }

            if (_isDragging)
                Position = Input.MousePosition.ToPoint() - _dragStart;

            if (Input.IsMouseKeyUp(0) || !IsVisible)
            {
                _dragStart = Point.Zero;
                _isDragging = false;
                _isAnyWindowDragged = false;
            }

            Position = new Point(System.Math.Clamp(Position.X, 0, GameMain.Instance.Resolution.Width - Size.Width),
                System.Math.Clamp(Position.Y, 0 + TopMargin, GameMain.Instance.Resolution.Height - Size.Height));
        }
    }
}