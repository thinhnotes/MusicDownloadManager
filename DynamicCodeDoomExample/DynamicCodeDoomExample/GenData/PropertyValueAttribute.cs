using System;

namespace DynamicCodeDoomExample
{
    public class PropertyValueAttribute : Attribute
    {
        public PropertyValueAttribute(int minValue)
        {
            MinValue = minValue;
            TypeProperty = PropertyType.Int;
        }

        public PropertyValueAttribute(string prefixValue, int minValue = -1)
        {
            PrefixValue = prefixValue;
            TypeProperty = PropertyType.String;
            if (minValue != -1)
            {
                MinValue = minValue;
                IsAscending = true;
            }
        }

        public PropertyValueAttribute(string[] listStrings)
        {
            ListRandomValue = listStrings;
            TypeProperty = PropertyType.RandomString;
        }

        public PropertyValueAttribute(int minvalue, int maxValue)
        {
            MinValue = minvalue;
            MaxValue = maxValue;
            TypeProperty = PropertyType.RandomInt;
        }

        public PropertyValueAttribute(PropertyType propertyType, string fileName = "names.txt")
        {
            TypeProperty = propertyType;
            FileName = fileName;
        }

        public PropertyType TypeProperty { get; set; }
        public string[] ListRandomValue { get; set; }
        public string PrefixValue { get; set; }
        public bool IsAscending { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public string FileName { get; set; }
    }
}