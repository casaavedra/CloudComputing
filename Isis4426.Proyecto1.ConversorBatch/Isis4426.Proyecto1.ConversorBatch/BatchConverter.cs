using Isis4426.Proyecto1.ConversorBatch.Business;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Timers;

namespace Isis4426.Proyecto1.ConversorBatch
{
    public class BatchConverter
    {
        private Timer batchTimer;

        public BatchConverter()
        {
            Configuration.Load();

            batchTimer = new Timer { Interval = Models.Configuration.Instance.ConvertTimer };
            batchTimer.Elapsed += BatchTimer_Elapsed;
            batchTimer.Start();
        }

        private void BatchTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            batchTimer.Stop();

            try
            {
                ParallelProcess();
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

        private void ParallelProcess()
        {
            var exceptions = new ConcurrentQueue<Exception>();

            try
            {
                Parallel.ForEach(Voice.PendingConvert(), (voice) =>
                {
                    Models.Voice newVoice = Convert.ConvertVoiceToMp3(voice);
                    Voice.Update(newVoice);
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
