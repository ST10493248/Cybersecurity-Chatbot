using System;
using System.IO;
using System.Media;

namespace CyberSecurityChatbot
{
    public class AudioService
    {
        public void PlayGreeting()
        {
            string audioPath = FindAudioFile();

            if (audioPath != null)
            {
                try
                {
                    using (SoundPlayer player = new SoundPlayer(audioPath))
                    {
                        player.Play(); // Changed to Play() for non-blocking
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Audio error: {ex.Message}");
                }
            }
        }

        private string FindAudioFile()
        {
            string[] possiblePaths = {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "audio", "greeting.wav"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "greeting.wav"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "welcome.wav")
            };

            foreach (string path in possiblePaths)
            {
                if (File.Exists(path))
                    return path;
            }
            return null;
        }
    }
}