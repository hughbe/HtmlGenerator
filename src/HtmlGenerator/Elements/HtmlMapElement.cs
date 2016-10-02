using System.Collections.Generic;

namespace HtmlGenerator
{
    public class HtmlMapElement : HtmlElement
    {
        public HtmlMapElement() : base("map", false) 
        {    
        }

        public new HtmlMapElement WithElement(HtmlElement element) => (HtmlMapElement)base.WithElement(element);
        public new HtmlMapElement WithElements(IEnumerable<HtmlElement> elements) => (HtmlMapElement)base.WithElements(elements);

        public new HtmlMapElement WithInnerText(string innerText) => (HtmlMapElement)base.WithInnerText(innerText);

        public new HtmlMapElement WithAttribute(HtmlAttribute attribute) => (HtmlMapElement)base.WithAttribute(attribute);
        public new HtmlMapElement WithAttributes(IEnumerable<HtmlAttribute> attributes) => (HtmlMapElement)base.WithAttributes(attributes);

		public HtmlMapElement WithName(string value) => WithAttribute(Attribute.Name(value));

		public HtmlMapElement WithAccessKey(string value) => WithAttribute(Attribute.AccessKey(value));

		public HtmlMapElement WithClass(string value) => WithAttribute(Attribute.Class(value));

		public HtmlMapElement WithContentEditable(string value) => WithAttribute(Attribute.ContentEditable(value));

		public HtmlMapElement WithContextMenu(string value) => WithAttribute(Attribute.ContextMenu(value));

		public HtmlMapElement WithDir(string value) => WithAttribute(Attribute.Dir(value));

		public HtmlMapElement WithHidden(string value) => WithAttribute(Attribute.Hidden(value));

		public HtmlMapElement WithId(string value) => WithAttribute(Attribute.Id(value));

		public HtmlMapElement WithLang(string value) => WithAttribute(Attribute.Lang(value));

		public HtmlMapElement WithSpellCheck(string value) => WithAttribute(Attribute.SpellCheck(value));

		public HtmlMapElement WithStyle(string value) => WithAttribute(Attribute.Style(value));

		public HtmlMapElement WithTabIndex(string value) => WithAttribute(Attribute.TabIndex(value));
    }
}
