using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlingTask
{
    public class AuctionModel
    {
        public string title { get; set; }
        public string imageUrl { get; set; }
        public string link { get; set; }
        public int lotCount { get; set; }
        public int startingDate { get; set; }
        public int endingDate { get; set; }
        public string startingMonth { get; set; }
        public string endingMonth { get; set; }
        public int startingYear { get; set; }
        public int endingYear { get; set; }
        public string startingTime { get; set; }
        public string endingTime { get; set; }
        public string location { get; set; }
    }
}
