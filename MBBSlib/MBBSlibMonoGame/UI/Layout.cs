namespace MBBSlib.MonoGame.UI
{
    public class Layout : Panel
    {
        public Layout(params Panel[] children)
        {
            foreach (Panel child in children) AddChildren(child);
        }
    }
}