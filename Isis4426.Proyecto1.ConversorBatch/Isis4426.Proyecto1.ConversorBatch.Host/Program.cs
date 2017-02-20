using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Isis4426.Proyecto1.ConversorBatch.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost sh = new ServiceHost(typeof(BatchConverter),
                                             new Uri("http://localhost:6089/"));
            bool openSucceeded = false;
            //TRY OPENINNG, IF FAILS THE HOST WILL BE ABORTED 
            try
            {
                ServiceEndpoint sep = sh.AddServiceEndpoint(typeof(BatchConverter),
                                                            new WebHttpBinding(),
                                                            "");
                sep.Behaviors.Add(new WebHttpBehavior());
                sh.Open();
                openSucceeded = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Service host failed to open {)}",
                                  ex.ToString());
            }
            finally
            {
                if (!openSucceeded)
                {
                    sh.Abort();
                }
            }
            if (sh.State == CommunicationState.Opened)
            {
                Console.WriteLine("The Service is running. Press Enter to stop.");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Server failed to open");
            }
            //TRY CLOSING, IF FAILS THE HOST WILL BE ABORTED 
            bool closeSucceed = false;
            try
            {
                sh.Close();
                closeSucceed = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Service failed to close. Exception {0}",
                                  ex.ToString());
            }
            finally
            {
                if (!closeSucceed)
                {
                    sh.Abort();
                }
            }
        }
    }
}
