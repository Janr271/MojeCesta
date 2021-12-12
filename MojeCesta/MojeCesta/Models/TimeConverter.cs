using System;
using FileHelpers;

namespace MojeCesta.Models
{
    class TimeConverter : ConverterBase
    {
        public override object StringToField(string from)
        {
            int[] cas = Array.ConvertAll(from.Split(':'), int.Parse);
            return new DateTime(1, 1, (cas[0] / 24) + 1, cas[0] % 24, cas[1], cas[2]);
        }
    }
}
