namespace HtmlGenerator
{
    public class HtmlXmlsAttribute : HtmlAttribute 
    {
        internal HtmlXmlsAttribute() : base("xmls", "Xmls", null, false, false) 
        {
        }

        internal HtmlXmlsAttribute(string value) : base("xmls", "Xmls", value, false, false) 
        {
        }
    }
}