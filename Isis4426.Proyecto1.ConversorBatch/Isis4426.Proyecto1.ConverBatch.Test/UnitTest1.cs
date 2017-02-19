using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Isis4426.Proyecto1.ConverBatch.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ConvertirWav()
        {
            ConversorBatch.BatchConverter dll = new ConversorBatch.BatchConverter();
            dll.StartConvertion();
        }
    }
}
