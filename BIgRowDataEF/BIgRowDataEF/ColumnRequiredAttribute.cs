using System;

namespace BigRowDataEF
{
    public class ColumnRequiredAttribute : Attribute
    {
        public bool IsRequied { get; set; }
    }
}