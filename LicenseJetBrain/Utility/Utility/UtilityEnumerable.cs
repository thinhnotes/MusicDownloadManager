using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Utility.Utility
{
    public static class UtilityEnumerable
    {
        public static string ToJsonString<T>(this IEnumerable<T> enumrable)
        {
            var javaScriptSerializer = new JavaScriptSerializer();
            return javaScriptSerializer.Serialize(enumrable);
        }
    }
}