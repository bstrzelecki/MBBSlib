using System;

namespace MBBSlib.MonoGame
{
    public class Time : IUpdateable
    {
        public static int DeltaTime { get; private set; }

        private static int start;
        public static event Action OnTick;
        private int tickCounter;
        private const int tickTime = 30;
        public void Update()
        {
            DeltaTime = DateTime.Now.Millisecond - start;
            DeltaTime = DeltaTime > 0 ? DeltaTime : 0;
            start = DateTime.Now.Millisecond;
            tickCounter += DeltaTime;
            if (tickCounter > tickTime)
            {
                tickCounter = 0;
                OnTick();
            }
        }
    }
}
