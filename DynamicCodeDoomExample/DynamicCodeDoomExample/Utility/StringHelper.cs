using System.IO;

namespace DynamicCodeDoomExample.Utility
{
    public class StringHelper
    {
        public static string[] GetFullNameFromFiles(string file)
        {
            return File.ReadAllLines(file);
        }

        public static string[] GetFullNameFromFiles()
        {
            return GetFullNameFromFiles("names.txt");
        }
    }
}