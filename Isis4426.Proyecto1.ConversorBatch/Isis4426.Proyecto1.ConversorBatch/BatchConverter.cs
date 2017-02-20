using Isis4426.Proyecto1.ConversorBatch.Business;
using System;
using System.Collections.Concurrent;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Isis4426.Proyecto1.ConversorBatch
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                     ConcurrencyMode = ConcurrencyMode.Multiple)]
    [ServiceContract]
    public class BatchConverter
    {
        public BatchConverter()
        {
            Configuration.Load();
        }

        [WebGet(UriTemplate = "/convertir", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        public void StartConvertion()
        {
            SecuencialProcess();
            //ParallelProcess();
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