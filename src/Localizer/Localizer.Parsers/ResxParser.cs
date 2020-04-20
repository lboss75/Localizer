using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Localizer.Parsers
{
    public class ResxParser
    {
        private readonly string file_path_;
        private readonly XDocument doc_;

        public ResxParser(string file_path)
        {
            this.file_path_ = file_path;
            this.doc_ = XDocument.Load(file_path);
        }

        static public ResxParser Load(string file_path)
        {
            return new ResxParser(file_path);
        }

        public bool TryGetText(string text, out string id)
        {
            foreach(var item in this.All())
            {
                if(item.Text == text)
                {
                    id = item.Id;
                    return true;
                }
            }

            id = null;
            return false;
        }

        public void AddText(string id, string text)
        {
            this.doc_.Root.Add(
                new XElement("data",
                    new XAttribute("name", id),
                    new XAttribute(XNamespace.Xml + "space", "preserve"),
                    new XElement("value", text)));
        }
        public class StringResource
        {
            private XElement item_;

            public StringResource(XElement item)
            {
                this.item_ = item;
            }

            public string Id { get => (string)this.item_.Attribute("name"); }
            public string Text { get => (string)this.item_.Element("value"); }
        }

        public IEnumerable<StringResource> All()
        {
            foreach(var item in this.doc_.Descendants("data"))
            {
                yield return new StringResource(item);
            }
        }

        public void Save()
        {
            this.doc_.Save(this.file_path_);
        }
    }
}
