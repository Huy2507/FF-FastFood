// Đường dẫn: Helpers/SlugHelper.cs
using System.Text;
using System.Text.RegularExpressions;

public static class SlugHelper
{
    public static string GenerateSlug(string phrase)
    {
        string str = RemoveAccent(phrase).ToLower();
        str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); // loại bỏ các ký tự không phải là chữ cái, chữ số hoặc dấu cách
        str = Regex.Replace(str, @"\s+", " ").Trim(); // thay thế nhiều khoảng trắng bằng một dấu cách và cắt bớt dấu cách ở đầu và cuối chuỗi
        str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim(); // cắt chuỗi nếu dài hơn 45 ký tự
        str = Regex.Replace(str, @"\s", "-"); // thay thế dấu cách bằng dấu gạch ngang
        return str;
    }

    private static string RemoveAccent(string txt)
    {
        byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
        return Encoding.ASCII.GetString(bytes);
    }
}
