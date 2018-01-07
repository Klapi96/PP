using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml;
using System.Net;

namespace lab3zad14
{
    class Program
    {
        public static async Task<XmlDocument> Zadanie3(string address)
        {
            WebClient webClient = new WebClient();
            string xml = await webClient.DownloadStringTaskAsync(new Uri(address));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc;
        }

        static void Main(string[] args)
        {
            Task<XmlDocument> t = Zadanie3("http://www.feedforall.com/sample.xml%22");
            t.Wait();
            var result = t.GetAwaiter().GetResult();

        }

    }

}
