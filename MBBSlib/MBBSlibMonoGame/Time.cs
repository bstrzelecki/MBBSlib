using System;

namespace MBBSlib.MonoGame
{
    public static class Time
    {
        public static int DeltaTime => tc.DeltaTime;
        public static bool IsPaused { get => tc.IsPaused; set => tc.IsPaused = value; }
        public static int TickTime { get => tc.TickTime; set => tc.TickTime = value; }
        public static event Action OnTick;
        internal static TickCounter tc;
        internal static void Initialize() => tc = new TickCounter();
        internal class TickCounter : IUpdateable
        {
            public int DeltaTime { get; private set; }
            public bool IsPaused { get; set; } = false;
            public int TickTime { get; set; } = 30;
            private static int start;
            private static int tickCounter;
            public TickCounter() => GameMain.RegisterUpdate(this);
            public void Update()
            {
                if(IsPaused)
                {
                    DeltaTime = 0;
                    return;
                }
                DeltaTime = DateTime.Now.Millisecond - start;
                DeltaTime = DeltaTime > 0 ? DeltaTime : 0;
                start = DateTime.Now.Millisecond;
                tickCounter += DeltaTime;
                if(tickCounter > TickTime)
                {
                    tickCounter = 0;
                    OnTick?.Invoke();
                }
            }
        }
    }
}
