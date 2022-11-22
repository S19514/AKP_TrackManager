using System.ComponentModel.DataAnnotations;

namespace AKP_TrackManager.Models.DTO
{
    public class CarMemberDto:Member
    {
        public int CarId { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public decimal EngingeCapacity { get; set; }
        [Required]
        public int EnginePower { get; set; }
        [Required]
        public string RegPlate { get; set; }
    }
}
