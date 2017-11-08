using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestProcess.Interface
{
    public abstract class AMusicDownloader : TWebRequest
    {
        public virtual IEnumerable<string> GetMusic(string url)
        {
            var key = GetXmlLink(url);
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new Exception("Can not get key");
            }
            return GetMusicString(key);
        }

        protected abstract string GetKey(string url);

        protected abstract string GetXmlLink(string url);

        //public abstract IEnumerable<string> GetXmlLinks();

        protected abstract IEnumerable<string> GetMusicString(string xmlLink);

        protected string GetContent(string url)
        {
            return Get(url);
        }
    }
}
