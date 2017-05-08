using System.ComponentModel.DataAnnotations;

namespace Photography.Models
{
    public class Camera
    {
        public int Id { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model{ get; set; }
        public bool? IsFullFrameOrNot { get; set; }
        [Required,Range(100,int.MaxValue)]
        public int MinIso { get; set; }

        public int? MaxIso { get; set; }

    }
}