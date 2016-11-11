using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Objects
{
    public class CardPoint
    {
        public long google_point_id { get; set; }
        public decimal lat { get; set; }
        public decimal lng { get; set; }
        public string google_status { get; set; }
        public string formatted_address { get; set; }
    }
}
