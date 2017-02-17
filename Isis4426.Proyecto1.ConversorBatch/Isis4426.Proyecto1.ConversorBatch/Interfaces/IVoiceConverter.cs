using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isis4426.Proyecto1.ConversorBatch.Interfaces
{
    interface IVoiceConverter
    {
        void ConvertVoiceToMp3(ref Models.Voice voice);
    }
}
