using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MBBSlib.MonoGame.UI
{
    public class Text : Panel
    {
        private Rectangle _size;
        private Vector2 _textSize = Vector2.Zero;
        private string _value = string.Empty;
        public Font Font = new Font("font");

        public Text()
        {
            
        }
        public Text(string value)
        {
            Value = value;
        }

        public Text(string value, Style<Text> prefab):this(prefab)
        {
            Value = value;
        }
        public Text(Style<Text> prefab)
        {
            Color = prefab.Prefab.Color;
            DrawBackground = prefab.Prefab.DrawBackground;
            Font = prefab.Prefab.Font;
        }    
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                _textSize = ((SpriteFont) Font)?.MeasureString(Value) ?? Vector2.Zero;
            }
        }

        public Color Color { get; set; } = Color.Green;
        public bool DrawBackground { get; set; } = true;

        public override Rectangle Size
        {
            get => new Rectangle(_size.Location, _textSize.ToPoint());
            set => _size.Location = value.Location;
        }

        public override void Draw(RenderBatch sprite)
        {
            if (DrawBackground)
                base.Draw(sprite);
            if (Value == string.Empty) return;
            _textSize = ((SpriteFont) Font)?.MeasureString(Value) ?? Vector2.Zero;
            sprite.DrawString(new Font("font"), Value, RelativePosition.ToVector2(), Color);
        }
    }
}