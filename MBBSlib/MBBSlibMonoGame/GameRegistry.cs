using System;
using System.Collections.Generic;
using System.Text;

namespace MBBSlib.MonoGame
{
    public partial class GameMain
    {
        private static List<Renderer> renderers = new List<Renderer>();
        private static readonly List<IUpdateable> updates = new List<IUpdateable>();
        private static readonly List<IAudioSource> audioSources = new List<IAudioSource>();
        private readonly static List<Renderer> queuedRenderers = new List<Renderer>();
        private readonly static List<IUpdateable> queuedUpdates = new List<IUpdateable>();
        private readonly static List<Renderer> rmQueuedRenderers = new List<Renderer>();
        private readonly static List<IUpdateable> rmQueuedUpdates = new List<IUpdateable>();

        public void RegisterAudioSource(IAudioSource source)
        {
            audioSources.Add(source);
        }

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
