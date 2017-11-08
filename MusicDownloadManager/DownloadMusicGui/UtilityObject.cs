﻿using System.Web.Script.Serialization;

namespace DownloadMusicGui
{
    public static class UtilityObject
    {
        public static T DeserializeJsonAs<T>(this string obj)
        {
            var javaScriptSerializer = new JavaScriptSerializer();
            return javaScriptSerializer.Deserialize<T>(obj);
        }

        public static string ToJsonString(this object obj)
        {
            var javaScriptSerializer = new JavaScriptSerializer();
            return javaScriptSerializer.Serialize(obj);
        }
    }
}