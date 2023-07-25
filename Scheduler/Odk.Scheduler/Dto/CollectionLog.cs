using System.Collections.Generic;

namespace Odk.Scheduler.Dto
{
    public class CollectionLog
    {
        public string Name { get; set; }
        public string[] Fields { get; set; }
        public IEnumerable<string[]> Rows { get; set; }
    }
}