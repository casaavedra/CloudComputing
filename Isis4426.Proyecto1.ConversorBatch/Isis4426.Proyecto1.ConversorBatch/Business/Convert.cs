using Isis4426.Proyecto1.ConversorBatch.Models;
using System.IO;
using System.Diagnostics;

namespace Isis4426.Proyecto1.ConversorBatch
{
    internal static class Convert 
    {
        internal static Voice ConvertVoiceToMp3(Voice voice)
        {
            string destinyName = Path.Combine(voice.Origin.DirectoryName, voice.Consecutive.ToString() + voice.Origin.Name);
            voice.Destiny = new FileInfo( Path.ChangeExtension(destinyName, ".mp3"));
            
            var process = new Process
            {
                StartInfo =
                {
                    FileName = @"ffmpeg\bin\ffmpeg",
                    Arguments = string.Format("-i {0} -codec:a libmp3lame -qscale:a 2 {1}",
                    voice.Origin.FullName, voice.Destiny.FullName)
                }
            };

            process.Start();
            process.WaitForExit(500);

            return Validate(voice);
        }

        internal static void Rollback(Voice voice)
        {
            if (voice.Destiny.Exists)
            {
                File.Delete(voice.Destiny.FullName);
            }
        }

        private static Voice Validate(Voice voice)
        {
            Voice newVoice = voice;
            newVoice.State = File.Exists(voice.Destiny.FullName) ? Status.GENERATED : Status.ERROR;

            return newVoice;
        }
    }
}