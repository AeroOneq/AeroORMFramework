
namespace AeroORMFramework.CustomTypes
{
    public class Email
    {
        private string email;
        public string Value
        {
            get => email;
            set
            {
                if (value.Length == 0 || value.Length > 255)
                {
                    throw new NotAppropriateParamException("The length of email must be " +
                        "greater then 0 and equal or less then 255");
                }
                email = value;
            }
        }
    }
}
