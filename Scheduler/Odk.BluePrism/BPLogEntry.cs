using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Odk.BluePrism
{
    public class BPLogEntry
    {
        public int LogId { get; set; }
        public int SessionNumber { get; set; }
        public Guid StageId { get; set; }
        public string StageName { get; set; }
        public int StageType { get; set; }
        public string ProcessName { get; set; }
        public string PageName { get; set; }
        public string ObjectName { get; set; }
        public string ActionName { get; set; }
        public string Result { get; set; }
        public int ResultType { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string AttributeXml { get; set; }

        public IEnumerable<BPLogParameter> Inputs
        {
            get
            {
                if (string.IsNullOrEmpty(AttributeXml))
                    return new List<BPLogParameter>();

                var doc = XDocument.Parse(AttributeXml);
                var result = new List<BPLogParameter>();

                foreach (var p in doc.Descendants(XName.Get("input")))
                {
                    var parameter = new BPLogParameter()
                    {
                        Name = p.Attribute("name").Value,
                        Value = p.Attribute("value")?.Value ?? "",
                        Type = p.Attribute("type").Value
                    };

                    if (parameter.Type == "collection")
                    {
                        var rows = p.Descendants(XName.Get("row"));
                        parameter.Value = rows.Count() + (rows.Count() == 1 ? " row" : " rows");
                    }

                    result.Add(parameter);
                }

                return result;
            }
        }

        public IEnumerable<BPLogParameter> Outputs
        {
            get
            {
                if (string.IsNullOrEmpty(AttributeXml))
                    return new List<BPLogParameter>();

                var doc = XDocument.Parse(AttributeXml);
                var result = new List<BPLogParameter>();
                foreach (var p in doc.Descendants(XName.Get("output")))
                {
                    var parameter = new BPLogParameter()
                    {
                        Name = p.Attribute("name").Value,
                        Value = p.Attribute("value")?.Value ?? "",
                        Type = p.Attribute("type").Value
                    };

                    if (parameter.Type == "collection")
                    {
                        var rows = p.Descendants(XName.Get("row"));
                        parameter.Value = rows.Count() + (rows.Count() == 1 ? " row" : " rows");
                    }

                    result.Add(parameter);
                }

                return result;
            }
        }

        public XElement RawInput(string name)
        {
            var doc = XDocument.Parse(AttributeXml);
            return doc.Descendants(XName.Get("input")).Single(a=> a.Attribute("name").Value == name);
        }

        public XElement RawOutput(string name)
        {
            var doc = XDocument.Parse(AttributeXml);
            return doc.Descendants(XName.Get("output")).Single(a => a.Attribute("name").Value == name);
        }
    }
}