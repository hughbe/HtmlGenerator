namespace HtmlGenerator
{
    public class HtmlDataAttribute : HtmlAttribute 
    {
        internal HtmlDataAttribute() : base("data", "Data", null, false, false) 
        {
        }

        internal HtmlDataAttribute(string value) : base("data", "Data", value, false, false) 
        {
        }
    }
}