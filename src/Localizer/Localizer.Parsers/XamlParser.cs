using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Localizer.Parsers
{
    public class XamlParser
    {
        private readonly string file_path_;
        private readonly XDocument doc_;

        public XamlParser(string file_path)
        {
            this.file_path_ = file_path;
            this.doc_ = XDocument.Load(file_path);
        }

        static public XamlParser Load(string file_path)
        {
            return new XamlParser(file_path);
        }

        public IEnumerable<ITextBlock> TextBlocks()
        {
            foreach(var item in this.doc_.Root.DescendantsAndSelf())
            {
                if(item.Name.LocalName == "Label" && !string.IsNullOrWhiteSpace(item.Value))
                {
                    yield return new ElementText2Attribute(item, "Content");
                }
                if (item.Name.LocalName == "Button" && !string.IsNullOrWhiteSpace(item.Value))
                {
                    yield return new ElementText2Attribute(item, "Content");
                }
                if (item.Name.LocalName == "Window")
                {
                    yield return new Element2Attribute(item, "Title");
                }
            }

        }
        public void Save()
        {
            this.doc_.Save(this.file_path_);
        }
    }
}
