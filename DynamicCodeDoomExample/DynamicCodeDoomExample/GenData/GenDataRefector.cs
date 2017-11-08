using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DynamicCodeDoomExample.Utility;

namespace DynamicCodeDoomExample
{
    public class GenDataRefector<TEnity> where TEnity : class
    {
        public int DefaltValueInt = 1;
        public string DefaultValueString = "Is Null";

        private List<PropertyValue> GetListPropertyValue()
        {
            var resuft = new List<PropertyValue>();
            var propertyInfos = typeof (TEnity).GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var typeCode = Type.GetTypeCode(propertyInfo.PropertyType);
                var customAttribute =
                    (PropertyValueAttribute)
                        propertyInfo.GetCustomAttributes(typeof (PropertyValueAttribute)).FirstOrDefault();
                switch (typeCode)
                {
                    case TypeCode.Empty:
                        break;
                    case TypeCode.Object:
                        break;
                    case TypeCode.DBNull:
                        break;
                    case TypeCode.Boolean:
                        break;
                    case TypeCode.Char:
                        break;
                    case TypeCode.SByte:
                        break;
                    case TypeCode.Byte:
                        break;
                    case TypeCode.Int16:

                    case TypeCode.UInt16:
                        break;
                    case TypeCode.Int32:
                        if (customAttribute != null)
                        {
                            if (customAttribute.MaxValue == 0)
                                resuft.Add(new PropertyValue(propertyInfo.Name, customAttribute.MinValue));
                            else
                                resuft.Add(new PropertyValue(propertyInfo.Name, customAttribute.MinValue,
                                    customAttribute.MaxValue));
                        }
                        else
                        {
                            resuft.Add(new PropertyValue(propertyInfo.Name, DefaltValueInt));
                        }
                        break;
                    case TypeCode.UInt32:
                        break;
                    case TypeCode.Int64:
                        break;
                    case TypeCode.UInt64:
                        break;
                    case TypeCode.Single:
                        break;
                    case TypeCode.Double:
                        break;
                    case TypeCode.Decimal:
                        break;
                    case TypeCode.DateTime:
                        break;
                    case TypeCode.String:
                        if (customAttribute != null)
                        {
                            if (customAttribute.TypeProperty == PropertyType.RandomString)
                            {
                                customAttribute.ListRandomValue =
                                    StringHelper.GetFullNameFromFiles(customAttribute.FileName);
                                resuft.Add(new PropertyValue(propertyInfo.Name, customAttribute.ListRandomValue));
                            }
                            else if (customAttribute.MinValue != 0)
                                resuft.Add(new PropertyValue(propertyInfo.Name, customAttribute.PrefixValue,
                                    customAttribute.MinValue));
                            else if (customAttribute.MinValue == 0)
                            {
                                resuft.Add(new PropertyValue(propertyInfo.Name, customAttribute.PrefixValue));
                            }
                        }
                        else
                        {
                            resuft.Add(new PropertyValue(propertyInfo.Name, DefaultValueString));
                        }
                        break;
                }
            }
            return resuft;
        }

        public string GenData(int count)
        {
            var listPropertyValue = GetListPropertyValue();
            var genData = new GenData();
            return genData.GenListDataString(typeof (TEnity).Name, count, listPropertyValue);
        }
    }
}