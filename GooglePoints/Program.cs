using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Core;
using System.Threading;
using NLog;
using System.Configuration;
using Newtonsoft.Json;
using CoreData;
using Core.Searchers;
using Core.ObjectSerializer;
using Core.Objects;

namespace GooglePoints
{
    class Program
    {
        //CoreGooglePoints cgp = new CoreGooglePoints();
        
        static void Main(string[] args)
        {
            WebClientExtended client = new WebClientExtended();
            string url = "http://nominatim.openstreetmap.org/search?q=Хмельницький вулиця Свободи 16а&format=xml";

            string st = System.Text.Encoding.UTF8.GetString(client.DownloadData(url));
            XmlSerializer serializer = new XmlSerializer(typeof(Searchresults));

            using (TextReader tr = new StringReader(st))
            {
                Searchresults result = (Searchresults)serializer.Deserialize(tr);
            }

            //Console.WriteLine(result);

            //WebRequest request = WebRequest.Create(
            //  //"http://nominatim.openstreetmap.org/search?q=івано-франківськ Гаркуші 17&format=xml"); //&addressdetails=1
            //  "http://nominatim.openstreetmap.org/search?q=Хмельницький вулиця Свободи 16а&format=xml");
            //// If required by the server, set the credentials.
            //request.Credentials = CredentialCache.DefaultCredentials;
            //request.Method = "GET";
            //// Get the response.
            //WebResponse response = request.GetResponse();
            //// Display the status.
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            //// Get the stream containing content returned by the server.
            //Stream dataStream = response.GetResponseStream();


            //XmlSerializer serializer = new XmlSerializer(typeof(Searchresults));
            ////Place resultingMessage = (Place)serializer.Deserialize(dataStream);

            //using (StreamReader reader = new StreamReader(dataStream, Encoding.UTF8))
            //{
            //    string xml = reader.ReadToEnd();
            //    using (TextReader tr = new StringReader(xml))
            //    {
            //        Searchresults result = (Searchresults)serializer.Deserialize(tr);
            //    }

            //    //    //Console.WriteLine(xml);
            //    //    //while (reader.Peek() >= 0)
            //    //    //{
            //    //    //string line = reader.ReadLine();
            //    //    //if (line.Contains("<pos>"))
            //    //    //{
            //    //    //    string[] position = line.Replace("<pos>", "").Replace("</pos>", "").Trim().Split(' ');
            //    //    //    Console.WriteLine("lat:" + position[1] + " lng:" + position[0]);
            //    //    //}

            //    //    //Console.WriteLine(reader.ReadLine());
            //    //    //}
            //}
            //String responseString = reader.ReadToEnd();

            Console.ReadLine();


            //XmlSerializer serializer = new XmlSerializer(typeof(GeoObject));
            //MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(responseFromServer));
            //GeoObject resultingMessage = (GeoObject)serializer.Deserialize(dataStream);

            //string place = resultingMessage.place[0].geojson;
            //response.Close();
            //PoligonPlace plg = (PoligonPlace)JsonConvert.DeserializeObject(place, typeof(PoligonPlace));

            //string poligon = string.Empty;

            //foreach(List<double> crd in plg.coordinates[0]) {
            //    poligon = poligon + string.Format("{0} {1}, ", 
            //        Convert.ToDecimal(crd[1]).ToString().Replace(',','.'),
            //        Convert.ToDecimal(crd[0]).ToString().Replace(',', '.')
            //        );
            //}
            //poligon = poligon.TrimEnd().Substring(0, poligon.TrimEnd().Length - 1);
            //DictEpicetnrK dict = new DictEpicetnrK();
            //long district_id = dict.SetDistrict(0, 2, "Хмельницький", poligon);

            //foreach (List<double> crd in plg.coordinates[0])
            //{
            //    dict.SetDistrictCoordinates(district_id, Convert.ToDecimal(crd[1]), Convert.ToDecimal(crd[0]));
            //}
            //Console.WriteLine(poligon);
            //Console.ReadLine();
        }
    }
}
