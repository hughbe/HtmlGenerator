using System.Collections.Generic;

namespace HtmlGenerator
{
    public class HtmlStrongElement : HtmlElement
    {
        public HtmlStrongElement() : base("strong", false) 
        {    
        }

        public new HtmlStrongElement WithElement(HtmlElement element) => (HtmlStrongElement)base.WithElement(element);
        public new HtmlStrongElement WithElements(IEnumerable<HtmlElement> elements) => (HtmlStrongElement)base.WithElements(elements);

        public new HtmlStrongElement WithInnerText(string innerText) => (HtmlStrongElement)base.WithInnerText(innerText);

        public new HtmlStrongElement WithAttribute(HtmlAttribute attribute) => (HtmlStrongElement)base.WithAttribute(attribute);
        public new HtmlStrongElement WithAttributes(IEnumerable<HtmlAttribute> attributes) => (HtmlStrongElement)base.WithAttributes(attributes);

		public HtmlStrongElement WithAccessKey(string value) => WithAttribute(Attribute.AccessKey(value));

		public HtmlStrongElement WithClass(string value) => WithAttribute(Attribute.Class(value));

		public HtmlStrongElement WithContentEditable(string value) => WithAttribute(Attribute.ContentEditable(value));

		public HtmlStrongElement WithContextMenu(string value) => WithAttribute(Attribute.ContextMenu(value));

		public HtmlStrongElement WithDir(string value) => WithAttribute(Attribute.Dir(value));

		public HtmlStrongElement WithHidden(string value) => WithAttribute(Attribute.Hidden(value));

		public HtmlStrongElement WithId(string value) => WithAttribute(Attribute.Id(value));

		public HtmlStrongElement WithLang(string value) => WithAttribute(Attribute.Lang(value));

		public HtmlStrongElement WithSpellCheck(string value) => WithAttribute(Attribute.SpellCheck(value));

		public HtmlStrongElement WithStyle(string value) => WithAttribute(Attribute.Style(value));

		public HtmlStrongElement WithTabIndex(string value) => WithAttribute(Attribute.TabIndex(value));
    }
}
