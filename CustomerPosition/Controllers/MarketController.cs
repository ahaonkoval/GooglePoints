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
    public class MarketController : ApiController
    {
        // GET api/market
        public IEnumerable<Market> Get()
        {
            DictEpicetnrK dict = new DictEpicetnrK();
            return dict.GetMarkets();
        }

        // GET api/market/5
        public Market Get(int id)
        {
            DictEpicetnrK dict = new DictEpicetnrK();
            return dict.GetMarketById(id);
        }

        // POST api/market
        public void Post([FromBody]string value)
        {
        }

        // PUT api/market/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/market/5
        public void Delete(int id)
        {
        }
    }
}
