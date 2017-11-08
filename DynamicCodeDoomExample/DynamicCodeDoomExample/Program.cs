using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicCodeDoomExample.Utility;

namespace DynamicCodeDoomExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //Sample sample = new Sample();
            //sample.AddFields();
            //sample.AddProperties("MaNV", typeof(string));
            //sample.AddMethod();
            //sample.AddConstructor();
            //sample.AddEntryPoint();
            //sample.GenerateCSharpCode("SampleCode.cs");
            var keyValuePairs = new List<KeyValuePair<string, string>>();
            var c = new KeyValuePair<string, string>("Id", "1");
            keyValuePairs.Add(c);
            var genData = new GenData();
            //var genDataString = genData.GenListDataString("SinhVien", 10, keyValuePairs);

            PropertyValue propertyId = new PropertyValue("Id", 1);
            PropertyValue propertyName = new PropertyValue("Ten", StringHelper.GetFullNameFromFiles());
            PropertyValue propertyLop = new PropertyValue("Lop", "CTK", 34);

            //var dataString = genData.GenListDataString("SinhVien", 10, propertyId, propertyName, propertyLop);

            //Console.WriteLine(dataString);

            var genDataRefector = new GenDataRefector<SinhVien>();
            var data = genDataRefector.GenData(10);
            Console.WriteLine(data);
            Console.ReadKey();

        }
    }
}
