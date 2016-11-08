﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Tools.WebHelper
{
    /// <summary>
    /// 
    /// </summary>
    public class QueryString
    {
        /// <summary>
        /// 等于Request.QueryString;如果为null 返回 空“” ，否则返回Request.QueryString[name]
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string Q(string name)
        {
            return Request.QueryString[name] == null ? "" : Request.QueryString[name];
        }

        /// <summary>
        /// 等于Request.Form如果为null 返回 空“” ，否则返回 Request.Form[name]
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string FormRequest(string name)
        {
            return Request.Form[name] == null ? "" : Request.Form[name];
        }

        /// <summary>
        /// 获取url中的id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int QId(string name)
        {
            return StrToId(Q(name));
        }

        /// <summary>
        /// 获取正确的Id，如果不是正整数，返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns>返回正确的整数ID，失败返回0</returns>
        public static int StrToId(string value)
        {
            if (IsNumberId(value))
            {
                return int.Parse(value);
            }
            else 
            {
                return 0;
            }
        }

        /// <summary>
        /// 检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证。
        /// </summary>
        /// <param name="value">需验证的字符串</param>
        /// <returns>是否合法的bool值</returns>
        public static bool IsNumberId(string value)
        {
            return Quickvalidate("^[1-9]*[0-9]*$", value);
        }

        /// <summary>
        /// 快速验证一个字符串是否符合指定的正则表达式。
        /// </summary>
        /// <param name="express">正则表达式的内容</param>
        /// <param name="value">需验证的字符串</param>
        /// <returns>是否合法的bool值</returns>
        public static bool Quickvalidate(string express, string value)
        {
            if (value == null)
            {
                return false;
            }
            Regex regex = new Regex(express);
            if (value.Length == 0)
            {
                return false;
            }

            return regex.IsMatch(value);
        }

        #region 类内部调用
        public static HttpContext Current 
        {
            get { return HttpContext.Current; } 
        }

        public static HttpRequest Request
        { 
            get { return Current.Request; } 
        }

        public static HttpResponse Response 
        {
            get { return Current.Response; }
        }
        #endregion
    }
}
