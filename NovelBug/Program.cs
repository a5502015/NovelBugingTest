using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YahooStockBug;

namespace NovelBug
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string url = @"https://www.ptwxz.com/html/0/706/";
            string page = @"342780.html";
            //string page = @"342793.html";
            int i = 0;
            while (page != "")
            {
                Novel ng = new Novel(url, page);



                string text = await ng.NovelBugging();

                ng.getInnerText(text,i);

                i++;


                Console.WriteLine("==========================");
                page = ng.GetNextPate(text);
                //System.Threading.Thread.Sleep(30000);
            }

            

            Console.WriteLine("END");
            Console.ReadKey(true);
        }

        
    }
}
