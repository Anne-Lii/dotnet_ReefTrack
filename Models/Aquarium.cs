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
        public int Id { get; set; }//unikt id

        [Required]
        [Display(Name = "Namn")]
        public string? Name { get; set; }//Namn på akvaiet

        [Required]
        [Display(Name = "Storlek")]
        public int Size { get; set; }//volym i liter

        [Required]
        [Display(Name = "Typ")]
        public string? Type { get; set; } //"Saltvatten" eller "Sötvatten"

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Uppstartsdatum")]
        public DateTime StartDate { get; set; } //uppstartsdatum

        public string? ImageName {get; set;}//filnamn som lagras i databasen

        [NotMapped]
        [Display(Name = "Bild")]
        public IFormFile? ImageFile { get; set; }//lagras ej i databasen


        //Relation till användaren
        [Required]
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }

        //Navigeringsproperties för Fiskar och Koraller
        public List<Fish> Fishes { get; set; } = new List<Fish>(); 
        public List<Coral> Corals { get; set; } = new List<Coral>();
    }
}