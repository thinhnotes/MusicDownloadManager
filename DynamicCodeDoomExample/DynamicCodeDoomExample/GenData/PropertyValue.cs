using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicCodeDoomExample
{
    public class PropertyValue
    {
        public PropertyValue(string nameProperty, int minValue)
        {
            NameProperty = nameProperty;
            MinValue = minValue;
            TypeProperty = PropertyType.Int;
        }

        public PropertyValue(string nameProperty, string prefixValue, int minValue = -1)
        {
            NameProperty = nameProperty;
            PrefixValue = prefixValue;
            TypeProperty = PropertyType.String;
            if (minValue != -1)
            {
                MinValue = minValue;
                IsAscending = true;
            }
        }

        public PropertyValue(string nameProperty, string[] listStrings)
        {
            NameProperty = nameProperty;
            ListRandomValue = listStrings;
            TypeProperty = PropertyType.RandomString;
        }

        public PropertyValue(string nameProperty, int minvalue, int maxValue)
        {
            NameProperty = nameProperty;
            MinValue = minvalue;
            MaxValue = maxValue;
            TypeProperty = PropertyType.RandomInt;
        } 

        public string NameProperty { get; set; }
        public PropertyType TypeProperty { get; set; }
        public string[] ListRandomValue { get; set; }
        public string PrefixValue { get; set; }
        public bool IsAscending { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
    }
}
