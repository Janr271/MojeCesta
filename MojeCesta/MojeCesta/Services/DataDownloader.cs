using System;
using System.Net;
using System.IO;
using System.IO.Compression;

namespace MojeCesta.Services
{
    public static class DataDownloader
    {
        public static void Download(string cestaKZipu, string cestaKeSlozce)
        {
            using(WebClient client = new WebClient())
            {
                client.DownloadFile(@"http://data.pid.cz/PID_GTFS.zip", cestaKZipu);
                ZipFile.ExtractToDirectory(cestaKZipu, cestaKeSlozce);
                File.Delete(cestaKZipu);
            }
        }
    }
}
