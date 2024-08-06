using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace CrawlingTask
{
    class Program
    {
        public static void Main(string[] args)
        {
            String url = "https://ineichen.com/auctions/past/";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            List<string> datetimeRegexList = new List<string>() 
            {
                @"^(\d+)\s-\s(\d+)\s(\w+)\s(\d{2}:\d{2}\sCET)$",
                @"^(\d+)\s-\s(\d+)\s(\w+)\s(\d{4})$",
                @"^(\d+)\s-\s(\d+)\s(\w+)$",
                @"^(\d)+\s(\w+)\s-\s(\d+)\s(\w+)$",
                @"^(\d+)\s(\w+),\s(\d{2}:\d{2}\sCET)\s(\d+)\s(\w+),\s(\d{2}:\d{2}\sCET)$",
                @"^(\d+)\s(\w+),\s(\d{2}:\d{2}\s\(CET\))$",
                @"^(\d+)\s(\w+)\s-\s(\d+)\s(\w+)\s(\d{2}:\d{2}\sCET)$",
                @"^(\d+)\s(\w+)\s-\s(\d+)\s(\w+)\s(\d{4})$",
                @"^(\d+)\s(\w+)\s\d{4},\s(\d{2}:\d{2}\s\(CET\))"

            };

            HtmlNodeCollection Nodes = doc.DocumentNode.SelectNodes("//div[contains(@class,'auctions-list')]//div[@id]");

            string titleXPath = ".//h2[@class='auction-item__name']/a";
            string descriptionXPath = ".//*[@class='auction-date-location']";
            string imageUrlXPath = ".//a[contains(@class,'auction-item__image')]/img";
            string linkXPath = ".//div[contains(@class,'auction-item__btns')]/a";
            string lotCountXPath = ".//div[contains(@class,'auction-item__btns')]/a";
            string startDateXPath = ".//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]";
            string startMonthXPath = ".//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]";
            string startYearXPath = ".//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]";
            string startTimeXPath = ".//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]";
            string endDateXPath = ".//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]";
            string endMonthXPath = ".//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]";
            string endYearXPath = ".//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]";
            string endTimeXPath = ".//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]";
            string locationXPath = ".//div[@class='auction-date-location']//div[i[contains(@class,'mdi-map-marker-outline')]|i[contains(@class,'mdi-web')]]";

            string descriptionRegex = @"\s+";

            if (Nodes != null)
            {
                int n = 1;
                foreach (var card in Nodes)
                {
                    AuctionModel model = new AuctionModel();
                    var titleNode = card.SelectSingleNode(titleXPath);
                    var descriptionNode = card.SelectSingleNode(descriptionXPath);
                    var imageUrlNode = card.SelectSingleNode(imageUrlXPath);
                    var linkNode = card.SelectSingleNode(linkXPath);
                    var lotCountNode = card.SelectSingleNode(lotCountXPath);
                    var dateTimeNode = card.SelectSingleNode(startDateXPath);

                    /*
                    model.title = titleNode.InnerText.Trim();
                    model.description = Regex.Replace(descriptionNode.InnerText.Trim(), descriptionRegex, " ");
                    
                    string imageUrlsrc = imageUrlNode.GetAttributeValue("src", string.Empty);
                    Uri absoluteUrl = new Uri(new Uri(url), imageUrlsrc);
                    model.imageUrl= absoluteUrl.ToString();
                    string linkText = linkNode.GetAttributeValue("href", string.Empty);
                    Uri absoluteUrl = new Uri(new Uri(url), linkText);
                    model.link = absoluteUrl.ToString();
                    string lotCountPattern = @"\d+";
                    Regex lotCountRegex = new Regex(lotCountPattern);
                    Match match = lotCountRegex.Match(lotCountNode.InnerText.Trim());
                    if (match.Success)
                    {
                        model.lotCount = Convert.ToInt32(match.Value);
                        Console.WriteLine(model.lotCount);
                    }
                    */
                    Console.WriteLine(dateTimeNode.InnerText.Trim());
                    Console.WriteLine("--------------------------------");

                    n++;
                }
            }
            Console.ReadKey();
        }
    }
}
