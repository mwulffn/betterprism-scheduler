using System;

namespace Odk.BluePrism
{
    public class BPWorkqueue
    {
        public Guid Id { get; set; }
        public string Name { get; set; }    
        public bool Running { get; set; }
    }
}
