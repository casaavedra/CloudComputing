using System;
using System.IO;

namespace Isis4426.Proyecto1.ConversorBatch.Models
{
    class Voice
    {
        public int Consecutive { get; set; }
        public FileInfo Origin { get; set; }
        public FileInfo Destiny { get; set; }
        public DateTime ConvertDate { get; set; }
        public Status State { get; set; }
    }
}