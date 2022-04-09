using System;
using MBBSlib.Visuals;

namespace MBBSlib.MonoGame
{
    /// <summary>
    /// Keeps track of physics updates
    /// </summary>
    public static class Time
    {
        /// <summary>
        /// Difference between frames in ms.
        /// </summary>
        public static int DeltaTime => _tc.DeltaTime;
        /// <summary>
        /// Is false when game time is passing
        /// </summary>
        public static bool IsPaused { get => _tc.IsPaused; set => _tc.IsPaused = value; }
        /// <summary>
        /// Number of ms between <see cref="OnTick"/> calls
        /// </summary>
        public static int TickTime { get => _tc.TickTime; set => _tc.TickTime = value; }
        /// <summary>
        /// Gets called every <see cref="TickTime"/> ms when <see cref="IsPaused"/> is false 
        /// </summary>
        public static event Action OnTick;
        internal static TickCounter _tc;
        internal static void Initialize() => _tc = new TickCounter();
        internal class TickCounter : IUpdateable
        {
            public int DeltaTime { get; private set; }
            public bool IsPaused { get; set; } = false;
            public int TickTime { get; set; } = 30;
            private static int _start;
            private static int _tickCounter;
            public TickCounter() => GameMain.RegisterUpdate(this);
            public void Update()
            {
                if (IsPaused)
                {
                    DeltaTime = 0;
                    return;
                }
                DeltaTime = DateTime.Now.Millisecond - _start;
                DeltaTime = DeltaTime > 0 ? DeltaTime : 0;
                _start = DateTime.Now.Millisecond;
                _tickCounter += DeltaTime;
                if (_tickCounter > TickTime)
                {
                    _tickCounter = 0;
                    OnTick?.Invoke();
                }
            }
        }
    }
}
