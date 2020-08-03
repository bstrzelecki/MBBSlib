using System;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;

namespace MBBSlib.MonoGame.UI
{
    public class Button : Panel, MBBSlib.MonoGame.IUpdateable
    {
        public bool UseTextSize = true;

        public Button()
        {
            GameMain.RegisterUpdate(this);
            Text = new Text {Position = new Point(TextMarginLeft, TextMarginTop)};
            Image = new Image();
            AddChildren(Image);
            var stack = new StackPanel(Image, Text) {Orientation = Orientation.Horizontal};
            AddChildren(stack);
        }

        public Button(Style<Button> prefab) : this()
        {
            BackgroundColor = prefab.Prefab.BackgroundColor;
            Size = prefab.Prefab.Size;
            UseTextSize = prefab.Prefab.UseTextSize;
            TextMarginLeft = prefab.Prefab.TextMarginLeft;
            TextMarginTop = prefab.Prefab.TextMarginTop;
            Image.Size = prefab.Prefab.Image.Size;
            Image.LockSize = prefab.Prefab.Image.LockSize;
            Image.Tint = prefab.Prefab.Image.Tint;
        }

        public Button(string text, Style<Button> prefab):this(prefab)
        {
            Text.Value = text;
        }

        public Button(Sprite image, Style<Button> prefab):this(prefab)
        {
            Image.Value = image;
        }
        public Text Text { get; }
        public Image Image { get; }
        public int TextMarginLeft { get; set; } = 0;
        public int TextMarginTop { get; set; } = 0;

        public void Update()
        {
            if (UseTextSize)
                Size = new Rectangle(Position, Image.Size.Size + Text.Size.Size);
            if (new Rectangle(RelativePosition, Size.Size).Contains(Input.MousePosition))
            {
                if (Input.IsMouseButtonClicked(0)) OnClicked?.Invoke();
                OnHover?.Invoke();
            }
        }

        public event Action OnClicked;
        public event Action OnHover;
    }
}