using System.Collections.Generic;

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
        /// <summary>
        /// Registers existing audio source
        /// </summary>
        /// <param name="source"></param>
        public void RegisterAudioSource(IAudioSource source) => audioSources.Add(source);

        protected struct Renderer
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
}
