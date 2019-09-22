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
                if(obj is Renderer r)
                {
                    return ((r.layer == layer) && (r.drawable == drawable));
                }
                return false;
            }
        }
    }
}
