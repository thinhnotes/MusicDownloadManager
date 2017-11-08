using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhacCuaTuiDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            var nhacCuaTuiCom = new NhacCuaTuiCom();
            var list = nhacCuaTuiCom.GetMusic(
                "http://www.nhaccuatui.com/playlist/top-20-bai-hat-viet-nam-nhaccuatui-tuan-102015-va.fRSurZ232Vge.html").ToList();
        }
    }
}
