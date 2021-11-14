using System;

namespace MojeCesta.Models
{
    class Pathway : IConstructor
    {
        public string Pathway_id { get; set; }
        public string From_stop_id { get; set; } 
        public string To_stop_id { get; set; } 
        public int Pathway_mode { get; set; }
        public bool Is_bidirectional { get; set; } 
        public TimeSpan Traversal_time { get; set; }
        public string Signposted_as { get; set; }

        public void Consturctor(string radek)
        {
            string[] hodnoty = radek.Split(',');
            Pathway_id = hodnoty[0];
            From_stop_id = hodnoty[1];
            To_stop_id = hodnoty[2];
            Pathway_mode = int.Parse(hodnoty[3]);
            Is_bidirectional = bool.Parse(hodnoty[4]);
            Traversal_time = TimeSpan.Parse(hodnoty[5]);
            Signposted_as = hodnoty[6];
        }
    }
}
