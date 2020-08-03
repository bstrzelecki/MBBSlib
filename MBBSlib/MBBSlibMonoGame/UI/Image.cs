using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;

namespace MBBSlib.MonoGame.UI
{
    public class Image : Panel
    {
        private Sprite _value;

        public Sprite Value
        {
            get => _value;
            set
            {
                _value = value;
                if (!LockSize)
                    Size = new Rectangle(Position, _value.Size.Size);
            }
        }


        public bool LockSize { get; set; } = false;
        public Color Tint { get; set; } = Color.White;

        public override void Draw(RenderBatch sprite)
        {
            //Draw background of the panel
            sprite.Draw(new Sprite("WhitePixel"), new Rectangle(RelativePosition, Size.Size), BackgroundColor);

            //FIXME
            if (Value != null)
                sprite.Draw(new Sprite(Value.ToString()), new Rectangle(RelativePosition, Size.Size), Tint);


            //Draw children
            foreach (Panel child in Children) child.Draw(sprite);
        }
    }
}