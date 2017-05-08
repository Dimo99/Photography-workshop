namespace Photography.Models
{ 
    public class Accessory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Photographer Owner { get; set; }
    }

}