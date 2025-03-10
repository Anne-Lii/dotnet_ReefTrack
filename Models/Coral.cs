using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Reeftrack.Models;

namespace ReefTrack.Models
{
    public class Coral
    {
        [Key]
        public int Id { get; set; }//PK

        [Required(ErrorMessage = "Namn på korallen är obligatoriskt.")]
        [StringLength(100, ErrorMessage = "Namn får inte vara längre än 100 tecken.")]
        [RegularExpression(@"^\S.*\S$", ErrorMessage = "Namn kan inte vara tomt eller bara bestå av mellanslag.")]
        [Display(Name = "Namn")]
        public string? CommonName { get; set; } //Namn

        [StringLength(100, ErrorMessage = "Vetenskapligt namn får inte vara längre än 100 tecken.")]
        [RegularExpression(@"^\S.*\S$", ErrorMessage = "Vetenskapligt namn kan inte vara tomt eller bara bestå av mellanslag.")]
        [Display(Name = "Vetenskapligt namn")]
        public string? LatinName { get; set; } //latinskt namn

        [Required(ErrorMessage = "Art måste anges.")]
        [StringLength(50, ErrorMessage = "Art får inte vara längre än 50 tecken.")]
        [RegularExpression(@"^\S.*\S$", ErrorMessage = "Art kan inte vara tomt eller bara bestå av mellanslag.")]
        [Display(Name = "Art")]
        public string? Species { get; set; } //LPS, SPS, mjukkorall

        [Required(ErrorMessage = "Antal måste anges.")]
        [Range(1, int.MaxValue, ErrorMessage = "Antal måste vara minst 1.")]
        [Display(Name = "Antal")]
        public int Quantity { get; set; } //Antal

        [Required(ErrorMessage = "Inköpsdatum måste anges.")]
        [DataType(DataType.Date)]
        [Display(Name = "Inköpsdatum")]
        public DateTime AddedDate { get; set; } = DateTime.UtcNow; // Standard: Nuvarande datum

        public string? ImageName {get; set;}//filnamn som lagras i databasen

        [NotMapped]
        [Display(Name = "Bild")]
        public IFormFile? ImageFile { get; set; }//lagras ej i databasen

        // Relationer
        [Required(ErrorMessage = "Akvarium-id måste anges.")]
        public int AquariumId { get; set; }

        [ForeignKey("AquariumId")]
        public Aquarium? Aquarium { get; set; }
    }
}
