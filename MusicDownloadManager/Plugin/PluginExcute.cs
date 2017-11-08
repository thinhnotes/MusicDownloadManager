using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RequestProcess.Attr;
using RequestProcess.Interface;

namespace Plugin
{
    public class PluginExcute
    {
        public static IEnumerable<string> Executec(string urlDownload)
        {
            var match = Regex.Match(urlDownload, "(http|https)://(?<site>[a-zA-Z0-9.]+)/").Groups["site"];
            if(match==null)
                return null;
            var siteDownload = match.Value;
            if(string.IsNullOrEmpty(siteDownload))
                return null;
            string location = ConfigurationManager.AppSettings["location"];
            if (location == null)
                throw new Exception("Not Files");
            if (!Directory.Exists(location))
                throw new Exception("Not Location");
            var allFiles = Directory.GetFiles(location, "*.dll");
            if (!allFiles.Any())
                return null;
            foreach (var file in allFiles)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
                var loadFile = Assembly.LoadFile(path);
                var allType = loadFile.GetTypes().Where(x => x.IsClass);
                foreach (var type in allType)
                {
                    if (type.IsSubclassOf(typeof(AMusicDownloader)))
                    {
                        string site = null;
                        var attributes = type.GetCustomAttributes(typeof(SiteAttribute));
                        var siteAttribute = attributes.FirstOrDefault();
                        var attribute = siteAttribute as SiteAttribute;
                        if (attribute != null)
                        {
                            site = attribute.Site;
                        }
                        if (string.IsNullOrEmpty(site))
                            continue;
                        if (site.Equals(siteDownload, StringComparison.OrdinalIgnoreCase))
                        {
                            AMusicDownloader musicDownloader = Activator.CreateInstance(type) as AMusicDownloader;
                            if (musicDownloader != null)
                            {
                                return musicDownloader.GetMusic(urlDownload);
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
