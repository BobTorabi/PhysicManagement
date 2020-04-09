using System.Text.RegularExpressions;

namespace PhysicManagement.Common
{
    public class Validate
    {
        public static bool IsPersianText(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            return Regex.IsMatch(name??"", @"^[\u0600-\u06FF 0-9۰-۹_.-]+$", RegexOptions.IgnoreCase);
        }
        public static bool IsPersianName(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            return Regex.IsMatch(name, @"^[\u0600-\u06FF ]+$", RegexOptions.IgnoreCase);
        }
        
        public static bool IsText(string name)
        {
            return Regex.IsMatch(name??"", @"^[a-zA-Z0-9_.-\\s]*$", RegexOptions.IgnoreCase);
        }

        public static bool IsEmail(string email)
        {
            return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        public static bool IsPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return false;
            return Regex.IsMatch(phone, @"^0\d{2}\d{8}$", RegexOptions.IgnoreCase);
        }

        public static bool IsMobile(string mobile)
        {
            if (string.IsNullOrEmpty(mobile)) return false;

            if (mobile.StartsWith("09"))
            {
                return (mobile.Length == 11);
            }
            else
            {
                return false;
            }
        }

        public static bool IsNationalCode(string nationalCode)
        {
            if (string.IsNullOrWhiteSpace(nationalCode)) return false;
            if (nationalCode.Length != 10) return false;
            switch (nationalCode)
            {
                case "0000000000":
                case "1111111111":
                case "2222222222":
                case "3333333333":
                case "4444444444":
                case "5555555555":
                case "6666666666":
                case "7777777777":
                case "8888888888":
                case "9999999999":
                    return false;
                default:
                    int code = 0;
                    char ch;
                    for (int i = 0; i < 9; i++)
                    {
                        ch = nationalCode[i];
                        if (ch < '0') return false;
                        if (ch > '9') return false;

                        int v = ch - 48;
                        code += v * (10 - i);
                    }
                    int r = code % 11;
                    if (r > 1) r = 11 - r;
                    ch = nationalCode[9];
                    if (r == (ch - 48)) return true;
                    break;
            }

            return false;
        }
    }
}
