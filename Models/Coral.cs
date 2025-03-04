using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Reeftrack.Models;

namespace ReefTrack.Models
{
    public class Coral
    {
        [Key]
        public int Id { get; set; }//PK

        [Required]
        public string? CommonName { get; set; } //Namn

        public string? LatinName { get; set; } //latinskt namn

        [Required]
        public string? Species { get; set; } //LPS, SPS, mjukkorall

        [Required]
        public int Quantity { get; set; } //Antal

        [Required]
        public DateTime AddedDate { get; set; } //Datum när korallen lades till

        // Relationer
        [Required]
        public int AquariumId { get; set; }

        [ForeignKey("AquariumId")]
        public Aquarium? Aquarium { get; set; }
    }
}
