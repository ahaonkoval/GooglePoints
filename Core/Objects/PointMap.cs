using System;
using System.Collections.Generic;
using System.Linq;
using System.Spatial;
using System.Text;
using System.Threading.Tasks;

using Core.Helpers;

namespace Core.Objects
{
    public class PointMap
    {
        public PointType Type { get; set; }
        public long CardId { get; set; }
        public Coordinate Coordinate { get; set; }
        public string SourceAddress { get; set; }
        public string FormattedAddress { get; set; }
        public SearchEngineStatus Status { get; set; }
        public string Xml { get; set; }
        public PointMap() {
            this.Coordinate = null;
        }
        public PointMap(long card_id, Coordinate coordinate, string source_address, string formatted_address, SearchEngineStatus status)
        {
            this.CardId = card_id;
            this.Coordinate = coordinate;
            this.SourceAddress = source_address;
            this.FormattedAddress = formatted_address;
            this.Status = status;
        }

        public void Save(HelperDB helper)
        {
            switch (this.Type) {
                case Objects.PointType.CustomerEpicentrK: helper.SetEpicentrKPoint(this);
                    break;
                case Objects.PointType.CustomerNewLine: helper.SetNewLinePoint(this);
                    break;
            }            
        }
    }

    public class Coordinate
    {
        public string Lat { get; set; }
        public string Lng { get; set; }
        public Coordinate() { }

        public Coordinate(string lat, string lng)
        {
            this.Lat = lat;
            this.Lng = lng;
        }
    }
}
