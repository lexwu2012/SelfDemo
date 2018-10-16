using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MutipleNetDemo.Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// 使用 string.Split 方法以指定的分隔符分隔字符串
        /// </summary>
        /// <param name="str">要分隔的字符串</param>
        /// <param name="separator">分隔符</param>
        /// <param name="options">分隔选项</param>
        public static string[] Split(this string str, string separator, StringSplitOptions options)
        {
            return str?.Split(new[] { separator }, options);
        }

        /// <summary>
        /// 指示指定的字符串是 null 还是空字符串。
        /// </summary>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }
}
