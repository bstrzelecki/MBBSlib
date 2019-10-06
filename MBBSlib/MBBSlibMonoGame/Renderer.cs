namespace MBBSlib.MonoGame
{
    public partial class GameMain
    {
#pragma warning disable CS0659 // 'GameMain.Renderer' overrides Object.Equals(object o) but does not override Object.GetHashCode()
        protected struct Renderer
#pragma warning restore CS0659 // 'GameMain.Renderer' overrides Object.Equals(object o) but does not override Object.GetHashCode()
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
