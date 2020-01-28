using System.Collections.Generic;

namespace MBBSlib.MonoGame
{
    public partial class GameMain
    {
        protected struct Renderer
        {
            public int layer;
            public IDrawable drawable;
            public Renderer(int l, IDrawable draw)
            {
                layer = l;
                drawable = draw;
            }
            public override bool Equals(object obj)
            {
                if (obj is Renderer r)
                {
                    return ((r.layer == layer) && (r.drawable == drawable));
                }
                return false;
            }

            public override int GetHashCode()
            {
                var hashCode = -628141515;
                hashCode = hashCode * -1521134295 + layer.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<IDrawable>.Default.GetHashCode(drawable);
                return hashCode;
            }
        }
    }
}
