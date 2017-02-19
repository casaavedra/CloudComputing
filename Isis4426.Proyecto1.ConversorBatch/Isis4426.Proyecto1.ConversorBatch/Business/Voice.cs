using System.Collections.Generic;

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
            voiceDal = new Data_Access.Voice();

            return voiceDal.Update(voice);
        }
    }
}