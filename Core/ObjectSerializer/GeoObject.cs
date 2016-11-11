/* 
 Licensed under the Apache License, Version 2.0
    
 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Core.ObjectSerializer
{
    [XmlRoot(ElementName = "GeocoderResponseMetaData", Namespace = "http://maps.yandex.ru/geocoder/1.x")]
    public class GeocoderResponseMetaData
    {
        [XmlElement(ElementName = "request", Namespace = "http://maps.yandex.ru/geocoder/1.x")]
        public string Request { get; set; }
        [XmlElement(ElementName = "found", Namespace = "http://maps.yandex.ru/geocoder/1.x")]
        public string Found { get; set; }
        [XmlElement(ElementName = "results", Namespace = "http://maps.yandex.ru/geocoder/1.x")]
        public string Results { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }

    [XmlRoot(ElementName = "metaDataProperty", Namespace = "http://www.opengis.net/gml")]
    public class MetaDataProperty
    {
        [XmlElement(ElementName = "GeocoderResponseMetaData", Namespace = "http://maps.yandex.ru/geocoder/1.x")]
        public GeocoderResponseMetaData GeocoderResponseMetaData { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlElement(ElementName = "GeocoderMetaData", Namespace = "http://maps.yandex.ru/geocoder/1.x")]
        public GeocoderMetaData GeocoderMetaData { get; set; }
    }

    [XmlRoot(ElementName = "Premise", Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
    public class Premise
    {
        [XmlElement(ElementName = "PremiseNumber", Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
        public string PremiseNumber { get; set; }
    }

    [XmlRoot(ElementName = "Thoroughfare", Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
    public class Thoroughfare
    {
        [XmlElement(ElementName = "ThoroughfareName", Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
        public string ThoroughfareName { get; set; }
        [XmlElement(ElementName = "Premise", Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
        public Premise Premise { get; set; }
    }

    [XmlRoot(ElementName = "Locality", Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
    public class Locality
    {
        [XmlElement(ElementName = "LocalityName", Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
        public string LocalityName { get; set; }
        [XmlElement(ElementName = "Thoroughfare", Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
        public Thoroughfare Thoroughfare { get; set; }
    }

    [XmlRoot(ElementName = "SubAdministrativeArea", Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
    public class SubAdministrativeArea
    {
        [XmlElement(ElementName = "SubAdministrativeAreaName", Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
        public string SubAdministrativeAreaName { get; set; }
        [XmlElement(ElementName = "Locality", Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
        public Locality Locality { get; set; }
    }

    [XmlRoot(ElementName = "AdministrativeArea", Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
    public class AdministrativeArea
    {
        [XmlElement(ElementName = "AdministrativeAreaName", Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
        public string AdministrativeAreaName { get; set; }
        [XmlElement(ElementName = "SubAdministrativeArea", Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
        public SubAdministrativeArea SubAdministrativeArea { get; set; }
    }

    [XmlRoot(ElementName = "Country", Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
    public class Country
    {
        [XmlElement(ElementName = "AddressLine", Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
        public string AddressLine { get; set; }
        [XmlElement(ElementName = "CountryNameCode", Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
        public string CountryNameCode { get; set; }
        [XmlElement(ElementName = "CountryName", Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
        public string CountryName { get; set; }
        [XmlElement(ElementName = "AdministrativeArea", Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
        public AdministrativeArea AdministrativeArea { get; set; }
    }

    [XmlRoot(ElementName = "AddressDetails", Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
    public class AddressDetails
    {
        [XmlElement(ElementName = "Country", Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
        public Country Country { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }

    [XmlRoot(ElementName = "GeocoderMetaData", Namespace = "http://maps.yandex.ru/geocoder/1.x")]
    public class GeocoderMetaData
    {
        [XmlElement(ElementName = "kind", Namespace = "http://maps.yandex.ru/geocoder/1.x")]
        public string Kind { get; set; }
        [XmlElement(ElementName = "text", Namespace = "http://maps.yandex.ru/geocoder/1.x")]
        public string Text { get; set; }
        [XmlElement(ElementName = "precision", Namespace = "http://maps.yandex.ru/geocoder/1.x")]
        public string Precision { get; set; }
        [XmlElement(ElementName = "AddressDetails", Namespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0")]
        public AddressDetails AddressDetails { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }

    [XmlRoot(ElementName = "description", Namespace = "http://www.opengis.net/gml")]
    public class Description
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "name", Namespace = "http://www.opengis.net/gml")]
    public class Name
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Envelope", Namespace = "http://www.opengis.net/gml")]
    public class Envelope
    {
        [XmlElement(ElementName = "lowerCorner", Namespace = "http://www.opengis.net/gml")]
        public string LowerCorner { get; set; }
        [XmlElement(ElementName = "upperCorner", Namespace = "http://www.opengis.net/gml")]
        public string UpperCorner { get; set; }
    }

    [XmlRoot(ElementName = "boundedBy", Namespace = "http://www.opengis.net/gml")]
    public class BoundedBy
    {
        [XmlElement(ElementName = "Envelope", Namespace = "http://www.opengis.net/gml")]
        public Envelope Envelope { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }

    [XmlRoot(ElementName = "Point", Namespace = "http://www.opengis.net/gml")]
    public class Point
    {
        [XmlElement(ElementName = "pos", Namespace = "http://www.opengis.net/gml")]
        public string Pos { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }

    [XmlRoot(ElementName = "GeoObject", Namespace = "http://maps.yandex.ru/ymaps/1.x")]
    public class GeoObject
    {
        [XmlElement(ElementName = "metaDataProperty", Namespace = "http://www.opengis.net/gml")]
        public MetaDataProperty MetaDataProperty { get; set; }
        [XmlElement(ElementName = "description", Namespace = "http://www.opengis.net/gml")]
        public Description Description { get; set; }
        [XmlElement(ElementName = "name", Namespace = "http://www.opengis.net/gml")]
        public Name Name { get; set; }
        [XmlElement(ElementName = "boundedBy", Namespace = "http://www.opengis.net/gml")]
        public BoundedBy BoundedBy { get; set; }
        [XmlElement(ElementName = "Point", Namespace = "http://www.opengis.net/gml")]
        public Point Point { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "gml", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Gml { get; set; }
        [XmlAttribute(AttributeName = "id", Namespace = "http://www.opengis.net/gml")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "featureMember", Namespace = "http://www.opengis.net/gml")]
    public class FeatureMember
    {
        [XmlElement(ElementName = "GeoObject", Namespace = "http://maps.yandex.ru/ymaps/1.x")]
        public GeoObject GeoObject { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }

    [XmlRoot(ElementName = "GeoObjectCollection", Namespace = "http://maps.yandex.ru/ymaps/1.x")]
    public class GeoObjectCollection
    {
        [XmlElement(ElementName = "metaDataProperty", Namespace = "http://www.opengis.net/gml")]
        public MetaDataProperty MetaDataProperty { get; set; }
        [XmlElement(ElementName = "featureMember", Namespace = "http://www.opengis.net/gml")]
        public FeatureMember FeatureMember { get; set; }
    }

    [XmlRoot(ElementName = "ymaps", Namespace = "http://maps.yandex.ru/ymaps/1.x")]
    public class Ymaps
    {
        [XmlElement(ElementName = "GeoObjectCollection", Namespace = "http://maps.yandex.ru/ymaps/1.x")]
        public GeoObjectCollection GeoObjectCollection { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string SchemaLocation { get; set; }
    }

}
