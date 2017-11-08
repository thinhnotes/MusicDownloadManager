using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Utility.Config;

namespace Utility.Helper
{
    public class NameHelper
    {
        public static IEnumerable<string> ListName;
        public static string FileName;

        static NameHelper()
        {
            if (FileName == null)
            {
                FileName = ConfigHelper.GetConfigValue("nameTxt");
            }
            if (ListName == null)
            {
                if (FileName != null && File.Exists(FileName))
                {
                    ListName = File.ReadLines(FileName);
                }
            }
        }

        public static string GetRandomFullName()
        {
            var rd = new Random(DateTime.Now.Millisecond);
            Thread.Sleep(50);
            var next = rd.Next(1, ListName.Count() - 1);
            return ListName.ElementAt(next);
        }

        public static IEnumerable<string> GetRandomFullName(int count)
        {
            for (var i = 0; i < 5; i++)
            {
                yield return GetRandomFullName();
            }
        }

        public static string GetFirstName(string fullName)
        {
            if (fullName == null) return null;
            return fullName.Split(' ').Last();
        }

        public static string GetRandomFirstName()
        {
            return GetFirstName(GetRandomFullName());
        }

        public static IEnumerable<string> GetRandomFirstName(int count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return GetFirstName(GetRandomFullName());
            }
        }

        public static string GetLastName(string fullName)
        {
            if (fullName == null) return null;
            return fullName.Split(' ').First();
        }

        public static string GetRandomLastName()
        {
            return GetLastName(GetRandomFullName());
        }

        public static IEnumerable<string> GetRandomLastName(int count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return GetLastName(GetRandomFullName());
            }
        }

        public static string GenerateUserName(string fullName)
        {
            if (fullName == null)
                return null;
            return GetFirstName(fullName) + GetLastName(fullName);
        } 
    }
}