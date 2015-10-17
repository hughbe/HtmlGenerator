namespace HtmlGenerator
{
    public class HtmlAutoPlayAttribute : HtmlAttribute 
    {
        internal HtmlAutoPlayAttribute() : base("autoplay", "AutoPlay", null, false, false) 
        {
        }

        internal HtmlAutoPlayAttribute(string value) : base("autoplay", "AutoPlay", value, false, false) 
        {
        }
    }
}