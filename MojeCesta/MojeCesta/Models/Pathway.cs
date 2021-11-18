using System;

namespace MojeCesta.Models
{
    class Pathway : IConstructor
    {
        public string Pathway_id { get; set; }
        public string From_stop_id { get; set; } 
        public string To_stop_id { get; set; } 
        public Mode Pathway_mode { get; set; }
        public bool Is_bidirectional { get; set; } 
        public TimeSpan Traversal_time { get; set; }
        public string Signposted_as { get; set; }

        public void Consturctor(string radek)
        {
            string[] hodnoty = radek.Split(',');
            Pathway_id = hodnoty[0];
            From_stop_id = hodnoty[1];
            To_stop_id = hodnoty[2];
            Pathway_mode = (Mode)Enum.Parse(typeof(Mode),hodnoty[3]);
            Is_bidirectional = hodnoty[4].Equals("1");
            Traversal_time = new TimeSpan(int.Parse(hodnoty[5])*10);
            Signposted_as = hodnoty[6];
        }

        public enum Mode : ushort
        {
            Walkway = 1,
            Stairs = 2,
            Travelator = 3,
            Ëscalator = 4,
            Elevator = 5,
            FareGate = 6,
            ExitGate = 7
        }
    }
}
