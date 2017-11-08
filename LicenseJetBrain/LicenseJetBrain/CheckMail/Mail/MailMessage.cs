namespace LicenseJetBrain.Mail
{
    public class MailMessage
    {
        public string Title { get; set; }
        public MailBody BodyMessage { get; set; }
        public string Link { get; set; }

        public string TimeRecieve { get; set; }
        public string MailTo { get; set; }
        public string MailFrom { get; set; }

    }
}