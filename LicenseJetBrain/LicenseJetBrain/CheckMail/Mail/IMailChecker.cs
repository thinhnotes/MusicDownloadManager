using System.Collections.Generic;

namespace LicenseJetBrain.Mail
{
    public interface IMailChecker
    {
        MailMessage GetMessage(ref MailMessage mailMessage);
        IEnumerable<MailMessage> GetAllMessages();
        IEnumerable<MailMessage> GetAllEmailLink();
    }
}