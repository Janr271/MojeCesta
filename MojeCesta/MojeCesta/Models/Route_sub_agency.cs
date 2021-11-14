

namespace MojeCesta.Models
{
    class Route_sub_agency : IConstructor
    {
        public string Route_id { get; set; } 
        public int Route_licence_number { get; set; } 
        public int Sub_agency_id { get; set; }
        public string Sub_agency_name { get; set; }

        public void Consturctor(string radek)
        {
            string[] hodnoty = radek.Split(',');
            Route_id = hodnoty[0];
            Route_licence_number = int.Parse(hodnoty[1]);
            Sub_agency_id = int.Parse(hodnoty[2]);
            Sub_agency_name = hodnoty[3];
        }
    }
}
