using System.Threading.Tasks;

namespace SignalBox.Core
{
#nullable enable
    public interface IPlatformEmailService
    {
        Task SendTenantInvitation(Tenant tenant, string toEmail, string? invitationUrl);
    }
}