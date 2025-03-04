using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Reeftrack.Models;

namespace ReefTrack.Models
{
    public class Fish
    {
        [Key]
        public int Id { get; set; }//PK

        [Required]
        public string? CommonName { get; set; } //namn

        public string? LatinName { get; set; } //latinskt namn

        [Required]
        public string? Species { get; set; } //Art

        [Required]
        public int Quantity { get; set; } //antal

        [Required]
        public DateTime AddedDate { get; set; } //Datum då fisken lades till

        //Relationer
        [Required]
        public int AquariumId { get; set; }

        [ForeignKey("AquariumId")]
        public Aquarium? Aquarium { get; set; }
    }
}
