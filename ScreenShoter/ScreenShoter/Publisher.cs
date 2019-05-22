using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenShoter
{
    internal static class Publisher
    {
        public static event EventHandler EventCaptured;
        public static event EventHandler EventDeleted;
        public static event EventHandler EventError;
        public static event EventHandler EventHideBallonTip;

        public static void ScreenCaptured() {
            PlaySound(SOUND.PASTE);
            EventCaptured?.Invoke(null, null);
        }

        public static void ScreenDeleted()
        {
            PlaySound(SOUND.DELETE);
            EventDeleted?.Invoke(null, null);
        }

        public static void ScreenError()
        {
            PlaySound(SOUND.DELETE);
            EventError?.Invoke(null, null);
        }

        public static void HideBallonTip() {
            EventHideBallonTip?.Invoke(null, null);
        }
        
        #region Sound Player implementation
        private enum SOUND
        {
            PASTE, DELETE, ERROR
        }

        private static void PlaySound(SOUND soundType)
        {
            System.Media.SoundPlayer player = null;
            string winPath = Environment.ExpandEnvironmentVariables("%SystemRoot%") + "\\Media\\";
            switch (soundType)
            {
                case SOUND.PASTE:
                    player = new System.Media.SoundPlayer(winPath + "Speech On.wav");
                    break;
                case SOUND.DELETE:
                    player = new System.Media.SoundPlayer(winPath + "Speech Off.wav");
                    break;
                case SOUND.ERROR:
                    break;
                default:
                    throw new ArgumentException("add argument: " + soundType.ToString());
            }
            player?.Play();
        }
        #endregion
    }

}
