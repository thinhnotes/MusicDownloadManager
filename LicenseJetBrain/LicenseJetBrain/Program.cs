using LicenseJetBrain.JetBrain;

namespace LicenseJetBrain
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var yopmailClient = new YopmailClient();
            //var allEmailLink = yopmailClient.GetAllMessages().ToList();
            var jetBrainClient = new JetBrainClient();
            //var verifyEmail = jetBrainClient.VerifyEmail("Hoa", "Long");
            var account = jetBrainClient.CreateAccount();
        }
    }
}