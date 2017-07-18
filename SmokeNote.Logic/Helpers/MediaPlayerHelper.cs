using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;

namespace SmokeNote.Logic.Helpers
{
    public static class MediaPlayerHelper
    {
        private static SoundPlayer player;
        private static bool _isPlaying = false;
        public static bool isPlaying
        {
            get
            {
                return _isPlaying;
            }
            set
            {
                _isPlaying = value;
            }
        }
        public static void PlaySound(string fileName, bool isLooping)
        {
            if (!File.Exists(fileName))
            {
                return;
            }
            if (player != null)
            {
                player.Stop();
                player = null;
            }
            player = new SoundPlayer(fileName);
            if (isLooping)
                player.PlayLooping();
            else
                player.Play();

            _isPlaying = true;
        }
        public static void Stop()
        {
            if (player != null)
                player.Stop();
            _isPlaying = false;
        }
    }
}
