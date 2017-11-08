using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3MusicDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new Mp3Music();
            var key = c.GetMusic("http://mp3.zing.vn/nghe-bxh/video-Viet-Nam/IWZ9Z08W.html?w=6&y=2015").ToList();

        }
    }
}
