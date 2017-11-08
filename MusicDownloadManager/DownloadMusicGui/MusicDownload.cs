using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;
using Plugin;

namespace DownloadMusicGui
{
    public partial class MusicDownload : Form
    {
        private string _fileName = ConfigurationManager.AppSettings["locationDownLoad"];
        private List<Music> _listMusics = new List<Music>();
        private static object _locker = new object();

        public MusicDownload()
        {
            InitializeComponent();
        }

        private void txtUrl_Leave(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox?.Text))
                return;
            if (!textBox.Text.Contains(@"/"))
                return;
            var list = PluginExcute.Executec(textBox.Text).ToList();
            var demoThread =
                new Thread(() => { UpdateString(list); });

            demoThread.Start();
        }

        public void UpdateString(IEnumerable<string> listMusic)
        {
            _listMusics = new List<Music>();
            var thread = new Thread(() =>
            {
                lvListMusic.Invoke((MethodInvoker) delegate
                {
                    foreach (var l in listMusic)
                    {
                        var music = l.DeserializeJsonAs<Music>();
                        _listMusics.Add(music);
                        var item1 = new ListViewItem(new[] {music.Title, music.Artist, music.Type});
                        lvListMusic.Items.Add(item1);
                    }
                    //this.Invoke(new Action(u=>txtListMusic.AppendText(+ " \r\n")),new Object[]{i})
                });
            });
            
            thread.Start();
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            this.btnDownload.Enabled = false;
            this.txtUrl.Enabled = false;
            this.btnChangeFolder.Enabled = false;
            await DownloadFiles(_listMusics);
            MessageBox.Show("Complete!");
            this.btnDownload.Enabled = true;
            this.txtUrl.Enabled = true;
            this.btnChangeFolder.Enabled = true;
        }

        private void btnChangeFolder_Click(object sender, EventArgs e)
        {
            using (var folder = new FolderBrowserDialog())
            {
                if (folder.ShowDialog() == DialogResult.OK)
                {
                    _fileName = folder.SelectedPath;
                }
            }
        }

        public async Task DownloadFiles(List<Music> listMusics)
        {
            if (!Directory.Exists(_fileName))
                Directory.CreateDirectory(_fileName);
            var count = 0;
            var name = this.Text;
            foreach (var music in listMusics)
            {
                await DownloadFilesAsync(music);
                count++;
                this.Text = $"{name} {count}/{listMusics.Count}";
            }
            
        }

        private async Task DownloadFilesAsync(Music music)
        {
            using (var client = new WebClient())
            {
                client.DownloadProgressChanged += (s, e) =>
                {
                    PrBProcess.Value = e.ProgressPercentage;
                };

                string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                Regex r = new Regex($"[{Regex.Escape(regexSearch)}]");

                var fileName = $"{_fileName + "\\"}{r.Replace(music.Title, "")}.{music.Type}";

                music.Link = PreCheckUrl(music.Link);

                if (!File.Exists(fileName))
                {
                    await client.DownloadFileTaskAsync(music.Link, fileName);
                }
            }
        }

        public string PreCheckUrl(string url)
        {
            if (!url.StartsWith("http"))
            {
                if (!url.StartsWith("//"))
                {
                    url = "//" + url;
                }
                url = "http:" + url;
            }
            return url;
        }
    }
}