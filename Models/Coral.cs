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
        [Display(Name = "Namn")]
        public string? CommonName { get; set; } //Namn

        [Display(Name = "Vetenskapligt namn")]
        public string? LatinName { get; set; } //latinskt namn

        [Required]
        [Display(Name = "Art")]
        public string? Species { get; set; } //LPS, SPS, mjukkorall

        [Required]
        [Display(Name = "Antal")]
        public int Quantity { get; set; } //Antal

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Inköpsdatum")]
        public DateTime AddedDate { get; set; } //Datum när korallen lades till

        public string? ImageName {get; set;}//filnamn som lagras i databasen

        [NotMapped]
        [Display(Name = "Bild")]
        public IFormFile? ImageFile { get; set; }//lagras ej i databasen

        // Relationer
        [Required]
        public int AquariumId { get; set; }

        [ForeignKey("AquariumId")]
        public Aquarium? Aquarium { get; set; }
    }
}
