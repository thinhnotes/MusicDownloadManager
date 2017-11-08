using System.Configuration;

namespace Utility.Config
{
    public class ConfigHelper
    {
        public static string GetConfigValue(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }
    }
}