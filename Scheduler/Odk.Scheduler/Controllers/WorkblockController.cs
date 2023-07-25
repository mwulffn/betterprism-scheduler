using Odk.BluePrism;
using Odk.Scheduler.Database;
using Odk.Scheduler.Database.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace Odk.Scheduler.Controllers
{
    [RoutePrefix("api/workblock")]
    public class WorkblockController : BaseController<Guid, Workblock>
    {
        private readonly IWorkblockRepository workblockRepository;

        public WorkblockController(IBluePrism bluePrism, IWorkblockRepository workblockRepository) : base(bluePrism)
        {
            this.workblockRepository = workblockRepository;
        }

        public override IEnumerable<Workblock> Get()
        {
            var blocks = workblockRepository.All();

            foreach (var block in blocks)
                block.ProcessName = bluePrism.GetProcess(block.ProcessId)?.Name ?? "Unknown process";

            return blocks;
        }

        public override Workblock Get(Guid id)
        {
            var block = workblockRepository.Single(id);
            block.ProcessName = bluePrism.GetProcess(block.ProcessId)?.Name ?? "Unknown process";

            return block;
        }

        public override Workblock Post([FromBody] Workblock obj)
        {
            var process = bluePrism.GetProcess(obj.ProcessId);

            if (process == null)
                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            if (obj.PostCompletionDelay < 0)
                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            obj.WorkblockId = Guid.NewGuid();
            workblockRepository.Insert(obj);
            obj.ProcessName = bluePrism.GetProcess(obj.ProcessId)?.Name ?? "Unknown process";

            return obj;
        }

        public override Workblock Put(Guid id, [FromBody] Workblock obj)
        {
            var process = bluePrism.GetProcess(obj.ProcessId);

            if (process == null)
                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            if (obj.PostCompletionDelay < 0)
                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            workblockRepository.Save(obj);
            obj.ProcessName = bluePrism.GetProcess(obj.ProcessId)?.Name ?? "Unknown process";

            return obj;
        }

        public override Workblock Delete(Guid id)
        {
            var workblock = workblockRepository.Single(id);

            if (workblockRepository.IsWorkblockInUse(workblock))
                throw new HttpResponseException(HttpStatusCode.Forbidden);

            workblock.Deleted = true;
            workblockRepository.Save(workblock);

            return workblock;
        }

        [Route("inuse/{id}")]
        [HttpGet]
        public bool InUse(Guid id)
        {
            var workblock = workblockRepository.Single(id);
            return workblockRepository.IsWorkblockInUse(workblock);
        }
    }
}