using System.Xml.Linq;

namespace Localizer.Parsers
{
    internal class ElementText2Attribute : ITextBlock
    {
        private readonly XElement element_;
        private readonly string attribute_;

        public ElementText2Attribute(XElement item, string attribute)
        {
            this.element_ = item;
            this.attribute_ = attribute;
        }

        public string Text
        {
            get => this.element_.Value; set
            {
                var a = this.element_.Attribute(this.attribute_);
                if (null != a)
                {
                    a.Value = value;
                }
                else
                {
                    this.element_.Value = string.Empty;
                    this.element_.Add(new XAttribute(this.attribute_, value));
                }
            }
        }
    }
}