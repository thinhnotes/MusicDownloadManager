using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using LicenseJetBrain.Mail;
using Utility.Config;
using WebRequestHelper;

namespace LicenseJetBrain
{
    public class YopmailClient : TWebRequest, IMailChecker
    {
        public static string EmailName;
        public const string UrlPrefix = "http://www.yopmail.com/en/";

        static YopmailClient()
        {
            if (EmailName == null)
                EmailName = ConfigHelper.GetConfigValue("emailChecker");
        }

        public MailMessage GetMessage(ref MailMessage mailMessage)
        {
            if (mailMessage.Link != null)
            {
                var bodyMessage = Get(mailMessage.Link);
                mailMessage.BodyMessage = new MailBody(bodyMessage);
                return mailMessage;
            }
            return null;
        }

        public IEnumerable<MailMessage> GetAllMessages()
        {
            return GetAllEmailLink().Select(x => GetMessage(ref x));
        }

        public IEnumerable<MailMessage> GetAllEmailLink()
        {
            var url =
                string.Format(
                    "http://www.yopmail.com/en/inbox.php?login={0}&p=r&d=&ctrl=&scrl=&spam=true&yf=MAGV0AGpjAGD1BGL0BGp1ZN&yp=IZmx0AQHlBGt2ZmVlAQVjBD&yj=RZmDlAmRmZQp1AmtkAwRkZj&v=2.6&r_c=&id=",
                    EmailName);
            var s = Get(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(s);
            var htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//a[starts-with(@href, \"mail.php\")]");

            foreach (var nodeColection in htmlNodeCollection)
            {
                MailMessage mail = new MailMessage
                {
                    MailTo = String.Format("{0}@yopmail.com", EmailName),
                    Link = UrlPrefix + nodeColection.Attributes["href"].Value
                };
                var nodeTitle = nodeColection.SelectSingleNode("./span[@class='lms']");
                if(nodeTitle!=null)
                    mail.Title = nodeTitle.InnerText;
                var nodeTime = nodeColection.SelectSingleNode(".//span[@class='lmh']");
                if (nodeTime != null)
                    mail.TimeRecieve = nodeTime.InnerText;
                var nodeFrom = nodeColection.SelectSingleNode(".//span[@class='lmf']");
                if (nodeFrom != null)
                    mail.MailFrom = nodeFrom.InnerText;
                yield return mail;
            }
        }
    }
}