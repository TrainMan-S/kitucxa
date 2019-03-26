namespace WebApp.Common
{
    public class CharCommon
    {
        // Get url alias
        public static string GetFilterUrl(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            else
            {
                //Xóa khoảng trắng đầu và cuối
                var slug = input.Trim();

                //Đổi chữ hoa thành chữ thường
                slug = input.ToLower();

                //Đổi ký tự có dấu thành không dấu
                slug = slug.Replace('á', 'a');
                slug = slug.Replace('à', 'a');
                slug = slug.Replace('ả', 'a');
                slug = slug.Replace('ạ', 'a');
                slug = slug.Replace('ã', 'a');
                slug = slug.Replace('ă', 'a');
                slug = slug.Replace('ắ', 'a');
                slug = slug.Replace('ằ', 'a');
                slug = slug.Replace('ẳ', 'a');
                slug = slug.Replace('ẵ', 'a');
                slug = slug.Replace('ặ', 'a');
                slug = slug.Replace('â', 'a');
                slug = slug.Replace('ấ', 'a');
                slug = slug.Replace('ầ', 'a');
                slug = slug.Replace('ẩ', 'a');
                slug = slug.Replace('ẫ', 'a');
                slug = slug.Replace('ậ', 'a');

                slug = slug.Replace('é', 'e');
                slug = slug.Replace('è', 'e');
                slug = slug.Replace('ẻ', 'e');
                slug = slug.Replace('ẽ', 'e');
                slug = slug.Replace('ẹ', 'e');
                slug = slug.Replace('ê', 'e');
                slug = slug.Replace('ế', 'e');
                slug = slug.Replace('ề', 'e');
                slug = slug.Replace('ể', 'e');
                slug = slug.Replace('ễ', 'e');
                slug = slug.Replace('ệ', 'e');

                slug = slug.Replace('i', 'i');
                slug = slug.Replace('í', 'i');
                slug = slug.Replace('ì', 'i');
                slug = slug.Replace('ỉ', 'i');
                slug = slug.Replace('ĩ', 'i');
                slug = slug.Replace('ị', 'i');

                slug = slug.Replace('ó', 'o');
                slug = slug.Replace('ò', 'o');
                slug = slug.Replace('ỏ', 'o');
                slug = slug.Replace('õ', 'o');
                slug = slug.Replace('ọ', 'o');
                slug = slug.Replace('ô', 'o');
                slug = slug.Replace('ố', 'o');
                slug = slug.Replace('ồ', 'o');
                slug = slug.Replace('ổ', 'o');
                slug = slug.Replace('ỗ', 'o');
                slug = slug.Replace('ộ', 'o');
                slug = slug.Replace('ơ', 'o');
                slug = slug.Replace('ớ', 'o');
                slug = slug.Replace('ờ', 'o');
                slug = slug.Replace('ở', 'o');
                slug = slug.Replace('ỡ', 'o');
                slug = slug.Replace('ợ', 'o');

                slug = slug.Replace('ú', 'u');
                slug = slug.Replace('ù', 'u');
                slug = slug.Replace('ủ', 'u');
                slug = slug.Replace('ũ', 'u');
                slug = slug.Replace('ụ', 'u');
                slug = slug.Replace('ư', 'u');
                slug = slug.Replace('ứ', 'u');
                slug = slug.Replace('ừ', 'u');
                slug = slug.Replace('ử', 'u');
                slug = slug.Replace('ữ', 'u');
                slug = slug.Replace('ự', 'u');

                slug = slug.Replace('ý', 'y');
                slug = slug.Replace('ỳ', 'y');
                slug = slug.Replace('ỷ', 'y');
                slug = slug.Replace('ỹ', 'y');
                slug = slug.Replace('ỵ', 'y');

                slug = slug.Replace('đ', 'd');

                //Xóa các ký tự đặt biệt
                slug = slug.Replace("?", "");
                slug = slug.Replace("&", "");
                slug = slug.Replace(",", "");
                slug = slug.Replace(":", "");
                slug = slug.Replace("!", "");
                slug = slug.Replace("'", "");
                slug = slug.Replace("\"", "");
                slug = slug.Replace("%", "");
                slug = slug.Replace("#", "");
                slug = slug.Replace("$", "");
                slug = slug.Replace("*", "");
                slug = slug.Replace("`", "");
                slug = slug.Replace("~", "");
                slug = slug.Replace("@", "");
                slug = slug.Replace("^", "");
                slug = slug.Replace(".", "");
                slug = slug.Replace("/", "");
                slug = slug.Replace(">", "");
                slug = slug.Replace("<", "");
                slug = slug.Replace("[", "");
                slug = slug.Replace("]", "");
                slug = slug.Replace(";", "");
                slug = slug.Replace("+", "");

                //Đổi khoảng trắng thành ký tự gạch ngang
                slug = slug.Replace(" ", "-");

                //Đổi nhiều ký tự gạch ngang liên tiếp thành 1 ký tự gạch ngang
                slug = slug.Replace("-----", "-");
                slug = slug.Replace("----", "-");
                slug = slug.Replace("---", "-");
                slug = slug.Replace("--", "-");

                return slug;
            }
        }

        public static string GetFolderName(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            else
            {
                //Xóa khoảng trắng đầu và cuối
                var slug = input.Trim();

                //Đổi chữ hoa thành chữ thường
                slug = input.ToLower();

                //Đổi ký tự có dấu thành không dấu
                slug = slug.Replace('á', 'a');
                slug = slug.Replace('à', 'a');
                slug = slug.Replace('ả', 'a');
                slug = slug.Replace('ạ', 'a');
                slug = slug.Replace('ã', 'a');
                slug = slug.Replace('ă', 'a');
                slug = slug.Replace('ắ', 'a');
                slug = slug.Replace('ằ', 'a');
                slug = slug.Replace('ẳ', 'a');
                slug = slug.Replace('ẵ', 'a');
                slug = slug.Replace('ặ', 'a');
                slug = slug.Replace('â', 'a');
                slug = slug.Replace('ấ', 'a');
                slug = slug.Replace('ầ', 'a');
                slug = slug.Replace('ẩ', 'a');
                slug = slug.Replace('ẫ', 'a');
                slug = slug.Replace('ậ', 'a');

                slug = slug.Replace('é', 'e');
                slug = slug.Replace('è', 'e');
                slug = slug.Replace('ẻ', 'e');
                slug = slug.Replace('ẽ', 'e');
                slug = slug.Replace('ẹ', 'e');
                slug = slug.Replace('ê', 'e');
                slug = slug.Replace('ế', 'e');
                slug = slug.Replace('ề', 'e');
                slug = slug.Replace('ể', 'e');
                slug = slug.Replace('ễ', 'e');
                slug = slug.Replace('ệ', 'e');

                slug = slug.Replace('i', 'i');
                slug = slug.Replace('í', 'i');
                slug = slug.Replace('ì', 'i');
                slug = slug.Replace('ỉ', 'i');
                slug = slug.Replace('ĩ', 'i');
                slug = slug.Replace('ị', 'i');

                slug = slug.Replace('ó', 'o');
                slug = slug.Replace('ò', 'o');
                slug = slug.Replace('ỏ', 'o');
                slug = slug.Replace('õ', 'o');
                slug = slug.Replace('ọ', 'o');
                slug = slug.Replace('ô', 'o');
                slug = slug.Replace('ố', 'o');
                slug = slug.Replace('ồ', 'o');
                slug = slug.Replace('ổ', 'o');
                slug = slug.Replace('ỗ', 'o');
                slug = slug.Replace('ộ', 'o');
                slug = slug.Replace('ơ', 'o');
                slug = slug.Replace('ớ', 'o');
                slug = slug.Replace('ờ', 'o');
                slug = slug.Replace('ở', 'o');
                slug = slug.Replace('ỡ', 'o');
                slug = slug.Replace('ợ', 'o');

                slug = slug.Replace('ú', 'u');
                slug = slug.Replace('ù', 'u');
                slug = slug.Replace('ủ', 'u');
                slug = slug.Replace('ũ', 'u');
                slug = slug.Replace('ụ', 'u');
                slug = slug.Replace('ư', 'u');
                slug = slug.Replace('ứ', 'u');
                slug = slug.Replace('ừ', 'u');
                slug = slug.Replace('ử', 'u');
                slug = slug.Replace('ữ', 'u');
                slug = slug.Replace('ự', 'u');

                slug = slug.Replace('ý', 'y');
                slug = slug.Replace('ỳ', 'y');
                slug = slug.Replace('ỷ', 'y');
                slug = slug.Replace('ỹ', 'y');
                slug = slug.Replace('ỵ', 'y');

                slug = slug.Replace('đ', 'd');

                //Xóa các ký tự đặt biệt
                slug = slug.Replace("?", "");
                slug = slug.Replace("&", "");
                slug = slug.Replace(",", "");
                slug = slug.Replace(":", "");
                slug = slug.Replace("!", "");
                slug = slug.Replace("'", "");
                slug = slug.Replace("\"", "");
                slug = slug.Replace("%", "");
                slug = slug.Replace("#", "");
                slug = slug.Replace("$", "");
                slug = slug.Replace("*", "");
                slug = slug.Replace("`", "");
                slug = slug.Replace("~", "");
                slug = slug.Replace("@", "");
                slug = slug.Replace("^", "");
                slug = slug.Replace(".", "");
                slug = slug.Replace("/", "");
                slug = slug.Replace(">", "");
                slug = slug.Replace("<", "");
                slug = slug.Replace("[", "");
                slug = slug.Replace("]", "");
                slug = slug.Replace(";", "");
                slug = slug.Replace("+", "");

                //Đổi khoảng trắng thành ký tự gạch ngang
                slug = slug.Replace(" ", "");

                //Đổi nhiều ký tự gạch ngang liên tiếp thành 1 ký tự gạch ngang
                slug = slug.Replace("-----", "");
                slug = slug.Replace("----", "");
                slug = slug.Replace("---", "");
                slug = slug.Replace("--", "");

                return slug;
            }
        }



    }
}
