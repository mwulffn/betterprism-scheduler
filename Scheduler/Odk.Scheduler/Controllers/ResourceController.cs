using Odk.BluePrism;
using System;
using System.Collections.Generic;

namespace Odk.Scheduler.Controllers
{
    public class ResourceController : BaseController<Guid, BPResource>
    {
        public ResourceController(IBluePrism bluePrism) : base(bluePrism)
        {

        }

        public override IEnumerable<BPResource> Get()
        {
            return bluePrism.AllResources();
        }

        public override BPResource Get(Guid id)
        {
            return bluePrism.GetResource(id);
        }
    }
}