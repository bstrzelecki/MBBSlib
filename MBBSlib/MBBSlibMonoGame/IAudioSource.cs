using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MBBSlib.MonoGame
{
    public interface IAudioSource
    {
        Vector3 Position { get; }
        void PlayAudio();
    }
}
