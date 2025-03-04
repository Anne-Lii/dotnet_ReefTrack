using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using ReefTrack.Models;

namespace Reeftrack.Models
{
    public class Aquarium
    {
      
        [Key]
        public int Id { get; set; }//unikt id

        [Required]
        public string? Name { get; set; }//Namn på akvaiet

        [Required]
        public int Size { get; set; }//volym i liter

        [Required]
        public string? Type { get; set; } //"Saltvatten" eller "Sötvatten"

        [Required]
        public DateTime StartDate { get; set; } //uppstartsdatum


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