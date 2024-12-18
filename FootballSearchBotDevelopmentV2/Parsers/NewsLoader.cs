using System;
using Fizzler;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System.Timers;
using System.IO;
using System.Net;
using System.Windows;
using System.Timers;

namespace Program
{
    class NewsLoader
    {
        static readonly HttpClient client = new HttpClient();

        private static System.Timers.Timer aTimer;
        //public static DateTime currentDate = DateTime.Now;
        //static public PageInfo lastNew = new PageInfo();
        static public string lastNew;

        static public int count = 0;
        //public static List<PageInfo> articlesListSpR = new List<PageInfo>();
        public static ISet<PageInfo> articlesListSpR = new HashSet<PageInfo>();
        public static List<PageInfo> articles = new List<PageInfo>();
        //public static DateTime lastParseDate = new DateTime();
        public static TimeSpan lastParseTime;
        public static DateTime lPT;

        static async public Task MainPr()
        {
            #region два цикла
            
            //client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36");
            var data = Convert.ToString(await LoadPage("https://www.sports.ru/football/news/"));
            Console.WriteLine("Начался парсинг новостей. Время: " + DateTime.Now);
            if (data != null)
            {
                await Task.Run(() => ParseNewsPage(data));
            }
            foreach (var art in articlesListSpR)
            {
                art.Data = Convert.ToString(await LoadPage(art.Link));
            }
            foreach (var dt in articlesListSpR)
            {
                await Task.Run(() => ParseNews(dt));
            }
            Console.WriteLine("Парсинг новостей кончился. Время: " + DateTime.Now);
            var s = articlesListSpR.ToList<PageInfo>();
            await Task.Run(() => Program.GetNews(s, /*DateTime.MinValue.TimeOfDay*/lastParseTime));
            if(articlesListSpR.Count > 0)
            {
                
                lastParseTime = s[0].Time.TimeOfDay;
            }
            
            Console.WriteLine(lastParseTime.ToString());
            #endregion
            #region один цикл
            //client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36");
            //var data = Convert.ToString(await LoadPage("https://www.sports.ru/football/news/"));
            //if (data != null)
            //{
            //    await Task.Run(() => ParseNewsPage(data));
            //}

            ////List<Task> t = new List<Task>();

            //PageInfo inf = new PageInfo();


            //foreach (var art in articlesListSpR)
            //{

            //    var lpTask = LoadPage(art.Link);

            //    lpTask.Wait();

            //    inf.Data = lpTask.Result;


            //    var task = ParseNews(inf);
            //    t.Add(task);
            //}

            ////var results = Task.WhenAll(t);
            ////Console.WriteLine("Цикл кончился");
            ////Program.GetNews(articlesListSpR);
            //if(articlesListSpR.Count != null)
            //{
            //    await Task.Run(() => Program.GetNews(articlesListSpR));
            //}
            //else
            //{
            //    Console.WriteLine("Список пуст");
            //}
            //foreach (var item in articlesListSpR)
            //{
            //    Console.WriteLine(item.Name);
            //}
            //lastNew = articlesListSpR[0].Data; 
            #endregion

            articlesListSpR.Clear();
            SetTimer();
        }

        static public async void LoadNs(object sender, ElapsedEventArgs e)
        {
            articlesListSpR.Clear();

            Console.WriteLine("Сработал таймер, начался повторный парсинг. Время: " + DateTime.Now);


            //client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36");
            var data = Convert.ToString(await LoadPage("https://www.sports.ru/football/news/"));
            if (data != null)
            {
                await Task.Run(() => ParseNewsPage(data));
            }
            foreach (var art in articlesListSpR)
            {
                art.Data = Convert.ToString(await LoadPage(art.Link));
            }
            foreach (var dt in articlesListSpR)
            {
                
                await Task.Run(() => ParseNews(dt));
            }
            Console.WriteLine("Парсинг кончился, запустился новый таймер. Время: " + DateTime.Now);
            var s = articlesListSpR.ToList<PageInfo>();
            await Task.Run(() => Program.GetNews(s, lastParseTime));
            if (articlesListSpR.Count > 0)
            {
                lastParseTime = s[0].Time.TimeOfDay;
            }
            articlesListSpR.Clear();
            Console.WriteLine(lastParseTime.ToString());

            #region один цикл
            //client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36");
            //var data = Convert.ToString(await LoadPage("https://www.sports.ru/football/news/"));
            //if (data != null)
            //{
            //    await Task.Run(() => ParseNewsPage(data));
            //}

            //List<Task> t = new List<Task>();

            //PageInfo inf = new PageInfo();


            //foreach (var art in articlesListSpR)
            //{

            //    var lpTask = LoadPage(art.Link);
            //    //lpTask.Wait();
            //    if (lpTask.Result == lastNew)
            //    {
            //        Console.WriteLine("Далее новости повторяются");
            //        break;
            //    }

            //    inf.Data = lpTask.Result;


            //    var task = ParseNews(inf);
            //    t.Add(task);

            //}

            //var results = Task.WhenAll(t);
            ////Console.WriteLine("Цикл кончился");
            ////Program.GetNews(articlesListSpR);
            //if (articlesListSpR.Count != null)
            //{
            //    await Task.Run(() => Program.GetNews(articlesListSpR));
            //}
            //else
            //{
            //    Console.WriteLine("Список пуст");
            //}
            ////foreach (var item in articlesListSpR)
            ////{
            ////    Console.WriteLine(item.Name);
            ////}
            //lastNew = articlesListSpR[0].Data;
            #endregion
        }
        private static async void SetTimer()
        {
            aTimer = new System.Timers.Timer(250000);
            aTimer.Elapsed += LoadNs;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        static public async Task<string> LoadPage(string url)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                //Console.WriteLine(response.StatusCode);
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                //Console.WriteLine("\nException Caught!");
                //Console.WriteLine("Message :{0} ", e.Message);
                return "";
            }
        }
        static async public void ParseNewsPage(string data)
        {
            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(data);

            foreach (var item in html.DocumentNode.QuerySelectorAll(".short-news"))
            {
                foreach (var nws in item.QuerySelectorAll("p"))
                {
                    HtmlNode txt = nws.QuerySelector(".short-text");
                    HtmlNode time = nws.QuerySelector(".time");
                    HtmlNode userNews = nws.QuerySelector(".user_news");
                    
                    PageInfo attcl = new PageInfo();

                    attcl.Time = Convert.ToDateTime(time.InnerText);
                    if (userNews != null)
                    {
                        break;
                    }
                    foreach (var ttt in txt.Attributes)
                    {
                        
                        if (ttt.Name == "href")
                        {
                            attcl.Link = "https://www.sports.ru" + ttt.Value;
                        }
                        if (ttt.Name == "title")
                        {
                            attcl.Name = ttt.Value;
                        }
                        
                    }
                    //if(attcl.Link == lastNew)
                    //{
                    //    return;
                    //}
                    articlesListSpR.Add(attcl);
                }
            }

        }
        static async public Task ParseNews(PageInfo pgInf)
        {
            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(pgInf.Data);

            foreach (var item in html.DocumentNode.QuerySelectorAll(".news-item__tags-line"))
            {
                foreach (var a in item.QuerySelectorAll("a"))
                {
                    pgInf.Tag.Add(a.InnerText.ToString());
                }
            }
        }
    }
}