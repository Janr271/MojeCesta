using System.Net;

namespace MojeCesta.Services
{
    public static class DataDownloader
    {
        public static void Download(string cestaKZipu)
        {
            using(WebClient client = new WebClient())
            {
                client.DownloadFile(@"http://data.pid.cz/PID_GTFS.zip", cestaKZipu);
            }
        }
    }
}
