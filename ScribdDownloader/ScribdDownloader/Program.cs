using System;

namespace ScribdDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            var scribdClient = new ScribdClient();
            var urlDocument = scribdClient.GetUrlDocument("https://www.scribd.com/document/348983774/ng-book2-angular-4-r58-pdf");
        }
    }
}
