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
        [Display(Name = "Namn")]
        public string? CommonName { get; set; } //namn

        [Display(Name = "Vetenskapligt namn")]
        public string? LatinName { get; set; } //latinskt namn

        [Required]
        [Display(Name = "Art")]
        public string? Species { get; set; } //Art

        [Required]
        [Display(Name = "Antal")]
        public int Quantity { get; set; } //antal

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Inköpsdatum")]
        public DateTime AddedDate { get; set; } = DateTime.UtcNow; //Standard: Nuvarande datum

        public string? ImageName {get; set;}//filnamn som lagras i databasen

        [NotMapped]
        [Display(Name = "Bild")]
        public IFormFile? ImageFile { get; set; }//lagras ej i databasen

        //Relationer
        [Required]
        public int AquariumId { get; set; }

        [ForeignKey("AquariumId")]
        public Aquarium? Aquarium { get; set; }
    }
}
