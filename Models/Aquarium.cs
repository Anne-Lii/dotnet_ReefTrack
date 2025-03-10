using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using ReefTrack.Models;

namespace Reeftrack.Models
{
    public class Aquarium
    {
        //Properties
        [Key]
        public int Id { get; set; }//unikt id PK

        [Required(ErrorMessage = "Namn på akvariet är obligatoriskt.")]
        [StringLength(100, ErrorMessage = "Namn får inte vara längre än 100 tecken.")]
        [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Namn kan inte vara tomt eller innehålla bara mellanslag.")]

        [Display(Name = "Namn")]
        public string? Name { get; set; }//Namn på akvaiet

        [Required(ErrorMessage = "Storlek i liter måste anges.")]
        [Range(1, int.MaxValue, ErrorMessage = "Storleken måste vara minst 1 liter.")]
        [Display(Name = "Storlek")]
        public int Size { get; set; }//volym i liter

        [Required(ErrorMessage = "Sötvatten eller saltvatten måste anges.")]
        [StringLength(50, ErrorMessage = "Typ får inte vara längre än 50 tecken.")]
        [Display(Name = "Typ")]
        public string? Type { get; set; } //"Saltvatten" eller "Sötvatten"

        [Required(ErrorMessage = "Startdatum måste anges.")]
        [DataType(DataType.Date)][Display(Name = "Startdatum")]
        public DateTime StartDate { get; set; } //uppstartsdatum

        public string? ImageName {get; set;}//filnamn som lagras i databasen

        [NotMapped]//lagras ej i databasen
        [Display(Name = "Bild")]
        public IFormFile? ImageFile { get; set; }


        //Relation till användaren
        [Required(ErrorMessage = "En användare måste vara kopplad till akvariet.")]
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual IdentityUser? User { get; set; }//FK

        //Navigeringsproperties för Fiskar och Koraller
        public ICollection<Fish> Fishes { get; set; } = new List<Fish>();
        public ICollection<Coral> Corals { get; set; } = new List<Coral>();

    }
}