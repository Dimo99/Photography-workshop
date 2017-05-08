using System;
using System.Collections.Generic;
using Microsoft.Build.Framework;

namespace Photography.Models
{
    public class Workshop
    {
        private ICollection<Photographer> participants;

        public Workshop()
        {
            participants = new List<Photographer>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public decimal PricePerParticipant { get; set; }
        [Required]
        public Photographer Trainer { get; set; }

        public virtual ICollection<Photographer> Participants
        {
            get { return participants; }
            set { participants = value; }
        }
    }
}