namespace AKP_TrackManager.Models.DTO
{
    public class AccidentCarMemberDto:Accident
    {
        public int MemberId { get; set; }
        public int CarId { get; set; }
        public string EmailAddress { get; set; }
        public string RegPlate { get; set; }
    }
}
