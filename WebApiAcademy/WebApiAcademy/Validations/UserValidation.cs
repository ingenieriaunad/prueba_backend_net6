namespace WebApiAcademy.Validations
{
    public class UserValidation
    {
       public static bool IsValidPassword(string password)
       {
            var regex = new System.Text.RegularExpressions.Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$");
            if(!regex.IsMatch(password))
            {
                return false;
            }
            return true;
       }
       public static bool IsValidEmail(string email)
       {
            var regex = new System.Text.RegularExpressions.Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if(!regex.IsMatch(email))
            {
                return false;
            }
            return true;
       }
    }
}
