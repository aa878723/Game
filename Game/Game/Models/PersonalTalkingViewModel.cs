namespace Game.Models
{
    public class PersonalTalkingViewModel
    {
        public string UserAccount { get; set; }
        public string FriendAccount { get; set; }
        public List<PersonalTalkEntry> PersonalTalkingSelectList { get; set; }
        public List<PersonalTalking> SelectedTalkingMessage { get; set; }
    }

    public class PersonalTalkEntry
    {
        public string FriendAccount { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastSendTime { get; set; }
    }
}
