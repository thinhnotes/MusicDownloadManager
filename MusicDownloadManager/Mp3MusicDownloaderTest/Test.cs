using System.Linq;
using Mp3MusicDownloader;
using NhacCuaTuiDownloader;
using NhacVnDownloader;
using NUnit.Framework;

namespace Mp3MusicDownloaderTest
{
    [TestFixture]
    public class Test
    {
        [TestCase()]
        public void TestMp3Zing()
        {
            var mp3Music = new Mp3Music();
            string url = "https://mp3.zing.vn/bai-hat/Tinh-Yeu-La-Day-Ho-Trung-Dung/ZW8WUE6O.html";
            var musics = mp3Music.GetMusic(url).ToList();
            Assert.IsNotNull(musics);
            Assert.IsTrue(musics.Count > 0);
        }

        [TestCase("https://www.nhaccuatui.com/playlist/bay-ve-thoi-gian-giang-hong-ngoc.dQ7APONh4YAA.html")]
        public void TestNhacCuaTui(string url)
        {
            var nhacCuaTuiCom = new NhacCuaTuiCom();
            var musics = nhacCuaTuiCom.GetMusic(url).ToList();
            Assert.IsNotNull(musics);
            Assert.IsTrue(musics.Count > 0);
        }

        [TestCase("https://nhac.vn/co-the-ban-thich-nghe?st=sopvbd")]
        public void TestNhacVn(string url)
        {
            var nhacCuaTuiCom = new NhacVnClient();
            var musics = nhacCuaTuiCom.GetMusic(url).ToList();
            Assert.IsNotNull(musics);
            Assert.IsTrue(musics.Count > 0);
        }
    }
}
