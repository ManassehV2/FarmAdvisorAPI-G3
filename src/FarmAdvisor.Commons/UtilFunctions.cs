using System.Text.RegularExpressions;

namespace FarmAdvisor.Commons;
public static class Utils
{

    public static bool isValidPhone(string phone)
    {
        if (phone != null) return Regex.IsMatch(phone, @"^([\+]?251[-]?|[0])?[1-9][0-9]{8}$", RegexOptions.None, TimeSpan.FromMilliseconds(5000));
        else return false;
    }
    public static bool isValidEmail(string email)
    {
        if (email != null) return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$", RegexOptions.None, TimeSpan.FromMilliseconds(5000));
        else return false;
    }
    public static bool isValidLatitude(double number)
    {
        return number >= -90 && number <= 90;
    }
    public static bool isValidLongitude(double number)
    {
        return number >= -180 && number <= 180;
    }
    public static double getGdd(double tMin, double tMax, double tBase)
    {
        return ((tMin + tMax) / 2) - tBase;
    }
}