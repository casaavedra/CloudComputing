using Isis4426.Proyecto1.ConversorBatch.Business;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Timers;

namespace Isis4426.Proyecto1.ConversorBatch
{
    public class BatchConverter
    {
        private readonly Timer batchTimer;

        public BatchConverter()
        {
            Configuration.Load();

            batchTimer = new Timer { Interval = Models.Configuration.Instance.ConvertTimer };
            batchTimer.Elapsed += BatchTimer_Elapsed;            
        }

        public void StartConvertion()
        {
            batchTimer.Start();
            //SecuencialProcess();
        }

        private void BatchTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            batchTimer.Stop();

            try
            {                
                SecuencialProcess();
                //ParallelProcess();
            }
            catch (AggregateException ae)
            {
                foreach (var ex in ae.InnerExceptions)
                {
                    Console.WriteLine(ex.Message);                    
                }

                batchTimer.Start();
            }
        }

        private void SecuencialProcess()
        {
            foreach (Models.Voice voice in Voice.PendingConvert())
            {
                Models.Voice newVoice = Convert.ConvertVoiceToMp3(voice);
                if (!Email.Send(newVoice)) Convert.Rollback(newVoice);
                if (Voice.Update(newVoice) == 0) Convert.Rollback(newVoice);
            }
        }

        private void ParallelProcess()
        {
            var exceptions = new ConcurrentQueue<Exception>();

            try
            {
                Parallel.ForEach(Voice.PendingConvert(), (voice) =>
                {
                    Models.Voice newVoice = Convert.ConvertVoiceToMp3(voice);
                    if (!Email.Send(newVoice)) Convert.Rollback(newVoice);
                    if (Voice.Update(newVoice) == 0) Convert.Rollback(newVoice);
                });
            }
            catch (Exception ex)
            {
                exceptions.Enqueue(ex);
            }

            if (exceptions.Count > 0) throw new AggregateException(exceptions);
        }
    }
}