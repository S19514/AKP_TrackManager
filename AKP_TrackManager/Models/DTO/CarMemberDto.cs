namespace AKP_TrackManager.Models.DTO
{
    public class CarMemberDto:Member
    {
        public int CarId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public decimal EngingeCapacity { get; set; }
        public int EnginePower { get; set; }
        public string RegPlate { get; set; }
    }
}
