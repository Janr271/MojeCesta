

namespace MojeCesta.Models
{
    class Route_sub_agency : IConstructor
    {
        public string Route_id { get; set; } 
        public int Route_licence_number { get; set; } 
        public int Sub_agency_id { get; set; }
        public string Sub_agency_name { get; set; }

        public void Consturctor(string[] radek)
        {
            Route_id = radek[0];
            if(radek[1] != "")
            {
                Route_licence_number = int.Parse(radek[1]);
            }
            Sub_agency_id = int.Parse(radek[2]);
            Sub_agency_name = radek[3];
        }
    }
}
