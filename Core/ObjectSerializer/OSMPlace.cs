/* 
  Licensed under the Apache License, Version 2.0

  http://www.apache.org/licenses/LICENSE-2.0
  */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Core.ObjectSerializer
{
    [XmlRoot(ElementName = "place")]
    public class Place
    {
        [XmlElement(ElementName = "house_number")]
        public string House_number { get; set; }
        [XmlElement(ElementName = "road")]
        public string Road { get; set; }
        [XmlElement(ElementName = "village")]
        public string Village { get; set; }
        [XmlElement(ElementName = "town")]
        public string Town { get; set; }
        [XmlElement(ElementName = "city")]
        public string City { get; set; }
        [XmlElement(ElementName = "county")]
        public string County { get; set; }
        [XmlElement(ElementName = "postcode")]
        public string Postcode { get; set; }
        [XmlElement(ElementName = "country")]
        public string Country { get; set; }
        [XmlElement(ElementName = "country_code")]
        public string Country_code { get; set; }
        [XmlAttribute(AttributeName = "place_id")]
        public string Place_id { get; set; }
        [XmlAttribute(AttributeName = "osm_type")]
        public string Osm_type { get; set; }
        [XmlAttribute(AttributeName = "osm_id")]
        public string Osm_id { get; set; }
        [XmlAttribute(AttributeName = "boundingbox")]
        public string Boundingbox { get; set; }
        [XmlAttribute(AttributeName = "polygonpoints")]
        public string Polygonpoints { get; set; }
        [XmlAttribute(AttributeName = "lat")]
        public string Lat { get; set; }
        [XmlAttribute(AttributeName = "lon")]
        public string Lon { get; set; }
        [XmlAttribute(AttributeName = "display_name")]
        public string Display_name { get; set; }
        [XmlAttribute(AttributeName = "class")]
        public string Class { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
    }

    [XmlRoot(ElementName = "searchresults")]
    public class Searchresults
    {
        [XmlElement(ElementName = "place")]
        public Place Place { get; set; }
        [XmlAttribute(AttributeName = "timestamp")]
        public string Timestamp { get; set; }
        [XmlAttribute(AttributeName = "querystring")]
        public string Querystring { get; set; }
        [XmlAttribute(AttributeName = "polygon")]
        public string Polygon { get; set; }
    }

}