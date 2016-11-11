using Core;
using CoreData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Core.Objects;

namespace CustomerPosition.Controllers
{
    public class CardPointController : ApiController
    {
        // GET api/cardpoint
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/cardpoint/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/cardpoint
        public void Post([FromBody]string value)
        {
        }

        // PUT api/cardpoint/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/cardpoint/5
        public void Delete(int id)
        {
        }

        public IEnumerable<CardPoint> GetPointsByMarketId(int id)
        {
            var parameters = Request.GetQueryNameValuePairs();
            var visit_count = parameters.Where(o => o.Key == "visit").FirstOrDefault();
            
            DictEpicetnrK dict = new DictEpicetnrK();
            return dict.GetPointsByMarketId(id, Convert.ToInt32(visit_count.Value));
        }
    }
}
