using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Isis4426.Proyecto1.ConverBatch.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ConvertirWavSecuencial()
        {
            ConversorBatch.BatchConverter dll = new ConversorBatch.BatchConverter();
            dll.StartSecuencialConvertion();
        }

        [TestMethod]
        public void ConvertirWavParalelo()
        {
            ConversorBatch.BatchConverter dll = new ConversorBatch.BatchConverter();
            dll.StartParallelConvertion();
        }
    }
}
