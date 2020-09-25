using Microsoft.Xna.Framework;
using System.Linq;

namespace MBBSlib.MonoGame.UI
{
    public class StackPanel : Layout
    {
        private Rectangle _size;
        public Orientation Orientation = Orientation.Horizontal;
        public int Spaceing = 0;

        public StackPanel(params Panel[] children) : base(children)
        {
        }

        public override Rectangle Size
        {
            get
            {
                var sumx = 0;
                var sumy = 0;
                foreach (Panel child in Children)
                {
                    sumx += child.Size.Width;
                    sumy += child.Size.Height;
                }

                var rect = new Rectangle(_size.Location, new Point(0, 0));
                if (Orientation == Orientation.Horizontal)
                {
                    rect.Width = sumx;
                    rect.Height = Children.Select(child => child.Size.Height).Concat(new[] { 0 }).Max();
                }
                else
                {
                    rect.Width = Children.Select(child => child.Size.Width).Concat(new[] { 0 }).Max();
                    rect.Height = sumy;
                }

                return rect;
            }
            set => _size.Location = value.Location;
        }

        public override void Draw(RenderBatch sprite)
        {
            //Initialize offset based on current StackPanel position
            var offset = 0; // this.Orientation == Orientation.Horizontal ? RelativePosition.X : RelativePosition.Y;

            //Stack children
            foreach (Panel c in Children)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    c.Position = new Point(offset, 0);
                    offset += c.Size.Width + Spaceing;
                }
                else
                {
                    c.Position = new Point(0, offset);
                    offset += c.Size.Height + Spaceing;
                }

                c.Draw(sprite);
            }
        }
    }
}