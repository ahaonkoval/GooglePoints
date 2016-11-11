using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Objects
{
    public class District
    {
        public long district_id { get; set; }
        public long potamus_district_id { get; set; }
        public long region_id { get; set; }
        public string name { get; set; }
        public decimal lat { get; set; }
        public decimal lng { get; set; }
    }
}
