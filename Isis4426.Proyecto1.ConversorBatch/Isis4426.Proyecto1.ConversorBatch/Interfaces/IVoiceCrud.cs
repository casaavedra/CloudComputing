using Isis4426.Proyecto1.ConversorBatch.Models;
using System.Collections.Generic;

namespace Isis4426.Proyecto1.ConversorBatch.Interfaces
{
    interface IVoiceCrud
    {
        List<Voice> PendingConvert();
        int Update(Voice voice);
    }
}
