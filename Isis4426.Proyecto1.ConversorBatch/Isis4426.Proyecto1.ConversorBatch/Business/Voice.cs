using System;
using System.Collections.Generic;

namespace Isis4426.Proyecto1.ConversorBatch.Business
{
    internal static class Voice 
    {
        internal static void Process()
        {
            Data_Access.Voice voiceDal = new Data_Access.Voice();

            try
            {
                voiceDal.BeginTransaction();
                Models.Voice recentVoice = voiceDal.GetFirstPendingConvert();

                if (recentVoice != null)
                {
                    Models.Voice mp3Voice = Convert.ConvertVoiceToMp3(recentVoice);
                    bool email = Email.Send(mp3Voice);
                    bool update = voiceDal.Update(mp3Voice) != 0;

                    if (!(email || update))
                    {
                        Convert.Rollback(mp3Voice);
                        voiceDal.Rollback();
                    }
                    else
                    {
                        voiceDal.Commit();
                    } 
                }
            }
            catch (Exception ex)
            {
                voiceDal.Rollback();

                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                voiceDal.Close();
            }
        }

        internal static int GetCountPendingConvert()
        {
            try
            {
                Data_Access.Voice voiceDal = new Data_Access.Voice();

                return voiceDal.GetCountPendingVoices();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;              
            }
        }       
    }
}