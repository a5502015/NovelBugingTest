using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YahooStockBug
{
    class Stock
    {
        private string url;
        public Stock(string link)
        {
            //string rule2 = @"(https?:\/\/[\w-\.]+(:\d+)?(\/[~\w\/\.]*)?(\?\S*)?(#\S*)?)";
            string rule = @"^(?:([A-Za-z]+):)?(\/{0,3})([0-9.\-A-Za-z]+)(?::(\d+))?(?:\/([^?#]*))?(?:\?([^#]*))?(?:#(.*))?$";
            if (new Regex(rule).IsMatch(link))
            {
                url = link;
            }
            else
            {
                url = @"https://tw.stock.yahoo.com/us/worldidx.php";
            }
        }
        
        public async Task<byte[]> GetUrlResponAsync()
        {
            string html = "";
            using (HttpClient client = new HttpClient()) //抄來的 爽
            {
                // Call asynchronous network methods in a try/catch block to handle exceptions
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    Console.WriteLine(response.StatusCode);
                    string responseBody = await response.Content.ReadAsStringAsync();

                    byte[] respnByte = await response.Content.ReadAsByteArrayAsync();
                    // Above three lines can be replaced with new helper method below
                    // string responseBody = await client.GetStringAsync(uri);
                    html = responseBody;
                    
                    return respnByte;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }

            return null;
        }
        public string analysisHtml(string html, string rule)
        {
            string ans = "";
            HtmlDocument doc = new HtmlDocument();
            //doc.Load("./page.html", Encoding.Default);
            doc.LoadHtml(html);

            //HtmlWeb webClient = new HtmlWeb();
            //HtmlDocument doc = webClient.Load(url);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(rule);

            foreach (HtmlNode node in nodes)
            {
                //Console.WriteLine(node.InnerText.Trim());
                ans += node.InnerText;
                //ans += "---------------------------\n";


                //foreach (HtmlNode chNode in node.ChildNodes)
                //{
                //    Console.Write(chNode.InnerText);
                //    ans += chNode.InnerHtml;
                //}
                //Console.Write("---------------------------");
            }

            return ans;
        }
        public string getUrl(string html, string rule)
        {
            string ans = "";
            HtmlDocument doc = new HtmlDocument();
            
            doc.LoadHtml(html);

            
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(rule);

            foreach (HtmlNode node in nodes)
            {
                
                //Console.WriteLine(node.InnerText + " " + node.Attributes["href"].Value);
                ans += node.InnerText + "|" + node.Attributes["href"].Value + "\n";
                
            }

            return ans;
        }

    }
}
