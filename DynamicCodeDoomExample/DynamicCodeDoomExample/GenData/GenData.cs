using System;
using System.Collections.Generic;
using System.Threading;

namespace DynamicCodeDoomExample
{
    public class GenData
    {
        public static string GenDataString(string className, string classValue,
            List<KeyValuePair<string, string>> propertyValue)
        {
            var classData = @"
var @VariableNameClass@ = new @ClassName@()
{
@dataProperty@
};
";
            classData = classData.Replace("@ClassName@", className);
            classData = classData.Replace("@VariableNameClass@", classValue);

            var propertyData = "";

            foreach (var property in propertyValue)
            {
                propertyData += string.Format("\t{0} = {1}, \r\n", property.Key, property.Value);
            }
            propertyData = propertyData.Substring(0, propertyData.Length - 2);
            classData = classData.Replace("@dataProperty@", propertyData);

            return classData;
        }

        public string GenListDataString(string className, int count,
            List<KeyValuePair<string, string>> propertyValue)
        {
            string resuft = null;
            for (var i = 0; i < count; i++)
            {
                resuft +=
                    GenDataString(className, char.ToLowerInvariant(className[0]) + className.Substring(1) + (i + 1),
                        propertyValue) +
                    "\r\n";
            }
            return resuft;
        }

        public string GenListDataString(string className, int count,
            List<PropertyValue> propertyValue)
        {
            string resuft = null;
            for (var i = 0; i < count; i++)
            {
                var listPropertyValuePair = new List<KeyValuePair<string, string>>();
                foreach (var propValue in propertyValue)
                {
                    string value = null;
                    var key = propValue.NameProperty;
                    switch (propValue.TypeProperty)
                    {
                        case PropertyType.Int:
                            value = (propValue.MinValue + i).ToString();
                            break;
                        case PropertyType.String:
                            if (propValue.IsAscending)
                                value = string.Format("\"{0}\"", propValue.PrefixValue + (propValue.MinValue + i));
                            else
                                value = string.Format("\"{0}\"", propValue.PrefixValue);
                            break;
                        case PropertyType.RandomString:
                            var random = new Random(DateTime.Now.Millisecond);
                            Thread.Sleep(1);
                            var next = random.Next(0, propValue.ListRandomValue.Length - 1);
                            value = string.Format("\"{0}\"", propValue.ListRandomValue[next]);
                            break;
                        case PropertyType.RandomInt:
                            var randomint = new Random(DateTime.Now.Millisecond);
                            Thread.Sleep(1);
                            var nextrd = randomint.Next(propValue.MinValue, propValue.MaxValue);
                            value = string.Format("\"{0}\"", nextrd);
                            break;
                    }

                    var properytValuePair = new KeyValuePair<string, string>(key, value);
                    listPropertyValuePair.Add(properytValuePair);
                }
                resuft +=
                    GenDataString(className, char.ToLowerInvariant(className[0]) + className.Substring(1) + (i + 1),
                        listPropertyValuePair) +
                    "\r\n";
            }
            return resuft;
        }
    }
}