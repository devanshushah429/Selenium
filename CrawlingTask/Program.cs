using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace CrawlingTask
{
    class Program
    {
        private static string connStr = "Server=DESKTOP-B32RQ3U;Database=Ineichen;Integrated Security=True;";
        
        public static string multipleSpaceRegex = @"\s+";
        public static void Main(string[] args)
        {
            String url = "https://ineichen.com/auctions/past/";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);

            HtmlNodeCollection Nodes = doc.DocumentNode.SelectNodes("//div[contains(@class,'auctions-list')]//div[@id]");

            string titleXPath = ".//h2[@class='auction-item__name']/a";
            string descriptionXPath = ".//*[@class='auction-date-location']";
            string imageUrlXPath = ".//a[contains(@class,'auction-item__image')]/img";
            string linkXPath = ".//div[contains(@class,'auction-item__btns')]/a";
            string lotCountXPath = ".//div[contains(@class,'auction-item__btns')]/a";
            string startDateXPath = ".//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]";
            string locationXPath = ".//div[@class='auction-date-location']//div[i[contains(@class,'mdi-map-marker-outline')]|i[contains(@class,'mdi-web')]]";

            int n = 1;
            if (Nodes != null)
            {
                foreach (var node in Nodes)
                {
                    AuctionModel model = new AuctionModel();

                    // Creating Nodes
                    var titleNode = node.SelectSingleNode(titleXPath);
                    var descriptionNode = node.SelectSingleNode(descriptionXPath);
                    var imageUrlNode = node.SelectSingleNode(imageUrlXPath);
                    var linkNode = node.SelectSingleNode(linkXPath);
                    var lotCountNode = node.SelectSingleNode(lotCountXPath);
                    var dateTimeNode = node.SelectSingleNode(startDateXPath);
                    var locationNode = node.SelectSingleNode(locationXPath);

                    // Set Values in model
                    SetTitle(model, titleNode);
                    SetDescription(model, descriptionNode);
                    SetImageUrl(url, model, imageUrlNode);
                    SetLink(url, model, linkNode);
                    SetLotCount(model, lotCountNode);
                    SetDate(model, dateTimeNode.InnerText.Trim());
                    SetLocation(model, locationNode);

                    Console.WriteLine(n+" )");
                    model.PrintData();
                    Console.WriteLine("-----------------------------------------------------------------");
                    n++;
                    
                    AuctionModel oldData = GetDataByLink(model.link);
                    if (oldData.title.Equals("N/A"))
                    {
                        AddRecordToDatabase(model);
                    }
                    else if (!model.Equals(oldData))
                    {
                        UpdateInDatabaseUsingLink(model);
                    }
                }
            }
            Console.ReadKey();
        }
        private static bool UpdateInDatabaseUsingLink(AuctionModel model)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("PR_UpdateDataByLink", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Title", model.title);
                        cmd.Parameters.AddWithValue("@Description", model.description);
                        cmd.Parameters.AddWithValue("@ImageUrl", model.imageUrl);
                        cmd.Parameters.AddWithValue("@Link", model.link);
                        cmd.Parameters.AddWithValue("@LotCount", model.lotCount);
                        cmd.Parameters.AddWithValue("@StartDate", model.startingDate);
                        cmd.Parameters.AddWithValue("@StartMonth", model.startingMonth);
                        cmd.Parameters.AddWithValue("@StartYear", model.startingYear);
                        cmd.Parameters.AddWithValue("@StartTime", model.startingTime);
                        cmd.Parameters.AddWithValue("@EndDate", model.endingDate);
                        cmd.Parameters.AddWithValue("@EndMonth", model.endingMonth);
                        cmd.Parameters.AddWithValue("@EndYear", model.endingYear);
                        cmd.Parameters.AddWithValue("@EndTime", model.endingTime);
                        cmd.Parameters.AddWithValue("@Location", model.location);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            return false;

        }
        private static AuctionModel GetDataByLink(string link)
        {
            try
            {
                AuctionModel model = new AuctionModel();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("PR_GetDataByLink", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Link", link);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                model.title = reader["Title"].ToString();
                                model.description = reader["Description"].ToString();
                                model.imageUrl = reader["ImageUrl"].ToString();
                                model.link = reader["Link"].ToString();
                                model.lotCount = Convert.ToInt32(reader["LotCount"]);
                                model.startingDate = Convert.ToInt32(reader["StartDate"]);
                                model.startingMonth = reader["StartMonth"].ToString();
                                model.startingYear = Convert.ToInt32(reader["StartYear"]);
                                model.startingTime = reader["StartTime"].ToString();
                                model.endingDate = Convert.ToInt32(reader["EndDate"]);
                                model.endingMonth = reader["EndMonth"].ToString();
                                model.endingYear = Convert.ToInt32(reader["EndYear"]);
                                model.endingTime = reader["EndTime"].ToString();
                                model.location = reader["Location"].ToString();
                            }
                        }
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new AuctionModel();
            }
        }
        private static bool AddRecordToDatabase(AuctionModel model)
        {
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
                        cmd.Parameters.AddWithValue("@Link", model.link);
                        cmd.Parameters.AddWithValue("@LotCount", model.lotCount);
                        cmd.Parameters.AddWithValue("@StartDate", model.startingDate);
                        cmd.Parameters.AddWithValue("@StartMonth", model.startingMonth);
                        cmd.Parameters.AddWithValue("@StartYear", model.startingYear);
                        cmd.Parameters.AddWithValue("@StartTime", model.startingTime);
                        cmd.Parameters.AddWithValue("@EndDate", model.endingDate);
                        cmd.Parameters.AddWithValue("@EndMonth", model.endingMonth);
                        cmd.Parameters.AddWithValue("@EndYear", model.endingYear);
                        cmd.Parameters.AddWithValue("@EndTime", model.endingTime);
                        cmd.Parameters.AddWithValue("@Location", model.location);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            return false;
        }

        private static void SetLocation(AuctionModel model, HtmlNode locationNode)
        {
            model.location = locationNode.InnerText.Trim().ToString();
        }

        private static void SetLotCount(AuctionModel model, HtmlNode lotCountNode)
        {
            string lotCountPattern = @"\d+";
            Regex lotCountRegex = new Regex(lotCountPattern);
            Match match1 = lotCountRegex.Match(lotCountNode.InnerText.Trim());
            if (match1.Success)
            {
                model.lotCount = Convert.ToInt32(match1.Value);
            }
        }

        private static void SetLink(string url, AuctionModel model, HtmlNode linkNode)
        {
            string linkText = linkNode.GetAttributeValue("href", string.Empty);
            Uri absoluteUrlLink = new Uri(new Uri(url), linkText);
            model.link = absoluteUrlLink.ToString();
        }

        private static void SetImageUrl(string url, AuctionModel model, HtmlNode imageUrlNode)
        {
            string imageUrlsrc = imageUrlNode.GetAttributeValue("src", string.Empty);
            Uri absoluteUrl = new Uri(new Uri(url), imageUrlsrc);
            model.imageUrl = absoluteUrl.ToString();
        }

        private static void SetDescription(AuctionModel model, HtmlNode descriptionNode)
        {
            model.description = Regex.Replace(descriptionNode.InnerText.Trim(), multipleSpaceRegex, " ");
        }

        private static void SetTitle(AuctionModel model, HtmlNode titleNode)
        {
            model.title = titleNode.InnerText.Trim();
        }

        private static void SetDate(AuctionModel model, string stringToMatch)
        {
            List<string> datetimeRegexList = new List<string>()
            {
                @"^(\d+)\s?-\s?(\d+)\s(\w+)\s(\d{2}:\d{2}\sCET)$",
                @"^(\d+)\s-\s(\d+)\s(\w+)\s(\d{4})$",
                @"^(\d+)\s-\s(\d+)\s(\w+)$",
                @"^(\d)+\s(\w+)\s-\s(\d+)\s(\w+)$",
                @"^(\d+)\s(\w+),\s(\d{2}:\d{2}\sCET)\s(\d+)\s(\w+),\s(\d{2}:\d{2}\sCET)$",
                @"^(\d+)\s(\w+),\s(\d{2}:\d{2}\s\(CET\))$",
                @"^(\d+)\s(\w+)\s-\s(\d+)\s(\w+)\s(\d{2}:\d{2}\sCET)$",
                @"^(\d+)\s(\w+)\s-\s(\d+)\s(\w+)\s(\d{4})$",
                @"^(\d+)\s(\w+)\s(\d{4}),\s(\d{2}:\d{2}\s\(CET\))$",
                @"^(\d+)\s(\w+)\s(\d{4})\s-\s(\d+)\s(\w+)\s(\d{4})$"
            };
            int dateRegexListLength = datetimeRegexList.Count;

            for (int i = 0; i < dateRegexListLength; i++)
            {
                Regex dateRegex = new Regex(datetimeRegexList[i]);
                var dateString = Regex.Replace(stringToMatch, multipleSpaceRegex, " ");
                Match match = dateRegex.Match(dateString);
                if (match.Success)
                {
                    if (i == 0)
                    {
                        model.startingDate = Convert.ToInt32(match.Groups[1].Value);
                        model.endingDate = Convert.ToInt32(match.Groups[2].Value);
                        model.startingMonth = match.Groups[3].Value;
                        model.endingMonth = match.Groups[3].Value;
                        model.endingTime = match.Groups[4].Value;

                    }
                    else if (i == 1)
                    {
                        model.startingDate = Convert.ToInt32(match.Groups[1].Value);
                        model.endingDate = Convert.ToInt32(match.Groups[2].Value);
                        model.startingMonth = match.Groups[3].Value;
                        model.endingMonth = match.Groups[3].Value;
                        model.startingYear = Convert.ToInt32(match.Groups[4].Value);
                        model.endingYear = Convert.ToInt32(match.Groups[4].Value);
                    }
                    else if (i == 2)
                    {
                        model.startingDate = Convert.ToInt32(match.Groups[1].Value);
                        model.endingDate = Convert.ToInt32(match.Groups[2].Value);
                        model.startingMonth = match.Groups[3].Value;
                        model.endingMonth = match.Groups[3].Value;
                    }
                    else if (i == 3)
                    {
                        model.startingDate = Convert.ToInt32(match.Groups[1].Value);
                        model.startingMonth = match.Groups[2].Value;
                        model.endingDate = Convert.ToInt32(match.Groups[3].Value);
                        model.endingMonth = match.Groups[4].Value;
                    }
                    else if (i == 4)
                    {
                        model.startingDate = Convert.ToInt32(match.Groups[1].Value);
                        model.startingMonth = match.Groups[2].Value;
                        model.startingTime = match.Groups[3].Value;
                        model.endingDate = Convert.ToInt32(match.Groups[4].Value);
                        model.endingMonth = match.Groups[5].Value;
                        model.endingTime = match.Groups[6].Value;
                    }
                    else if (i == 5)
                    {
                        model.startingDate = Convert.ToInt32(match.Groups[1].Value);
                        model.startingMonth = match.Groups[2].Value;
                        model.startingTime = match.Groups[3].Value;
                    }
                    else if (i == 6)
                    {
                        model.startingDate = Convert.ToInt32(match.Groups[1].Value);
                        model.startingMonth = match.Groups[2].Value;
                        model.endingDate = Convert.ToInt32(match.Groups[3].Value);
                        model.endingMonth = match.Groups[4].Value;
                        model.endingTime = match.Groups[5].Value;
                    }
                    else if (i == 7)
                    {
                        model.startingDate = Convert.ToInt32(match.Groups[1].Value);
                        model.startingMonth = match.Groups[2].Value;
                        model.endingDate = Convert.ToInt32(match.Groups[3].Value);
                        model.endingMonth = match.Groups[4].Value;
                        model.startingYear = Convert.ToInt32(match.Groups[5].Value);
                        model.endingYear = Convert.ToInt32(match.Groups[5].Value);
                    }
                    else if (i == 8)
                    {
                        model.startingDate = Convert.ToInt32(match.Groups[1].Value);
                        model.startingMonth = match.Groups[2].Value;
                        model.startingYear = Convert.ToInt32(match.Groups[3].Value);
                        model.startingTime = match.Groups[4].Value;
                    }
                    else if (i == 9)
                    {
                        model.startingDate = Convert.ToInt32(match.Groups[1].Value);
                        model.startingMonth = match.Groups[2].Value;
                        model.startingYear = Convert.ToInt32(match.Groups[3].Value);
                        model.endingDate = Convert.ToInt32(match.Groups[4].Value);
                        model.endingMonth = match.Groups[5].Value;
                        model.endingYear = Convert.ToInt32(match.Groups[6].Value);
                    }
                }
            }

        }
    }
}
