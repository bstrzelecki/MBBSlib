namespace MBBSlib.MonoGame
{
    public class Renderer
    {
        public int layer;
        public IDrawable drawable;
        public Renderer(int l, IDrawable draw)
        {
            layer = l;
            drawable = draw;
        }
    }
}