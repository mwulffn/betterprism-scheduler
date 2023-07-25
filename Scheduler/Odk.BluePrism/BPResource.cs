using System;

namespace Odk.BluePrism
{
    public class BPResource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FQDN { get; set; }
        public string DisplayStatus { get; set; }
    }
}
