using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Utility.Utility;

namespace LicenseJetBrain.Mail
{
    public class MailBody
    {
        public string BodyRaw { get; set; }

        public MailBody()
        {
            
        }

        public MailBody(string bodyRaw):this()
        {
            BodyRaw = bodyRaw;
        } 
        public IEnumerable<string> NavigationLink
        {
            get
            {
                if (BodyRaw != null)
                {
                    var htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(BodyRaw);
                    var htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//a");
                    foreach (var htmlNode in htmlNodeCollection)
                    {
                        yield return htmlNode.Attributes["href"].Value;
                    }

                    var matchCollection = Regex.Matches(htmlDocument.DocumentNode.InnerText, "(http|https)://[^\\s]+");
                    foreach (Match match in matchCollection)
                    {
                        yield return match.Value;
                    }
                }
            }
        }

        public List<string> ContentMail
        {
            get
            {
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(BodyRaw);
                var bodyNode = htmlDocument.DocumentNode.SelectSingleNode("//body");
                foreach (HtmlNode comment in bodyNode.SelectNodes("//comment()"))
                {
                    comment.ParentNode.RemoveChild(comment);
                }
                var strings = bodyNode.InnerText.Split(new[] { "\r\n" }, StringSplitOptions.None);
                return strings.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim().HtmlDecode()).ToList();
            }
        }
    }
}
