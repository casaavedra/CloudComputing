using Isis4426.Proyecto1.ConversorBatch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isis4426.Proyecto1.ConversorBatch.Interfaces
{
    interface IVoiceCrud
    {
        List<Voice> PendingConvert();
        int Update(Voice voice);
    }
}
