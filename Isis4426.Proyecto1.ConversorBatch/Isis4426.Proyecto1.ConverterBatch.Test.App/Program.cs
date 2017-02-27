using System;

namespace Isis4426.Proyecto1.ConverterBatch.Test.App
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ConversorBatch.BatchConverter dll = new ConversorBatch.BatchConverter();
                dll.StartConvertion();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }

            Console.ReadLine();
        }
    }
}
