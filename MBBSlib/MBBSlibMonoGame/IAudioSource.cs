using Microsoft.Xna.Framework;

namespace MBBSlib.MonoGame
{
    public interface IAudioSource
    {
        Vector3 Position { get; }
        void PlayAudio();
    }
}
