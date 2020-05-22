using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace MBBSlib.MonoGame
{
    public static class Sounds
    {
        public static float Volume { get => MediaPlayer.Volume; set => MediaPlayer.Volume = value; }
        private static readonly Dictionary<string, Song> songs = new Dictionary<string, Song>();
        public static void Loop(Song song)
        {
            MediaPlayer.Play(song);
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        }
        public static void Loop(string song)
        {
            Song s = GetSong(song);
            Loop(s);
        }
        public static void Play(string song)
        {
            Song s = GetSong(song);
            MediaPlayer.Play(s);
        }
        private static Song GetSong(string song)
        {
            if(songs.ContainsKey(song))
            {
                return songs[song];
            }
            else
            {
                var s = Song.FromUri(song, new Uri(Environment.CurrentDirectory + @"Content\Audio\" + song));
                songs.Add(song, s);
                return s;
            }
        }
        private static Song _currentSong;
        private static void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
        {
            if(_currentSong == null) return;
            MediaPlayer.Play(_currentSong);
        }
        public static void Stop()
        {
            _currentSong = null;
            MediaPlayer.Stop();
        }
    }
}
