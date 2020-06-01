using System.Collections.Generic;

namespace MBBSlib.MonoGame
{
    public partial class GameMain
    {
        private static List<Renderer> _renderers = new List<Renderer>();
        private static readonly List<IUpdateable> _updates = new List<IUpdateable>();
        private static readonly List<IAudioSource> _audioSources = new List<IAudioSource>();
        private readonly static List<Renderer> _queuedRenderers = new List<Renderer>();
        private readonly static List<IUpdateable> _queuedUpdates = new List<IUpdateable>();
        private readonly static List<Renderer> _rmQueuedRenderers = new List<Renderer>();
        private readonly static List<IUpdateable> _rmQueuedUpdates = new List<IUpdateable>();
        /// <summary>
        /// Registers existing audio source
        /// </summary>
        /// <param name="source"></param>
        public void RegisterAudioSource(IAudioSource source) => _audioSources.Add(source);

        private struct Renderer
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
