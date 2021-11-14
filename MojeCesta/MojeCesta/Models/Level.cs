
namespace MojeCesta.Models
{
    class Level : IConstructor
    {
        public string Level_id { get; set; } 
        public int Level_index { get; set; }
        public string Level_name { get; set; }

        public void Consturctor(string radek)
        {
            string[] hodnoty = radek.Split(',');
            Level_id = hodnoty[0];
            Level_index = int.Parse(hodnoty[1]);
            Level_name = hodnoty[2];
        }
    }
}
