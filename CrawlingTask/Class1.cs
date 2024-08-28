using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CrawlingTask
{
    class Class1
    {
        private static string connStr = "Server=DESKTOP-B32RQ3U;Database=Ineichen;Integrated Security=True;";

        private static string multipleSpaceRegex = @"\s+";
        private static String url = "https://ineichen.com/auctions/past/";
        public static void Main1()
        {

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);

            HtmlNodeCollection Nodes = doc.DocumentNode.SelectNodes("//div[contains(@class,'auctions-list')]//div[@id]");

            int n = 1;
            if (Nodes != null)
            {
                foreach (HtmlNode node in Nodes)
                {
                    AuctionModelDemo model = new AuctionModelDemo();

                    // Set Values in model
                    SetModel(node, model);

                    // Print data of model
                    Console.WriteLine(n + " )");
                    model.PrintData();
                    Console.WriteLine("-----------------------------------------------------------------");
                    n++;

                    AuctionModelDemo oldData = GetDataByLink(model.link);
                    if (oldData.title == null)
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

        private static void SetModel(HtmlNode node, AuctionModelDemo model)
        {
            string titleXPath = ".//h2[@class='auction-item__name']/a";
            string descriptionXPath = ".//*[@class='auction-date-location']";
            string imageUrlXPath = ".//a[contains(@class,'auction-item__image')]/img";
            string linkXPath = ".//div[contains(@class,'auction-item__btns')]/a";
            string lotCountXPath = ".//div[contains(@class,'auction-item__btns')]/a";
            string startDateXPath = ".//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]";
            string locationXPath = ".//div[@class='auction-date-location']//div[i[contains(@class,'mdi-map-marker-outline')]|i[contains(@class,'mdi-web')]]";

            SetTitle(titleXPath, node, model);
            SetDescription(descriptionXPath, node, model);
            SetImageUrl(imageUrlXPath, node, model);
            SetLink(linkXPath, node, model);
            SetLotCount(lotCountXPath, node, model);
            SetDate(startDateXPath, node, model);
            SetLocation(locationXPath, node, model);
        }

        private static bool UpdateInDatabaseUsingLink(AuctionModelDemo model)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("PR_UpdateDataByLink", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Title", (model.title == null ? string.Empty : model.title));
                        cmd.Parameters.AddWithValue("@Description", (model.description == null ? string.Empty : model.description));
                        cmd.Parameters.AddWithValue("@ImageUrl", (model.imageUrl == null ? string.Empty : model.imageUrl));
                        cmd.Parameters.AddWithValue("@Link", (model.link == null ? string.Empty : model.link));
                        cmd.Parameters.AddWithValue("@LotCount", model.lotCount);
                        cmd.Parameters.AddWithValue("@StartDate", model.startDate);
                        cmd.Parameters.AddWithValue("@StartMonth", (model.startMonth == null ? string.Empty : model.startMonth));
                        cmd.Parameters.AddWithValue("@StartYear", model.startYear);
                        cmd.Parameters.AddWithValue("@StartTime", (model.startTime == null ? string.Empty : model.startTime));
                        cmd.Parameters.AddWithValue("@EndDate", model.endDate);
                        cmd.Parameters.AddWithValue("@EndMonth", (model.endMonth == null ? string.Empty : model.endMonth));
                        cmd.Parameters.AddWithValue("@EndYear", model.endYear);
                        cmd.Parameters.AddWithValue("@EndTime", (model.endTime == null ? string.Empty : model.endTime));
                        cmd.Parameters.AddWithValue("@Location", (model.location == null ? string.Empty : model.location));

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
        private static AuctionModelDemo GetDataByLink(string link)
        {
            try
            {
                AuctionModelDemo model = new AuctionModelDemo();
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
                                model.startDate = Convert.ToInt32(reader["StartDate"]);
                                model.startMonth = reader["StartMonth"].ToString();
                                model.startYear = Convert.ToInt32(reader["StartYear"]);
                                model.startTime = reader["StartTime"].ToString();
                                model.endDate = Convert.ToInt32(reader["EndDate"]);
                                model.endMonth = reader["EndMonth"].ToString();
                                model.endYear = Convert.ToInt32(reader["EndYear"]);
                                model.endTime = reader["EndTime"].ToString();
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
                return new AuctionModelDemo();
            }
        }
        private static bool AddRecordToDatabase(AuctionModelDemo model)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("PR_AuctionsDemo_InsertData", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Title", (model.title == null ? string.Empty : model.title));
                        cmd.Parameters.AddWithValue("@Description", (model.description == null ? string.Empty : model.description));
                        cmd.Parameters.AddWithValue("@ImageUrl", (model.imageUrl == null ? string.Empty : model.imageUrl));
                        cmd.Parameters.AddWithValue("@Link", (model.link == null ? string.Empty : model.link));
                        cmd.Parameters.AddWithValue("@LotCount", model.lotCount);
                        cmd.Parameters.AddWithValue("@StartDate", model.startDate);
                        cmd.Parameters.AddWithValue("@StartMonth", (model.startMonth == null ? string.Empty : model.startMonth));
                        cmd.Parameters.AddWithValue("@StartYear", model.startYear);
                        cmd.Parameters.AddWithValue("@StartTime", (model.startTime == null ? string.Empty : model.startTime));
                        cmd.Parameters.AddWithValue("@EndDate", model.endDate);
                        cmd.Parameters.AddWithValue("@EndMonth", (model.endMonth == null ? string.Empty : model.endMonth));
                        cmd.Parameters.AddWithValue("@EndYear", model.endYear);
                        cmd.Parameters.AddWithValue("@EndTime", (model.endTime == null ? string.Empty : model.endTime));
                        cmd.Parameters.AddWithValue("@Location", (model.location == null ? string.Empty : model.location));
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

        private static void SetLocation(string locationXPath, HtmlNode node, AuctionModelDemo model)
        {
            var locationNode = node.SelectSingleNode(locationXPath);
            model.location = locationNode.InnerText.Trim().ToString();
        }

        private static void SetLotCount(string lotCountXPath, HtmlNode node, AuctionModelDemo model)
        {
            HtmlNode lotCountNode = node.SelectSingleNode(lotCountXPath);
            string lotCountPattern = @"\d+";
            Regex lotCountRegex = new Regex(lotCountPattern);
            Match match1 = lotCountRegex.Match(lotCountNode.InnerText.Trim());
            if (match1.Success)
            {
                model.lotCount = Convert.ToInt32(match1.Value);
            }
        }

        private static void SetLink(string linkXPath, HtmlNode node, AuctionModelDemo model)
        {
            var linkNode = node.SelectSingleNode(linkXPath);
            string linkText = linkNode.GetAttributeValue("href", string.Empty);
            Uri absoluteUrlLink = new Uri(new Uri(url), linkText);
            model.link = absoluteUrlLink.ToString();
        }

        private static void SetImageUrl(string imageUrlXPath, HtmlNode node, AuctionModelDemo model)
        {
            var imageUrlNode = node.SelectSingleNode(imageUrlXPath);
            string imageUrlsrc = imageUrlNode.GetAttributeValue("src", string.Empty);
            Uri absoluteUrl = new Uri(new Uri(url), imageUrlsrc);
            model.imageUrl = absoluteUrl.ToString();
        }

        private static void SetDescription(string descriptionXPath, HtmlNode node, AuctionModelDemo model)
        {
            var descriptionNode = node.SelectSingleNode(descriptionXPath);
            model.description = Regex.Replace(descriptionNode.InnerText.Trim(), multipleSpaceRegex, " ");
        }

        private static void SetTitle(string titleXPath, HtmlNode node, AuctionModelDemo model)
        {
            HtmlNode titleNode = node.SelectSingleNode(titleXPath);
            model.title = titleNode.InnerText.Trim();
        }

        private static void SetDate(string startDateXPath, HtmlNode node, AuctionModelDemo model)
        {
            var dateTimeNode = node.SelectSingleNode(startDateXPath);
            List<string> datetimeRegexList = new List<string>()
            {
                @"^(?<startdate>\d+)\s?-\s?(?<endDate>\d+)\s(?<endmonth>\w+)\s(?<endtime>\d{2}:\d{2}\sCET)$",
                @"^(?<startdate>\d+)\s-\s(?<enddate>\d+)\s(?<endmonth>\w+)\s(?<endyear>\d{4})$",
                @"^(?<startdate>\d+)\s-\s(?<enddate>\d+)\s(?<endmonth>\w+)$",
                @"^(?<startdate>\d)+\s(?<startmonth>\w+)\s-\s(?<enddate>\d+)\s(?<endmonth>\w+)$",
                @"^(?<startdate>\d+)\s(?<startmonth>\w+),\s(?<starttime>\d{2}:\d{2}\sCET)\s(?<enddate>\d+)\s(?<endmonth>\w+),\s(?<endtime>\d{2}:\d{2}\sCET)$",
                @"^(?<startdate>\d+)\s(?<startmonth>\w+),\s(?<starttime>\d{2}:\d{2}\s\(CET\))$",
                @"^(?<startdate>\d+)\s(?<startmonth>\w+)\s-\s(?<enddate>\d+)\s(?<endmonth>\w+)\s(?<time>\d{2}:\d{2}\sCET)$",
                @"^(?<startdate>\d+)\s(?<startmonth>\w+)\s-\s(?<enddate>\d+)\s(?<endmonth>\w+)\s(?<endyear>\d{4})$",
                @"^(?<startdate>\d+)\s(?<startmonth>\w+)\s(?<startyear>\d{4}),\s(?<starttime>\d{2}:\d{2}\s\(CET\))$",
                @"(?<startdate>\d+)\s(?<startmonth>\w+)\s(?<startyear>\d{4})\s-\s(?<endDate>\d+)\s(?<endmonth>\w+)\s(?<endyear>\d{4})$"
            };
            int dateRegexListLength = datetimeRegexList.Count;

            for (int i = 0; i < dateRegexListLength; i++)
            {
                Regex dateRegex = new Regex(datetimeRegexList[i]);
                var dateString = Regex.Replace(dateTimeNode.InnerText.Trim(), multipleSpaceRegex, " ");
                Match match = dateRegex.Match(dateString);
                if (match.Success)
                {
                    model.startDate = SafeConvertToInt32(match.Groups["startdate"].Value);
                    model.endDate = SafeConvertToInt32(match.Groups["enddate"].Value);
                    model.startMonth = match.Groups["startmonth"].Value;
                    model.endMonth = match.Groups["endmonth"].Value;
                    model.startYear = SafeConvertToInt32(match.Groups["startyear"].Value);
                    model.endYear = SafeConvertToInt32(match.Groups["endyear"].Value);
                    model.startTime = match.Groups["starttime"].Value;
                    model.endTime = match.Groups["endtime"].Value;

                }
            }

        }
        public static int SafeConvertToInt32(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }
            return Convert.ToInt32(value);
        }
    }
}
