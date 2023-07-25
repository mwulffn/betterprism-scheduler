using Odk.BluePrism;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Odk.Scheduler.Controllers
{    
    [EnableCors("*", "*", "*")]
    public abstract class BaseController<TKey, T> : ApiController
    {
        protected readonly IBluePrism bluePrism;
        protected readonly NLog.Logger logger;

        public BaseController(IBluePrism bluePrism)
        {
            this.bluePrism = bluePrism;
            this.logger = NLog.LogManager.GetCurrentClassLogger();
        }

        public virtual IEnumerable<T> Get()
        {
            throw new HttpResponseException(HttpStatusCode.NotImplemented);
        }

        public virtual T Get(TKey id)
        {
            throw new HttpResponseException(HttpStatusCode.NotImplemented);
        }

        public virtual T Put([FromBody] T obj)
        {
            return Post(obj);
        }

        public virtual T Put(TKey id, [FromBody] T obj)
        {
            throw new HttpResponseException(HttpStatusCode.NotImplemented);
        }

        public virtual T Post([FromBody] T obj)
        {
            throw new HttpResponseException(HttpStatusCode.NotImplemented);
        }

        public virtual T Delete(TKey id)
        {
            throw new HttpResponseException(HttpStatusCode.NotImplemented);
        }
    }
}