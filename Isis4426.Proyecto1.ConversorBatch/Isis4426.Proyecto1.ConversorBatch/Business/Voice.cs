using System;
using System.Collections.Generic;

namespace Isis4426.Proyecto1.ConversorBatch.Business
{
    internal static class Voice 
    {        
        public static List<Models.Voice> PendingConvert()
        {
            try
            {
                Data_Access.Voice voiceDal = new Data_Access.Voice();

                return voiceDal.PendingConvert();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;              
            }
        }

        public static int Update(Models.Voice voice)
        {
            try
            {
                Data_Access.Voice voiceDal = new Data_Access.Voice();

                return voiceDal.Update(voice);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;            
            }
        }
    }
}