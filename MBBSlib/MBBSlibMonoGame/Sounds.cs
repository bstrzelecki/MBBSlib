using Microsoft.Xna.Framework.Media;
using System;
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
        public static void Loop(string song)
        {
            Song s = Song.FromUri(song, new Uri(Environment.CurrentDirectory + @"Content\Audio\" + song));
            Loop(s);
        }
        public static void Play(string song)
        {
            Song s = Song.FromUri(song, new Uri(Environment.CurrentDirectory + @"Content\Audio\" + song));
            MediaPlayer.Play(s)
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
