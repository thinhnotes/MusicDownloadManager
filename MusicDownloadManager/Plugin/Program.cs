using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin
{
    class Program
    {
        static void Main(string[] args)
        {
            var enumerable =
                PluginExcute.Executec(
                    "http://nhac.vui.vn/nghe-bang-xep-hang-bai-hat-viet-nam-mr1y2015w9.html").ToList();
        }
    }
}
