using System.Collections.Generic;

namespace HtmlGenerator
{
    public class HtmlH1Element : HtmlElement
    {
        public HtmlH1Element() : base("h1", false) 
        {    
        }

        public new HtmlH1Element WithElement(HtmlElement element) => (HtmlH1Element)base.WithElement(element);
        public new HtmlH1Element WithElements(IEnumerable<HtmlElement> elements) => (HtmlH1Element)base.WithElements(elements);

        public new HtmlH1Element WithInnerText(string innerText) => (HtmlH1Element)base.WithInnerText(innerText);

        public new HtmlH1Element WithAttribute(HtmlAttribute attribute) => (HtmlH1Element)base.WithAttribute(attribute);
        public new HtmlH1Element WithAttributes(IEnumerable<HtmlAttribute> attributes) => (HtmlH1Element)base.WithAttributes(attributes);

		public HtmlH1Element WithAccessKey(string value) => WithAttribute(Attribute.AccessKey(value));

		public HtmlH1Element WithClass(string value) => WithAttribute(Attribute.Class(value));

		public HtmlH1Element WithContentEditable(string value) => WithAttribute(Attribute.ContentEditable(value));

		public HtmlH1Element WithContextMenu(string value) => WithAttribute(Attribute.ContextMenu(value));

		public HtmlH1Element WithDir(string value) => WithAttribute(Attribute.Dir(value));

		public HtmlH1Element WithHidden(string value) => WithAttribute(Attribute.Hidden(value));

		public HtmlH1Element WithId(string value) => WithAttribute(Attribute.Id(value));

		public HtmlH1Element WithLang(string value) => WithAttribute(Attribute.Lang(value));

		public HtmlH1Element WithSpellCheck(string value) => WithAttribute(Attribute.SpellCheck(value));

		public HtmlH1Element WithStyle(string value) => WithAttribute(Attribute.Style(value));

		public HtmlH1Element WithTabIndex(string value) => WithAttribute(Attribute.TabIndex(value));
    }
}
