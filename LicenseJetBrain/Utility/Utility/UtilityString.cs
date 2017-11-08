using System;
using System.Linq;
using System.Text;
using System.Web;

namespace Utility.Utility
{
    public static class UtilityString
    {
        public static string CombineUrl(this string s, string url)
        {
            var baseUri = new Uri(s);
            return new Uri(baseUri, url).OriginalString;
        }

        //xxx
        public static bool TestWildcard(this string s, string name)
        {
            return false;
        }

        public static string HtmlDecode(this string str)
        {
            return HttpUtility.HtmlDecode(str);
        }

        public static string UrlDecode(this string str)
        {
            return HttpUtility.UrlDecode(str);
        }

        public static string UrlEncode(this string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        public static string SubStringSpaceLengh(this string str, int lenght)
        {
            string[] strings = str.Split(' ');
            string resuft = "";
            foreach (string s in strings)
            {
                if (resuft.Length > lenght)
                    break;
                resuft += s + " ";
            }
            return resuft;
        }

        public static string UppercaseFist(this string s)
        {
            if(string.IsNullOrWhiteSpace(s)) return null;
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        /// <summary>
        ///     xóa dấu tính việt
        /// </summary>
        /// <param name="str">chuỗi cần xóa dấu</param>
        /// <returns>chuỗi đã xóa dấu</returns>
        public static string RemoveVnChar(this string str)
        {
            string engs = "aAeEoOuUiIdDyY";
            string[] vns =
            {
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "đ",
                "Đ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ"
            };
            var sb = new StringBuilder();
            foreach (char ch in str)
            {
                int i;
                for (i = 0; i < vns.Length; i++)
                    if (vns[i].Contains(ch)) break;
                sb.Append(i < vns.Length ? engs[i] : ch);
            }
            return sb.ToString();
        }

        
    }
}