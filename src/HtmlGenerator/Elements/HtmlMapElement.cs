namespace HtmlGenerator
{
    public class HtmlMapElement : HtmlElement
    {
        public HtmlMapElement() : base("map", false) 
        {    
        }

		public HtmlMapElement WithName(string value) => this.WithAttribute(Attribute.Name(value));
    }
}
