using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebToKindle.Helper
{
    public static class Helper
    {
        public static async Task<string> GetWebpage(String URL, bool supportGzip = true)
        {
            System.Threading.Thread.Sleep(new Random().Next(100, 200));
            var request = (HttpWebRequest)HttpWebRequest.Create(URL);
            if (supportGzip)
            {
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            }
            using var response = request.GetResponse();
            using var stream = response.GetResponseStream();
            using var reader = new StreamReader(stream);
            
            return await reader.ReadToEndAsync();
        }
    }
}
