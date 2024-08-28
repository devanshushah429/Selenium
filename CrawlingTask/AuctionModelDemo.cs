using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlingTask
{
    public class AuctionModelDemo
    {
        public string title { get; set; }
        public string description { get; set; }
        public string imageUrl { get; set; }
        public string link { get; set; }
        public int lotCount { get; set; }
        public int startDate { get; set; }
        public int endDate { get; set; }
        public string startMonth { get; set; }
        public string endMonth { get; set; }
        public int startYear { get; set; }
        public int endYear { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string location { get; set; }
        public void PrintData()
        {
            Console.WriteLine("Title : " + this.title);
            Console.WriteLine("ImageUrl : " + this.imageUrl);
            Console.WriteLine("link : " + this.link);
            Console.WriteLine("lotCount : " + this.lotCount);
            Console.WriteLine("start date : " + this.startDate);
            Console.WriteLine("startMonth : " + this.startMonth);
            Console.WriteLine("startYear : " + this.startYear);
            Console.WriteLine("startTime : " + this.startTime);
            Console.WriteLine("end date : " + this.endDate);
            Console.WriteLine("end Month : " + this.endMonth);
            Console.WriteLine("end Year : " + this.endYear);
            Console.WriteLine("end Time : " + this.endTime);
            Console.WriteLine("Location : " + this.location);
        }
        public bool Equals(AuctionModel model)
        {
            return (this.title.Equals(model.title) &&
                this.imageUrl.Equals(model.imageUrl) &&
                this.lotCount.Equals(model.lotCount) &&
                this.startDate.Equals(model.startDate) &&
                this.startMonth.Equals(model.startMonth) &&
                this.startYear.Equals(model.startYear) &&
                this.startTime.Equals(model.startTime) &&
                this.endDate.Equals(model.endDate) &&
                this.endMonth.Equals(model.endMonth) &&
                this.endYear.Equals(model.endYear) &&
                this.endTime.Equals(model.endTime) &&
                this.location.Equals(model.location)
                );
        }
    }

}