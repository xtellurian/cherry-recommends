namespace SignalBox.Core
{
    public class MembershipNotFoundException : SignalBoxException
    {
        public MembershipNotFoundException(string userId) : base($"User {userId} has no memberships")
        { }
    }
}