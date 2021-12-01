
namespace MojeCesta.Models
{
    class Level : IConstructor
    {
        public string Level_id { get; set; } 
        public int Level_index { get; set; }
        public string Level_name { get; set; }

        public void Consturctor(string[] radek)
        {
            Level_id = radek[0];
            Level_index = int.Parse(radek[1]);
            Level_name = radek[2];
        }
    }
}
