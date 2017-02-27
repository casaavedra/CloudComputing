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

        [WebGet(UriTemplate = "/convertir")]
        [OperationContract]
        public void StartSecuencialConvertion()
        {            
            SecuencialProcess();
            Console.WriteLine("secuencial");            
        }

        [WebGet(UriTemplate = "/convertirParalelo")]
        [OperationContract]
        public void StartParallelConvertion()
        {
            try
            {
                ParallelProcess();
                Console.WriteLine("parallel");
            }
            catch (AggregateException ae)
            {
                foreach (var ex in ae.InnerExceptions)
                {
                    if (ex is ArgumentException)
                        Console.WriteLine(ex.Message);
                    else
                        throw ex;
                }
            }
        }

        private void SecuencialProcess()
        {
            try
            {
                Voice.Process();
            }            
            catch (Exception)
            {
                //Controlled in the Business Layer
            }
        }
        
        private void ParallelProcess()
        {
            var exceptions = new ConcurrentQueue<Exception>();

            try
            {
                Parallel.For(0, Voice.GetCountPendingConvert(), (voice) =>
                {
                    Voice.Process();
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