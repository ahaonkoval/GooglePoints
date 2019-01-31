using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nominatim.API.Web;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {

            Dictionary<string, string> source = new Dictionary<string, string>();

            source.Add("format", "1");
            source.Add("key", "123");
            source.Add("accept-language", "ua");
            source.Add("addressdetails", "sdf4 sdgfdsf");
            source.Add("namedetails", "detail");
            source.Add("extratags", "ext");
            source.Add("email", "email");

            string sc = StringFormatExtensions.NamedFormat("https:\\nomi", source);

            Console.WriteLine(sc);

            //Console.ReadLine();


        }
    }
}
