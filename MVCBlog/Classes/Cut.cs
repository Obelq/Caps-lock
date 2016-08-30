namespace WebsiteForAds.Classes
{
    public class Cut
    {
        public static string CutText(string text, int maxLength = 100)
        {
            if (text.Length < maxLength)
            {
                return text;
            }
            return text.Substring(0, maxLength) + "...";
        }
    }
}