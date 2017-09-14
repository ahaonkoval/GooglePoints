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
    public class RegionController : ApiController
    {
        // GET api/googledata
        public IEnumerable<DataModels.DictRegions> Get()
        {
            using (DictGeoData dict = new DictGeoData())
                return dict.GetRegions();
        }

        // GET api/googledata/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/googledata
        public void Post([FromBody]string value)
        {
        }

        // PUT api/googledata/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/googledata/5
        public void Delete(int id)
        {
        }
    }
}
