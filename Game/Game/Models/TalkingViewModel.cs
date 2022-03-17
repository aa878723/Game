namespace Game.Models
{
    public class TalkingViewModel
    {
        public string RoomName { get; set; }
        public string UserAccount { get; set; }
        public IEnumerable<Talking> Messages { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
