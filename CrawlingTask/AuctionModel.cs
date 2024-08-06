using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlingTask
{
    public class AuctionModel
    {
        public string title { get; set; } = "N/A";
        public string description { get; set; } = "N/A";
        public string imageUrl { get; set; } = "N/A";
        public string link { get; set; } = "N/A";
        public int lotCount { get; set; } = -1;
        public int startingDate { get; set; } = -1;
        public int endingDate { get; set; } = -1;
        public string startingMonth { get; set; } = "N/A";
        public string endingMonth { get; set; } = "N/A";
        public int startingYear { get; set; } = -1;
        public int endingYear { get; set; } = -1;
        public string startingTime { get; set; } = "N/A";
        public string endingTime { get; set; } = "N/A";
        public string location { get; set; } = "N/A";
        public void PrintData()
        {
            Console.WriteLine("Title : " + this.title);
            Console.WriteLine("ImageUrl : " + this.imageUrl);
            Console.WriteLine("link : " + this.link);
            Console.WriteLine("lotCount : " + this.lotCount);
            Console.WriteLine("Starting date : " + this.startingDate);
            Console.WriteLine("startingMonth : " + this.startingMonth);
            Console.WriteLine("startingYear : " + this.startingYear);
            Console.WriteLine("startingTime : " + this.startingTime);
            Console.WriteLine("Ending date : " + this.endingDate);
            Console.WriteLine("Ending Month : " + this.endingMonth);
            Console.WriteLine("Ending Year : " + this.endingYear);
            Console.WriteLine("Ending Time : " + this.endingTime);
            Console.WriteLine("Location : " + this.location);
        }
        public bool Equals(AuctionModel model)
        {
            return (this.title.Equals(model.title) &&
                this.imageUrl.Equals(model.imageUrl) &&
                this.lotCount.Equals(model.lotCount) &&
                this.startingDate.Equals(model.startingDate) &&
                this.startingMonth.Equals(model.startingMonth) &&
                this.startingYear.Equals(model.startingYear) &&
                this.startingTime.Equals(model.startingTime) &&
                this.endingDate.Equals(model.endingDate) &&
                this.endingMonth.Equals(model.endingMonth) &&
                this.endingYear.Equals(model.endingYear) &&
                this.endingTime.Equals(model.endingTime) &&
                this.location.Equals(model.location)
                ) ;
        }
    }

}
