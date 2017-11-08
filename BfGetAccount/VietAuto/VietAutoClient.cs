using System.Collections.Generic;
using System.Linq;
using Core;
using HtmlAgilityPack;
using Utility.Encript;
using Utility.Utility;
using WebRequestHelper;

namespace VietAuto
{
    public class VietAutoClient : TWebRequest, IAccountClient, IUserClient
    {
        public string GetAcccount(int i)
        {
            var url = string.Format(ConsValue.MemberUrl, i);
            var content = Get(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(content);
            var nodeUserName = htmlDocument.DocumentNode.SelectSingleNode("//span[@class='member_username']");
            if (nodeUserName == null) return null;
            return new {Name = nodeUserName.InnerText.Trim(), ReferKey = GetKeyId(url)}.ToJsonString();
        }

        public IEnumerable<string> GetAllAcccount(int i = 0)
        {
            for (var j = 0; j < i + 1000; j++)
            {
                yield return GetAcccount(j);
            }
        }

        public int Login(string user, string pass)
        {
            var url = ConsValue.LoginUrl;
            var data = string.Format("do=login&vb_login_username={0}&vb_login_md5password={1}", user, pass.GetMd5());
            Post(url, data);
            if (CookieCollection["viebb_sessionhash"] != null)
                return 0;
            return -1;
        }

        public string GetKeyId(string url)
        {
            if(url.Last()=='/')
                url = url.Remove(url.Length - 1, 1);
            return url.Split('/').Last();
        }
    }
}