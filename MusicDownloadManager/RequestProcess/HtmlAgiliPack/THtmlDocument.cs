using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace RequestProcess.HtmlAgiliPack
{
    public class THtmlDocument
    {
        public static IEnumerable<string> SelectNode(string content, string xpath)
        {
            var document = new HtmlDocument();
            document.LoadHtml(content);
            return document.DocumentNode.SelectNodes(xpath).Select(x => x.InnerText);
        }

        public static IEnumerable<string> SelectNode(string content, string xpath, string attr)
        {
            var document = new HtmlDocument();
            document.LoadHtml(content);
            var htmlNodes = document.DocumentNode.SelectNodes(xpath).Where(x => x.Attributes[attr] != null).ToList();
            return document.DocumentNode.SelectNodes(xpath).Where(x => x.Attributes[attr] != null).Select(x => x.Attributes[attr].Value);
        }

        public static IEnumerable<string> SelectNodeByUrl(string url, string xpath)
        {
            var document = new HtmlDocument();
            document.Load(url);
            return document.DocumentNode.SelectNodes(xpath).Select(x => x.InnerText);
        }

        public static IEnumerable<string> SelectNodeByUrl(string url, string xpath, string attr)
        {
            var document = new HtmlDocument();
            document.Load(url);
            return document.DocumentNode.SelectNodes(xpath).Where(x => x.Attributes[attr] != null).Select(x => x.Attributes[attr].Value);
        }


        public static string SelectSingleNode(string content, string xpath)
        {
            var document = new HtmlDocument();
            document.LoadHtml(content);
            var selectSingleNode = document.DocumentNode.SelectSingleNode(xpath);
            return selectSingleNode == null ? null : selectSingleNode.InnerText;
        }

        public static string SelectSingleNodeByUrl(string url, string xpath)
        {
            var document = new HtmlDocument();
            document.Load(url);
            var selectSingleNode = document.DocumentNode.SelectSingleNode(xpath);
            return selectSingleNode == null ? null : selectSingleNode.InnerText;
        }

        public static string SelectSingleNodeByUrl(string url, string xpath, string attr)
        {
            var document = new HtmlDocument();
            document.Load(url);
            var selectSingleNode = document.DocumentNode.SelectSingleNode(xpath);
            return selectSingleNode == null ? null : selectSingleNode.GetAttributeValue(attr, "");
        }

        public static string SelectSingleNode(string content, string xpath, string attr)
        {
            var document = new HtmlDocument();
            document.LoadHtml(content);
            var selectSingleNode = document.DocumentNode.SelectSingleNode(xpath);
            return selectSingleNode == null ? null : selectSingleNode.GetAttributeValue(attr, "");
        }
    }
}
