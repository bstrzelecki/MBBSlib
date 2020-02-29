using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;
namespace MBBSlib.MonoGame
{
    public static class Sounds
    {
        public static float Volume { get { return MediaPlayer.Volume; } set { MediaPlayer.Volume = value; } }
        public static void Loop(Song song)
        {
            MediaPlayer.Play(song);
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        }
        private static Song _currentSong;
        private static void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
        {
            if (_currentSong == null) return;
            MediaPlayer.Play(_currentSong);
        }
        public static void Stop()
        {
            _currentSong = null;
            MediaPlayer.Stop();
        }
    }
}
