using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Objects
{
    public class DistrictCoordinate
    {
        public long coordinate_id { get; set; }
        public decimal lat { get; set; }
        public decimal lng { get; set; }
        public long district_id { get; set; }
    }
}
