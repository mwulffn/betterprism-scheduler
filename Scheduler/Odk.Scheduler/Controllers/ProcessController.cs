using Odk.BluePrism;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace Odk.Scheduler.Controllers
{
    public class ProcessController : BaseController<Guid, BPProcess>
    {
        public ProcessController(IBluePrism bluePrism) : base(bluePrism)
        {

        }

        public override IEnumerable<BPProcess> Get()
        {
            return bluePrism.ListProcesses();
        }

        public override BPProcess Get(Guid id)
        {
            var process = bluePrism.GetProcess(id);

            if (process == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return process;
        }
    }
}