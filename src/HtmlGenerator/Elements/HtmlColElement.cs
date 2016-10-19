namespace HtmlGenerator
{
    public class HtmlColElement : HtmlElement
    {
        public HtmlColElement() : base("col", true)
        {
        }

        public HtmlColElement WithSpan(string value) => this.WithAttribute(Attribute.Span(value));
    }
}
