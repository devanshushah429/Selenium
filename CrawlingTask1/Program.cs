using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data;

namespace CrawlingTask1
{
    class Program
    {
        public static string url = "https://ineichen.com/auctions/past/";
        public static HtmlWeb web = new HtmlWeb();
        public static HtmlDocument doc = web.Load(url);
        public static void Main(string[] args)
        {

            /*
            GetAllDates();
            List<string> discriptionList = GetAllDiscription();
            */
            /*int i = 0;
            foreach(int a in endingList)
            {
                i++;
                Console.WriteLine(i + ") " + a);
            }
            GetAllLocation();
            */
            List<string> titlesList = GetAllTitles();
            List<string> imageUrlList = GetAllImageUrl();
            List<string> linkList = getAllLinks();
            List<int> lotCountList = GetAllLotCounts();
            List<int> startingDatesList = GetAllStartingDates();
            List<int> endingDatesList = GetAllEndingDates();
            List<string> startingMonthList = GetAllStartMonth();
            List<string> endingMonthList = GetAllEndingMonth();
            List<int> startingYearList = GetAllStartingYear();
            List<int> endingYearList = GetAllEndingYear();
            List<string> startingTimeList = GetAllStartingTime();
            List<string> locationList = GetAllLocation();
            InsertIntoDatabase();
            Console.Read();
        }

        private static void InsertIntoDatabase(AuctionModel model)
        {
            string connStr = "Server=DESKTOP-B32RQ3U;Database=Ineichen;Integrated Security=True;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("PR_AuctionsDemo_InsertData", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Title", model.title);
                        cmd.Parameters.AddWithValue("@Description", model.description);
                        cmd.Parameters.AddWithValue("@ImageUrl", model.imageUrl);
                        cmd.Parameters.AddWithValue("@Link", model.Link);
                        cmd.Parameters.AddWithValue("@LotCount", model.lotCount);
                        cmd.Parameters.AddWithValue("@StartDate", model.startingDate);
                        cmd.Parameters.AddWithValue("@StartMonth", model.startingDate);
                        cmd.Parameters.AddWithValue("@StartYear", model.startingDate);
                        cmd.Parameters.AddWithValue("@StartTime", model.startingDate);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        Console.WriteLine("Rows affected: " + rowsAffected);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        #region Titles
        public static List<string> GetAllTitles()
        {
            List<string> titleList = new List<string>();
            string titlesXPath = "//h2[contains(@class,'auction-item__name')]/a";
            var titleNodes = doc.DocumentNode.SelectNodes(titlesXPath);
            foreach (var node in titleNodes)
            {
                titleList.Add(node.InnerText);
            }
            return titleList;
        }
        #endregion

        #region Description List
        public static List<string> GetAllDiscription()
        {
            List<string> descriptionList = new List<string>();
            string titlesXPath = "//div[@class='auction-date-location']";
            var descriptionNodes = doc.DocumentNode.SelectNodes(titlesXPath);
            if (descriptionNodes != null)
            {
                foreach (var node in descriptionNodes)
                {
                    descriptionList.Add(node.InnerText);
                    Console.WriteLine(node.InnerText);
                }
            }
            else
            {
                Console.WriteLine("Not Found");
            }

            return descriptionList;
        }
        #endregion

        #region Image URL
        public static List<string> GetAllImageUrl()
        {
            List<string> imageUrlList = new List<string>();
            string imageUrlXPath = "//a[@class='auction-item__image']/img";
            var urlNodes = doc.DocumentNode.SelectNodes(imageUrlXPath);
            foreach (var imgNode in urlNodes)
            {
                string src = imgNode.GetAttributeValue("src", string.Empty);
                Uri absoluteUrl = new Uri(new Uri(url), src);
                imageUrlList.Add(absoluteUrl.AbsoluteUri.ToString());
            }
            return imageUrlList;
        }
        #endregion

        #region Links
        public static List<string> getAllLinks()
        {
            List<string> linksList = new List<string>();
            string linksXPath = "//div[@class='auction-item__btns']/a";
            var linkNodes = doc.DocumentNode.SelectNodes(linksXPath);
            foreach (var linkNode in linkNodes)
            {
                string src = linkNode.GetAttributeValue("href", string.Empty);
                Uri absoluteUrl = new Uri(new Uri(url), src);
                linksList.Add(absoluteUrl.AbsoluteUri.ToString());
            }
            return linksList;

        }
        #endregion

        #region Lot Counts
        public static List<int> GetAllLotCounts()
        {
            List<int> lotCountList = new List<int>();
            HtmlNodeCollection lotCountNodes = doc.DocumentNode.SelectNodes("//div[@class='auction-item__btns']/a");
            string pattern = @"\d+";
            Regex regex = new Regex(pattern);
            if (lotCountNodes != null)
            {
                foreach (HtmlNode lotCountNode in lotCountNodes)
                {
                    Match match = regex.Match(lotCountNode.InnerText.Trim());
                    if (match.Success)
                    {
                        lotCountList.Add(int.Parse(match.Value));
                    }
                }
            }
            return lotCountList;
        }
        #endregion

        #region Starting Date
        public static List<int> GetAllStartingDates()
        {
            List<int> list = new List<int>();
            string xPath = "//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]";
            var descriptionNodes = doc.DocumentNode.SelectNodes(xPath);
            int i = 0;
            string pattern = @"^\d+(?=[-|\s])";
            Regex regex = new Regex(pattern);
            if (descriptionNodes != null)
            {
                foreach (var a in descriptionNodes)
                {
                    Match match = regex.Match(a.InnerText.Trim());
                    if (match.Success)
                    {
                        list.Add(int.Parse(match.Value));
                    }
                    else
                    {
                        list.Add(-1);
                    }
                }
            }
            else
            {
                Console.WriteLine("Not Found");
            }
            return list;
        }
        #endregion

        #region  Ending Dates
        public static List<int> GetAllEndingDates()
        {
            List<int> list = new List<int>();
            string xPath = "//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]";
            var endingDatesNodes = doc.DocumentNode.SelectNodes(xPath);
            int i = 0;
            string pattern = @"(?<=\s|-)\d{1,2}\s\w+\s(\d{4})";
            Regex regex = new Regex(pattern);
            if (endingDatesNodes != null)
            {
                foreach (var a in endingDatesNodes)
                {
                    Match match = regex.Match(a.InnerText.Trim());
                    if (match.Success)
                    {
                        Console.WriteLine(match.Groups[0].Value);
                        list.Add(int.Parse(match.Groups[0].Value));
                    }
                    else
                    {
                        list.Add(-1);
                    }
                }
            }
            else
            {
                Console.WriteLine("Not Found");
            }
            return list;
        }
        #endregion

        #region Start Month
        public static List<string> GetAllStartMonth()
        {
            List<String> list = new List<String>();
            string xPath = "//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]";
            var descriptionNodes = doc.DocumentNode.SelectNodes(xPath);
            int i = 0;
            string pattern = @"(?<=\d{1,2}\s)([A-Z]+)";
            Regex regex = new Regex(pattern);
            if (descriptionNodes != null)
            {
                foreach (var a in descriptionNodes)
                {
                    Match match = regex.Match(a.InnerText.Trim());
                    if (match.Success)
                    {
                        list.Add(match.Value);
                    }
                    else
                    {
                        list.Add("");
                    }
                }
            }
            else
            {
                Console.WriteLine("Not Found");
            }
            foreach (string item in list)
            {
                Console.WriteLine(item);
            }
            return list;
        }
        #endregion

        #region End Month
        public static List<string> GetAllEndingMonth()
        {
            List<String> list = new List<String>();
            string xPath = "//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]";
            var descriptionNodes = doc.DocumentNode.SelectNodes(xPath);
            int i = 0;
            string pattern = @"(?<=\s|-)\d{1,2}\s([A-Z]+)";
            Regex regex = new Regex(pattern);
            if (descriptionNodes != null)
            {
                foreach (var a in descriptionNodes)
                {
                    Match match = regex.Match(a.InnerText.Trim());
                    if (match.Success)
                    {
                        list.Add(match.Groups[1].Value);
                    }
                    else
                    {
                        list.Add("");
                    }
                }
            }
            else
            {
                Console.WriteLine("Not Found");
            }
            foreach (string item in list)
            {
                Console.WriteLine(item);
            }
            return list;
        }
        #endregion

        #region Start Year
        public static List<int> GetAllStartingYear()
        {
            List<int> list = new List<int>();
            string xPath = "//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]";
            var descriptionNodes = doc.DocumentNode.SelectNodes(xPath);
            int i = 0;
            string pattern = @"\w+\s(\d{4})";
            Regex regex = new Regex(pattern);
            if (descriptionNodes != null)
            {
                foreach (var a in descriptionNodes)
                {
                    Match match = regex.Match(a.InnerText.Trim());
                    if (match.Success)
                    {
                        list.Add(int.Parse(match.Groups[1].Value));
                    }
                    else
                    {
                        list.Add(-1);
                    }
                }
                foreach (int item in list)
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Console.WriteLine("Not Found");
            }
            return list;
        }
        #endregion

        #region Ending Year
        public static List<int> GetAllEndingYear()
        {
            List<int> list = new List<int>();
            string xPath = "//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]";
            var descriptionNodes = doc.DocumentNode.SelectNodes(xPath);
            int i = 0;
            string pattern = @"\w+\s(\d{4})";
            Regex regex = new Regex(pattern);
            if (descriptionNodes != null)
            {
                foreach (var a in descriptionNodes)
                {
                    Match match = regex.Match(a.InnerText.Trim());
                    if (match.Success)
                    {
                        list.Add(int.Parse(match.Groups[1].Value));
                    }
                    else
                    {
                        list.Add(-1);
                    }
                }
                foreach (int item in list)
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Console.WriteLine("Not Found");
            }
            return list;
        }
        #endregion

        #region Starting Time
        public static List<string> GetAllStartingTime()
        {
            List<string> list = new List<string>();
            string xPath = "//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]";
            var descriptionNodes = doc.DocumentNode.SelectNodes(xPath);
            int i = 0;
            string pattern = @"(\d{2}:\d{2})\s\(?CET\)?";
            Regex regex = new Regex(pattern);
            if (descriptionNodes != null)
            {
                foreach (var a in descriptionNodes)
                {
                    Match match = regex.Match(a.InnerText.Trim());
                    if (match.Success)
                    {
                        list.Add(match.Groups[1].Value);
                    }
                    else
                    {
                        list.Add("");
                    }
                }
                foreach (string item in list)
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Console.WriteLine("Not Found");
            }
            return list;
        }
        #endregion

        #region Location
        public static List<string> GetAllLocation()
        {
            List<string> locationList = new List<string>();
            string locationXPath = "//div[i[contains(@class,'mdi-web')]] | //div[i[contains(@class,'mdi-map')]]";
            var locationNodes = doc.DocumentNode.SelectNodes(locationXPath);
            int i = 0;
            foreach (var v in locationNodes)
            {
                locationList.Add(v.InnerText.Trim());
            }
            return locationList;
        }
        #endregion
    }
}