using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

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


        //Relationer
        [Required]
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }

    }
}