using System.Collections.Generic;
using System.IO;

namespace Isis4426.Proyecto1.ConversorBatch.Business
{
    internal static class Voice 
    {
        private static Data_Access.Voice voiceDal;
        
        public static List<Models.Voice> PendingConvert()
        {
            voiceDal = new Data_Access.Voice();
            return voiceDal.PendingConvert();
        }

        public static int Update(Models.Voice voice)
        {            
            voice.Destiny = new FileInfo(Path.ChangeExtension(voice.Origin.FullName, ".mp3"));            
            voiceDal = new Data_Access.Voice();

            return voiceDal.Update(voice);
        }
    }
}
