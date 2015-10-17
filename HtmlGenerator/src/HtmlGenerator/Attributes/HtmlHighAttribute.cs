namespace HtmlGenerator
{
    public class HtmlHighAttribute : HtmlAttribute 
    {
        internal HtmlHighAttribute() : base("high", "High", null, false, false) 
        {
        }

        internal HtmlHighAttribute(string value) : base("high", "High", value, false, false) 
        {
        }
    }
}