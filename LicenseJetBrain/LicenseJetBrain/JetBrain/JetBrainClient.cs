using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using HtmlAgilityPack;
using Utility.Helper;
using Utility.Utility;
using WebRequestHelper;

namespace LicenseJetBrain.JetBrain
{
    public class JetBrainClient : TWebRequest
    {
        private static int CountMailChecked = 0;
        public int CountMailCheck { get; set; }

        public bool VerifyEmail(string firstName, string lastName)
        {
            CountMailChecked = 0;
            if (CountMailCheck < 1)
                CountMailCheck = 5;
            string emailAddress = GetRandomEmail();
            var url = "https://www.jetbrains.com/estore/students/apply";
            var redirectLink = "https://www.jetbrains.com/estore/students/registrationCompleted";

            var data =
                string.Format(
                    "registrationMethod=EDU_EMAIL&studentType=STUDENT&firstNameHint=First Name&lastNameHint=Last Name&firstName={0}&lastName={1}&email=&cardholderName=&cardNumber=&eduEmail={2}",
                    HttpUtility.UrlEncode(firstName.UppercaseFist()), HttpUtility.UrlEncode(lastName.UppercaseFist()),
                    HttpUtility.UrlEncode(emailAddress));
            Post(url, data);
            if (Location == redirectLink)
                return true;
            return false;
        }

        public string GetAcctiveLinkEmail(string fullname)
        {
            Thread.Sleep(1000);
            var yopmailClient = new YopmailClient();
            var mailMessage = yopmailClient.GetAllEmailLink().First();
            yopmailClient.GetMessage(ref mailMessage);
            var contentMail = mailMessage.BodyMessage.ContentMail;
            var firstName = string.Format("Dear {0},", NameHelper.GetFirstName(fullname));
            if (contentMail[4].Equals(firstName, StringComparison.OrdinalIgnoreCase))
            {
                return mailMessage.BodyMessage.NavigationLink.FirstOrDefault(x => x.Contains("account.jetbrains.com"));
            }
            CountMailChecked++;
            if (CountMailChecked == CountMailCheck)
                return null;
            return GetAcctiveLinkEmail(fullname);
        }

        public bool CreateAccount()
        {
            var fullName = NameHelper.GetRandomFullName();
            Console.WriteLine(fullName);
            return CreateAccount(fullName);
        }

        public bool CreateAccount(string fullName)
        {
            var firstName = NameHelper.GetFirstName(fullName);
            var lastName = NameHelper.GetLastName(fullName);
            var userName = GenerateUserName(firstName, lastName);
            Console.WriteLine(userName);
            if (VerifyEmail(firstName, lastName))
            {
                var url = GetAcctiveLinkEmail(fullName);
                var key = url.Replace("https://account.jetbrains.com/order/assets/", "");
                return CreateAccount(key, userName, userName);
            }
            return false;
        }

        //public bool CreateAccount(string url)
        //{
        //    var userName = GenerateUserName();
        //    return CreateAccount(url, userName, userName);
        //}

        public bool CreateAccount(string key, string userName, string passWord)
        {
            Get(string.Format("https://account.jetbrains.com/licenseAgreements/order/{0}", key));
            var cookie = CookieCollection["_st"];
            if (cookie != null)
            {
                var stKey = cookie.Value;
                return CreateAccount(key, userName, passWord, stKey);
            }
            return false;
        }

        public bool CreateAccount(string key, string userName, string passWord, string stKey)
        {
            Post(string.Format("https://account.jetbrains.com/licenseAgreements/accept_on_order/{0}?_st={1}", key,
                stKey));
            var register = Get(string.Format("https://account.jetbrains.com/order/assets/{0}", key));
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(register);
            var selectSingleNode = htmlDocument.DocumentNode.SelectSingleNode("//form");
            if (selectSingleNode == null) return false;
            var actionValue = selectSingleNode.Attributes["action"].Value;
            var actionUrl = string.Format("https://account.jetbrains.com{0}", actionValue);
            while (!Register(actionUrl, userName, passWord))
            {
                userName += RandomHelper.GetRandomStringFrom("zxcvbnmasdfghjklqwertyuiop12334567890", 1);
            }
            return true;
        }


        public bool Register(string url, string userName, string passWord)
        {
            url = url.HtmlDecode();
            var dataRegister = string.Format("username={0}&password={1}&pass2={1}", userName, passWord);
            AutoRedirect = false;
            Location = null;
            Post(url, dataRegister);
            if (Location == null) return false;
            Console.WriteLine("{0}:{1}", userName, passWord);
            return true;
        }

        public string GenerateUserName(string firstName, string lastName)
        {
            return string.Format("{0}{1}", firstName.RemoveVnChar(), lastName.RemoveVnChar());
        }

        public static string GetRandomEmail()
        {
            return string.Format("how2120535+{0}@maricopa.edu",
                RandomHelper.GetRandomStringFrom("zxcvbnmasdfghjklqwertyuiop", 5));
        }
    }
}