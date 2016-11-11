using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Objects
{
    public class Market
    {
        public long market_google_coordinates_id { get; set; }
        public string name_short { get; set; }
        public string address { get; set; }
        public decimal lat { get; set; }
        public decimal lng { get; set; }
        public string label { get; set; }
    }
}
