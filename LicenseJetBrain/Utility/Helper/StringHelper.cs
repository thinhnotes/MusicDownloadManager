using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Utility;

namespace Utility.Helper
{
    public class StringHelper
    {
        /// <summary>
        ///     lấy username dạng viêt tắt của tên họ
        /// </summary>
        /// <param name="fullname">tên đầy đủ</param>
        /// <returns>username theo chuẩn viết tắt</returns>
        public static string GetUsername(string fullname)
        {
            fullname = fullname.RemoveVnChar();
            string[] sp = fullname.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string last = sp.Last();
            string mid = sp.Skip(1).FirstOrDefault();

            return (mid + last).ToLower();
        }
    }
}
