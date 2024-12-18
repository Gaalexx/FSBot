using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Fizzler;

namespace Program
{
    enum LeaguesEnum
    {
        RPL,
        LaLiga,
        SeriaA,
        BundesLiga,
        FNL,
        Liga1,
        UPL,
        APL
    }
    enum CountriesEnum
    {
        Russia,
        Germany,
        France,
        Spain,
        Italy,
        Ukraine,
        England
    }
    class Parser
    {


        static Program mnPrg = new Program();
        static public List<List<string>> teamsTable = new List<List<string>>();
        static public List<string> dataRow = new List<string>();
        static readonly HttpClient client = new HttpClient();
        static public List<League> leagues = new List<League>();
        static public Dictionary<LeaguesEnum, League> LeaguesDic = new Dictionary<LeaguesEnum, League>();
        static public Dictionary<CountriesEnum, Country> CountriesDic = new Dictionary<CountriesEnum, Country>();
        static public Dictionary<string, string> teamPageLink = new Dictionary<string, string>();

        public async Task prstst()
        {
            await Task.Run(() => Main1());
        }
        public async Task<string> runLoadpg(string url)
        {
            return await Task.Run(() => LoadPage(url));
        }
        public async Task<List<List<string>>> prsPg(string data, List<List<string>> a)
        {
            return await Task.Run(() => ParseTeamPage(data, a));
        }

        static async Task Main1()
        {

            LeaguesDic.Add(LeaguesEnum.RPL, new League("https://www.transfermarkt.world/premier-liga/startseite/wettbewerb/RU1", "Российская Премьер Лига", 0));
            LeaguesDic.Add(LeaguesEnum.LaLiga, new League("https://www.transfermarkt.world/la-liga/startseite/wettbewerb/ES1", "ЛаЛига", 1));
            LeaguesDic.Add(LeaguesEnum.SeriaA, new League("https://www.transfermarkt.world/serie-a/startseite/wettbewerb/IT1", "Серия А", 2));
            LeaguesDic.Add(LeaguesEnum.BundesLiga, new League("https://www.transfermarkt.world/1-bundesliga/startseite/wettbewerb/L1", "Бундеслига 1", 4));
            LeaguesDic.Add(LeaguesEnum.FNL, new League("https://www.transfermarkt.world/1-division/startseite/wettbewerb/RU2", "ФНЛ", 0));
            LeaguesDic.Add(LeaguesEnum.Liga1, new League("https://www.transfermarkt.world/ligue-1/startseite/wettbewerb/FR1", "Лига 1", 3));
            LeaguesDic.Add(LeaguesEnum.UPL, new League("https://www.transfermarkt.world/premier-liga/startseite/wettbewerb/UKR1", "УПЛ", 5));
            LeaguesDic.Add(LeaguesEnum.APL, new League("https://www.transfermarkt.world/premier-league/startseite/wettbewerb/GB1", "АПЛ", 6));

            CountriesDic.Add(CountriesEnum.Russia, new Country("Россия", "🇷🇺", 0));
            CountriesDic.Add(CountriesEnum.Spain, new Country("Испания", "🇪🇸", 1));
            CountriesDic.Add(CountriesEnum.Italy, new Country("Италия", "🇮🇹", 2));
            CountriesDic.Add(CountriesEnum.France, new Country("Франция", "🇫🇷", 3));
            CountriesDic.Add(CountriesEnum.Germany, new Country("Германия", "🇩🇪", 4));
            CountriesDic.Add(CountriesEnum.Ukraine, new Country("Украина", "🇺🇦", 5));
            CountriesDic.Add(CountriesEnum.England, new Country("Англия", "🏴󠁧󠁢󠁥󠁮󠁧󠁿", 6));


            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36");
            
            foreach (var item in LeaguesDic)
            {
                item.Value.Data = await Task.Run(() => LoadPage(item.Value.Link));
                ParsePage(item.Value.Data, item.Value.TeamDic);
            }
            Console.WriteLine("команды спаршены");
            short iter = 0;
            foreach (var item in CountriesDic)
            {
                iter = 0;
                foreach (var it in LeaguesDic)
                {
                    if (it.Value.CountryID == item.Value.ID)
                    {
                        item.Value.Divisions.Add(iter, it.Value);
                        iter++;
                    }
                }
            }
            teamsTable.Add(dataRow);
            mnPrg.GetTable(teamsTable, LeaguesDic, CountriesDic, teamPageLink);
        }

        public async static Task<string> LoadPage(string url)
        {
            try
            {
                //WebRequest request = WebRequest.Create(url);
                //using WebResponse response = await request.GetResponseAsync();
                //using Stream stream1 = response.GetResponseStream(); //не варик
                //HtmlDocument NewsDocument = new HtmlDocument();
                //NewsDocument.Load(stream1);


                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return "";
            }
        }
        static async public void ParsePage(string data, Dictionary<int, string> tmDic)
        {
            HtmlAgilityPack.HtmlDocument html = new HtmlDocument();
            html.LoadHtml(data);

            foreach (HtmlNode item in html.DocumentNode.QuerySelectorAll(".items"))
            {
                HtmlAgilityPack.HtmlNode table = item.QuerySelector("tbody");
                foreach (HtmlAgilityPack.HtmlNode row in table.QuerySelectorAll("tr"))
                {
                    foreach (HtmlNode cell in row.QuerySelectorAll("td"))
                    {
                        if (cell.InnerText != "")
                        {



                            var step1 = Regex.Replace(cell.InnerText, @"<[^>]+>|&nbsp;", "").Trim();
                            var step2 = Regex.Replace(step1, @"\s{2,}", " ");
                            dataRow.Add(step2);
                            if (dataRow.Count % 6 == 1 || dataRow.Count == 0)
                            {

                                tmDic.Add(tmDic.Count + 1, step2);
                                foreach (var href in cell.QuerySelectorAll("a"))
                                {
                                    
                                    var link = href.GetAttributeValue("href", "");

                                   
                                    if (link != "" && link != "#")
                                    {
                                        teamPageLink.Add(step2, link);
                                    }
                                }
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                break;
            }
        }


        static public async Task<List<List<string>>> ParseTeamPage(string data, List<List<string>> a)
        {
            HtmlAgilityPack.HtmlDocument html = new HtmlDocument();
            html.LoadHtml(data);

            foreach (HtmlNode item in html.DocumentNode.QuerySelectorAll(".items"))
            {
                HtmlAgilityPack.HtmlNode table = item.QuerySelector("tbody");
                int itr = 0;
                foreach (HtmlAgilityPack.HtmlNode row in table.QuerySelectorAll("tr"))
                {
                    itr++;
                    List<string> b = new List<string>();
                    int lngth = 0;
                    if (itr % 3 == 2  || itr % 3 == 0)
                    {
                        continue;
                    }
                    else
                    {
                        foreach (HtmlNode cell in row.QuerySelectorAll("td"))
                        {
                            //Console.WriteLine(cell.InnerText);
                            if (lngth != 1 && lngth != 2 && lngth != 3)
                            {
                                switch (lngth)
                                {
                                    case 0:
                                        b.Add("№ Номер: " + cell.InnerText);
                                        break;
                                    case 4:
                                        b.Add("🛡Позиция: " + cell.InnerText);
                                        break;
                                    case 5:
                                        b.Add("👤Имя: " + cell.InnerText);
                                        break;
                                    case 6:
                                        b.Add("👶День рождения (возраст): " + cell.InnerText);
                                        break;
                                    case 7:
                                        b.Add("🏳Страна: " + cell.FirstChild.GetAttributeValue("title", ""));
                                        break;
                                    case 8:
                                        b.Add("💶Цена: " + cell.InnerText);
                                        break;
                                }


                            }
                            lngth++;
                        }
                    }

                    a.Add(b);
                }
            }
            Console.WriteLine("Команда спаршена " + DateTime.Now);
            return a;
        }
    }
    
}