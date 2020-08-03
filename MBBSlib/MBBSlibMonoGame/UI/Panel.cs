using System.Collections.Generic;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;

namespace MBBSlib.MonoGame.UI
{
    public class Panel : MBBSlib.MonoGame.IDrawable
    {
        public Color BackgroundColor = Color.Transparent;

        protected Panel()
        {
            Parent = null;
        }

        public virtual Rectangle Size { get; set; }

        public Point Position
        {
            get => Size.Location;
            set => Size = new Rectangle(value, Size.Size);
        }

        public Point RelativePosition => GetParentPosition() + Position;

        public Panel Parent { get; protected set; }
        protected List<Panel> Children { get; set; } = new List<Panel>();
        public bool IsVisible { get; set; } = true;


        public bool UseRelativePosition { get; set; } = true;

        public virtual void Draw(RenderBatch sprite)
        {
            //If object is not visible ignore rest
            if (!IsVisible) return;

            //Draw background of the panel
            sprite.Draw(new Sprite("WhitePixel"), new Rectangle(RelativePosition, Size.Size), BackgroundColor);

            //Draw children
            foreach (Panel child in Children) child.Draw(sprite);
        }

        public void AddChildren(Panel panel)
        {
            panel.Parent = this;
            Children.Add(panel);
        }

        private Point GetParentPosition()
        {
            if (Parent == null) return Point.Zero;
            return Parent.Position + Parent.GetParentPosition();
        }
    }
}