using System;

namespace BigRowDataEF
{
    public class ColumnDescriptionAttribute : Attribute
    {
        public ColumnDescriptionAttribute(string description)
        {
            Description = description;
        }
        public string Description { get; set; }
    }
}