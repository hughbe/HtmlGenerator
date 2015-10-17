namespace HtmlGenerator
{
    public class HtmlListAttribute : HtmlAttribute 
    {
        internal HtmlListAttribute() : base("list", "List", null, false, false) 
        {
        }

        internal HtmlListAttribute(string value) : base("list", "List", value, false, false) 
        {
        }
    }
}