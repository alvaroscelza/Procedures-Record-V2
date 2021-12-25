using System.Text;

namespace ProceduresRecord.Web.MVC.Helpers
{
    public static class GenericFunctions
    {
        public static string ToLowerAndRemoveDiacritics(string text)
        {
            string loweredSearchValue = text.ToLower();
            byte[] tempBytes;
            tempBytes = Encoding.GetEncoding("ISO-8859-8").GetBytes(loweredSearchValue);
            return Encoding.UTF8.GetString(tempBytes);
        }
    }
}