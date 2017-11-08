using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace BigRowDataEF
{
    /// <summary>
    ///     Generic Read Files
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class FileReader<TEntity> where TEntity : class
    {
        /// <summary>
        ///     Template Auto Code
        /// </summary>
        private const string TemplateAutoCode = "{0}";

        /// <summary>
        ///     Read From File Stream
        /// </summary>
        /// <param name="fileStream">File Stream</param>
        /// <param name="charSplit"></param>
        public FileReader(FileStream fileStream, char charSplit = ',')
        {
            CharSplit = charSplit;
            DictionaryColumns = new Dictionary<string, int>();
            AllLines = ReadLines(fileStream, Encoding.UTF8).ToArray();
            ReadColumn(AllLines[0]);
        }

        /// <summary>
        ///     Input String Files name
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <param name="charSplit"></param>
        /// <exception cref="System.Exception">Can not find Found</exception>
        public FileReader(string fileName, char charSplit = ',')
            : this(new FileStream(fileName, FileMode.Open), charSplit)
        {
        }

        /// <summary>
        ///     Char Split
        /// </summary>
        public char CharSplit { get; set; }

        /// <summary>
        ///     Dictionary Save Proerty Has Columns
        /// </summary>
        private Dictionary<string, int> DictionaryColumns { get; set; }

        /// <summary>
        ///     All Line string value
        /// </summary>
        private string[] AllLines { get; set; }

        /// <summary>
        ///     Split Row By Custom has "
        /// </summary>
        /// <param name="row">Row Split</param>
        /// <returns>String Array</returns>
        private string[] SplitRow(string row)
        {
            var charArray = row.ToCharArray();
            var resuft = new List<string>();
            var stringBuilder = new StringBuilder();
            var isCanSplit = true;
            for (var i = 0; i < charArray.Length; i++)
            {
                if (charArray[i].Equals('"'))
                {
                    isCanSplit = !isCanSplit;
                }
                if (charArray[i].Equals(CharSplit) && isCanSplit)
                {
                    resuft.Add(stringBuilder.ToString());
                    stringBuilder = new StringBuilder();
                }
                else
                {
                    stringBuilder.Append(charArray[i]);
                }
                if (i == charArray.Length - 1)
                {
                    resuft.Add(stringBuilder.ToString());
                }
            }
            return resuft.Select(x => x.Replace("\"", "")).ToArray();
        }

        /// <summary>
        ///     Convert File To Entity
        /// </summary>
        /// <returns>List Entity</returns>
        /// <exception cref="System.Exception">Can Not Entity Type</exception>
        /// <exception cref="System.Exception">Can not Cast property</exception>
        public List<TEntity> ConvertEntity()
        {
            var rowdata = AllLines.Skip(1).ToArray();
            return ReadFile(rowdata);
        }

        /// <summary>
        ///     Read All line of file
        /// </summary>
        /// <param name="streamProvider">File Stream</param>
        /// <param name="encoding">Encoding</param>
        /// <returns>IEnumerables string</returns>
        private IEnumerable<string> ReadLines(FileStream streamProvider, Encoding encoding)
        {
            using (var reader = new StreamReader(streamProvider, encoding))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        /// <summary>
        ///     Read Columns From Files
        /// </summary>
        /// <param name="columns"></param>
        private void ReadColumn(string columns)
        {
            ReadColumn(SplitRow(columns));
        }

        /// <summary>
        ///     Read Columns From Files
        /// </summary>
        /// <param name="columns"></param>
        private void ReadColumn(string[] columns)
        {
            var propertyInfos = typeof (TEntity).GetProperties();
            for (var i = 0; i < columns.Count(); i++)
            {
                foreach (var propertyInfo in propertyInfos)
                {
                    var customAttributesRequireds = propertyInfo.GetCustomAttributes(typeof (ColumnRequiredAttribute),
                        false);

                    var customAttributesColumns = propertyInfo.GetCustomAttributes(typeof (ColumnDescriptionAttribute),
                        false);
                    if (customAttributesColumns.Length > 0)
                    {
                        var customAttribute = (ColumnDescriptionAttribute) customAttributesColumns.First();
                        if (customAttribute.Description.Equals(columns[i], StringComparison.CurrentCultureIgnoreCase))
                        {
                            DictionaryColumns.Add(propertyInfo.Name, i);
                        }

                        if (customAttributesRequireds.Length > 0)
                        {
                            var requiredAttribute = (ColumnRequiredAttribute) customAttributesRequireds.First();
                            if (requiredAttribute.IsRequied)
                            {
                                if (
                                    !columns.Any(
                                        x => x.Equals(customAttribute.Description, StringComparison.OrdinalIgnoreCase)))
                                {
                                    throw new Exception("File struct!");
                                }
                            }
                        }
                    }
                    else
                    {
                        if (propertyInfo.Name.Equals(columns[i], StringComparison.CurrentCultureIgnoreCase))
                        {
                            DictionaryColumns.Add(propertyInfo.Name, i);
                        }

                        if (customAttributesRequireds.Length > 0)
                        {
                            var requiredAttribute = (ColumnRequiredAttribute) customAttributesRequireds.First();
                            if (requiredAttribute.IsRequied)
                            {
                                if (!columns.Any(x => x.Equals(propertyInfo.Name, StringComparison.OrdinalIgnoreCase)))
                                {
                                    throw new Exception("File struct!");
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Read DataRows From Files
        /// </summary>
        /// <param name="datarows"></param>
        /// <returns></returns>
        private List<TEntity> ReadFile(string[] datarows)
        {
            var resuft = new List<TEntity>();
            foreach (var row in datarows)
            {
                if (Regex.IsMatch(row, "\""))
                {
                }
                var valueColumns = SplitRow(row);
                var entity = Activator.CreateInstance(typeof (TEntity)) as TEntity;
                if (entity == null)
                    throw new Exception("Can not find Type Of Entity");
                for (var j = 0; j < valueColumns.Count(); j++)
                {
                    foreach (var dicColumn in DictionaryColumns)
                    {
                        var prop = entity.GetType()
                            .GetProperty(dicColumn.Key, BindingFlags.Public | BindingFlags.Instance);
                        if (prop != null)
                        {
                            try
                            {
                                var t = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                                var safeValue = (valueColumns[dicColumn.Value] == "")
                                    ? GetDefault(t)
                                    : Convert.ChangeType(valueColumns[dicColumn.Value], t);
                                //var value = Convert.ChangeType(valueColumns[dicColumn.Value], prop.PropertyType);
                                prop.SetValue(entity, safeValue, null);
                            }
                            catch (Exception exception)
                            {
                                throw new Exception(exception.Message);
                            }
                        }
                    }
                }
                resuft.Add(entity);
            }
            return resuft;
        }

        /// <summary>
        ///     Generate a unique code
        /// </summary>
        /// <returns>unique code</returns>
        public static string GenerateAutoCode()
        {
            return string.Format(TemplateAutoCode, Guid.NewGuid().ToString().ToUpper().Replace("-", string.Empty));
        }

        /// <summary>
        ///     Get Default value Of Type
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns></returns>
        public object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}