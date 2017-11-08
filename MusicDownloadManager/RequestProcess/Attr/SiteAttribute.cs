using System;

namespace RequestProcess.Attr
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class SiteAttribute : Attribute
    {
        public SiteAttribute(string site)
        {
            Site = site;
        }

        public string Site { get; set; }
    }
}