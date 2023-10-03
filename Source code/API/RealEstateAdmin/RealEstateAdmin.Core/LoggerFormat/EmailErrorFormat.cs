namespace RealEstateAdmin.Core.LoggerFormat
{
    public static class EmailErrorFormat
    {
        public static string Parse(string email, string subject, Exception e)
        {
            return $"Email Error => Email : {email} , Subject : {subject} ,Error : {e} ";
        }
    }
}
