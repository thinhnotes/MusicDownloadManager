using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Config;

namespace VietAuto
{
    public class ConsValue
    {
        public const string SiteUrl = "http://vieauto.com/";
        public const string LoginUrl = "http://vieauto.com/vieauto/login.php?do=login";
        public const string MemberUrl = "http://vieauto.com/vieauto/members/{0}";
        public static readonly string DefaultUser = ConfigHelper.GetConfigValue("user");
        public static readonly string DefaultPass = ConfigHelper.GetConfigValue("pass");
    }
}
