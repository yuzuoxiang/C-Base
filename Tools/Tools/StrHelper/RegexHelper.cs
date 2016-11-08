using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tools.StrHelper
{
    /// <summary>
    /// 操作正则表达式的公共类
    /// </summary>
    public class RegexHelper
    {
        /// <summary>
        /// 使用正则替换字符串
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <param name="value">替换字符串</param>
        /// <returns></returns>
        public static string ReplaceStr(string input, string pattern, string value)
        {
            return ReplaceStr(input, pattern, value, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 使用正则替换字符串
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <param name="value">替换字符串</param>
        /// <param name="options">筛选条件</param>
        /// <returns></returns>
        public static string ReplaceStr(string input, string pattern, string value,RegexOptions options)
        {
            return Regex.Replace(input, pattern, value, options);
        }

        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <returns></returns>
        public static bool IsMatch(string input, string pattern)
        {
            return IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <param name="options">筛选条件</param>
        /// <returns></returns>
        public static bool IsMatch(string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options);
        }


    }
}
