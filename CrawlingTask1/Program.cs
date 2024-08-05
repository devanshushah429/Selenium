using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace CrawlingTask1
{
    class Program
    {
public static void Main(string[] args)
        {
            // URL of the website to scrape
            var url = "https://ineichen.com/auctions/past/";

            // Load the web page
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var titleNodes = doc.DocumentNode.SelectNodes("//h2[contains(@class,'auction-item__name')]/a");

            foreach(var node in titleNodes)
            {
                Console.WriteLine("Text : "+node.InnerText);
            }
            Console.Read();

        }

    }
}