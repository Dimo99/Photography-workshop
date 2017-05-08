using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Photography.Models.Attributes;

namespace Photography.Models
{
    public class Photographer
    {
        private ICollection<Lens> lenses;
        private ICollection<Accessory> accessories;
        private ICollection<Workshop> workshopsParticipating;
        private ICollection<Workshop> workshopsTrainer;
        public Photographer()
        {
            this.lenses = new List<Lens>();
            this.accessories = new List<Accessory>();
            this.workshopsParticipating = new List<Workshop>();
            this.workshopsTrainer = new List<Workshop>();
        }
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required,MinLength(2),MaxLength(50)]
        public string LastName { get; set; }

        [PhotographerPhone]
        public string Phone { get; set; }
        [Required]
        public Camera PrimaryCamera { get; set; }

        public Camera SecondaryCamera { get; set; }

        public virtual ICollection<Accessory> Accessories
        {
            get { return accessories; }
            set { accessories = value; }
        }

        public virtual ICollection<Lens> Lenses
        {
            get { return lenses; }
            set { lenses = value; }
        }
        [InverseProperty("Participants")]
        public virtual ICollection<Workshop> WorkshopsParticipating
        {
            get { return workshopsParticipating; }
            set { workshopsParticipating = value; }
        }
        [InverseProperty("Trainer")]
        public virtual ICollection<Workshop> WorkshopsTrainer
        {
            get { return workshopsTrainer; }
            set { workshopsTrainer = value; }
        }
    }
}