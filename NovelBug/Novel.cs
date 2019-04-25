using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YahooStockBug;

namespace NovelBug
{
    class Novel
    {
        Stock st;
        char[] rule = { '\n', '\a' };
        public Novel(string url, string page)
        {
            st = new Stock(url + page);
        }

        public async Task<string> NovelBugging()
        {
            string novel = "";
            

            byte[] html = await st.GetUrlResponAsync();


            novel = Encoding.GetEncoding("gb2312").GetString(html);


            using (StreamWriter sw = new StreamWriter("./raw.html", false))
            {
                sw.Write(novel);
            }

            return novel;
        }

        public void getInnerText(string text,int id)
        {
            string ans = st.analysisHtml(text, @"/html/head");
            
           
            ans = ans.Replace(@"&nbsp;", "\n");
           
            

            string[] ansArr = ans.Split(rule, StringSplitOptions.RemoveEmptyEntries);
            bool star = false;
            if (!Directory.Exists("./novel"))
            {
                Directory.CreateDirectory("./novel");
            }
            using (StreamWriter sw = new StreamWriter("./novel/novel_"+ id + ".txt", false))
            {
                foreach (var tmp in ansArr)
                {
                    if (new Regex(@"快捷键").IsMatch(tmp))
                    {
                        //star = false;
                        break;
                    }

                    if ((new Regex(@"^第.+章$").IsMatch(tmp) || star))
                    {
                        star = true;
                        //Console.WriteLine(tmp);


                        sw.WriteLine(tmp);


                    }
                }
            }
        }
        public string GetNextPate(string text)
        {
            string next = st.getUrl(text, @"/html/head/div/a");

            //Console.WriteLine(next);
            string[] nextArr = next.Split(rule, StringSplitOptions.RemoveEmptyEntries);
            foreach (var tmp in nextArr)
            {
                if (new Regex(@"下一").IsMatch(tmp))
                {
                    
                    string nextPate  = tmp.Split('|')[1];
                    Console.WriteLine(nextPate);
                    if (new Regex("index").IsMatch(nextPate))
                        break;

                    return nextPate;
                    //break;
                }
            }
            return "";
        }
    }
}
