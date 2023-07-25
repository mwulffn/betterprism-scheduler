using Odk.BluePrism;
using Odk.Scheduler.Database;
using Odk.Scheduler.Dto;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Xml.Linq;

namespace Odk.Scheduler.Controllers
{
    [RoutePrefix("api/sessionlog")]
    public class SessionLogController : BaseController<Guid, BPLogEntry>
    {
        private readonly ISessionRepository sessionRepository;
        private readonly ITaskRepository taskRepository;

        public SessionLogController(IBluePrism bluePrism, ISessionRepository sessionRepository, ITaskRepository taskRepository) : base(bluePrism)
        {
            this.sessionRepository = sessionRepository;
            this.taskRepository = taskRepository;
        }

        [HttpGet]
        [Route("{id}/page")]
        public Page<BPLogEntry> Page(Guid id, int page)
        {
            var bpPage = bluePrism.GetLogPage(id, page, 100);

            return bpPage;
        }

        [HttpGet]
        [Route("{id}/parameter")]
        public CollectionLog Parameter(int id, [FromUri] string direction, [FromUri] string parameter)
        {
            var result = new CollectionLog();
            var logEntry = bluePrism.GetLogEntry(id);
            XElement element;

            switch (direction)
            {
                case "input":
                    element = logEntry.RawInput(parameter);
                    break;
                case "output":
                    element = logEntry.RawOutput(parameter);
                    break;
                default:
                    throw new InvalidOperationException("Wrong parameter");
            }

            var rows = element.Descendants(XName.Get("row"));
            result.Name = element.Attribute("name").Value;

            //Do the headers
            result.Fields = rows.First().Descendants(XName.Get("field")).Select(a => a.Attribute("name").Value).ToArray();

            List<string[]> dtorows = new List<string[]>();

            foreach (var row in rows)
            {
                dtorows.Add(row.Descendants(XName.Get("field")).Select(a => a.Attribute("value").Value).ToArray());
            }

            result.Rows = dtorows;

            return result;
        }
    }
}