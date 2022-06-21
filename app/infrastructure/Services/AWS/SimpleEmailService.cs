using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Services
{
    public class SimplePlatformEmailService : IPlatformEmailService
    {
        private readonly ILogger<SimplePlatformEmailService> logger;
        private readonly SesCredentials credentials;

        public SimplePlatformEmailService(ILogger<SimplePlatformEmailService> logger, IOptions<SesCredentials> credentials)
        {
            this.logger = logger;
            this.credentials = credentials.Value;
        }
        public Task SendTenantInvitation(Tenant tenant, string toEmail, string invitationUrl)
        {
            if (string.IsNullOrEmpty(credentials.SmtpUsername))
            {
                throw new ConfigurationException("The server hasn't been configured to send emails");
            }

            // This address/ and or domain must be verified with Amazon SES.
            var from = credentials.Email;
            var fromName = "Cherry Recommends";

            // If you're using Amazon SES in a region other than US West (Oregon), 
            // replace email-smtp.us-west-2.amazonaws.com with the Amazon SES SMTP  
            // endpoint in the appropriate AWS Region.
            var host = $"email-smtp.{credentials.Region}.amazonaws.com";

            // The port you will connect to on the Amazon SES SMTP endpoint. We
            // are choosing port 587 because we will use STARTTLS to encrypt
            // the connection.
            int port = 587;

            // The subject line of the email
            var subject = "Invitation to Cherry";

            // The body of the email
            var body =
                "<h1>You're invited to Cherry Recommends!</h1>" +
                $"<p>You've been invited to join {tenant.Name}" +
                $" at <a href='https://app.cherry.au/{tenant.Name}'> Cherry AI </a> </p> ";
            if (invitationUrl != null)
            {
                body += $"<p> <a href={invitationUrl}> Click here </a> to accept the invitation, or copy and paste the link below.</p>" +
                     $"<p>{invitationUrl}</p>";
            }

            // Create and build a new MailMessage object
            var message = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress(from, fromName),
                Subject = subject,
                Body = body
            };
            message.To.Add(new MailAddress(toEmail));

            using var client = new SmtpClient(host, port);
            // Pass SMTP credentials
            client.Credentials =
                new NetworkCredential(credentials.SmtpUsername, credentials.SmtpPassword);

            // Enable SSL encryption
            client.EnableSsl = true;

            try
            {
                client.Send(message);
            }
            catch (System.Exception ex)
            {
                logger.LogError("The email was not sent. Message: {message}", ex.Message);
                throw new DependencyException("Email Service failed", ex);
            }

            return Task.CompletedTask;
        }
    }
}