namespace SignalBox.Functions
{
    public class AddMember
    {
        public AddMember()
        { }
        public AddMember(string userId, string email)
        {
            UserId = userId;
            Email = email;
        }
        public string UserId { get; set; }
        public string Email { get; set; }
    }
}