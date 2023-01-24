using System;
using System.Text.RegularExpressions;

namespace FarmAdvisor.Commons;
public static class Utils
{

  public static bool isValidPhone(string number)
  {
    if (number != null) return Regex.IsMatch(number, @"^([\+]?251[-]?|[0])?[1-9][0-9]{8}$");
    else return false;
  }
  public static bool isValidEmail(string email)
  {
    if (email != null) return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$");
    else return false;
  }
}