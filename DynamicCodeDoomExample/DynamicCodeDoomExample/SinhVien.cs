using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicCodeDoomExample.Utility;

namespace DynamicCodeDoomExample
{
    class SinhVien
    {
        public static string[] a = StringHelper.GetFullNameFromFiles();
        [PropertyValue(1)]
        public int Id { get; set; }

        [PropertyValue(PropertyType.RandomString)]
        public string Ten { get; set; }
        [PropertyValue(20, 30)]
        public int Tuoi { get; set; }
    }
}
