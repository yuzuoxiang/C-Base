using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Tools.WebHelper
{
    /// <summary>
    /// Cookie辅助类
    /// </summary>
    public class CookieHelper
    {
        /// <summary>
        /// 清除指定Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        public static void ClearCookie(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie!=null)
            {
                TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
                cookie.Expires = DateTime.Now.Add(ts);
                HttpContext.Current.Response.AppendCookie(cookie);
                HttpContext.Current.Request.Cookies.Remove(cookieName);
            }
        }

        /// <summary>
        /// 获取指定Cookie值
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string GetCookieValue(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            string str = string.Empty;
            if (cookie!=null)
            {
                str = cookie.Value;
            }

            return str;
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static HttpCookie GetCookie(string cookieName)
        {
            return HttpContext.Current.Request.Cookies[cookieName];
        }

        /// <summary>
        /// 添加一个Cookie,默认浏览器关闭过期
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="cookieValue"></param>
        /// <param name="days"></param>
        public static void SetCookie(string cookieName, System.Collections.Specialized.NameValueCollection cookieValue, int? days)
        {
            var cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie == null)
            {
                cookie = new HttpCookie(cookieName);
            }

            ClearCookie(cookieName);
            cookie.Values.Add(cookieValue);
            var siteurl = System.Configuration.ConfigurationManager.AppSettings["siteUrl"];
            if (!string.IsNullOrEmpty(siteurl))
            {
                cookie.Domain = siteurl.Replace("www.","");
            }

            if (days != null && days > 0)
            {
                cookie.Expires = DateTime.Now.AddDays(Convert.ToInt32(days));
            }
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="cookieValue"></param>
        /// <param name="expires"></param>
        public static void SetCookie(string cookieName, string cookieValue, int? expires)
        {
            var cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie!=null)
            {
                ClearCookie(cookieName);
            }
            cookie = new HttpCookie(cookieName);
            cookie.Value = cookieValue;
            var siteUrl = System.Configuration.ConfigurationManager.AppSettings["siteUrl"];
            if (!string.IsNullOrEmpty(siteUrl))
            {
                cookie.Domain = siteUrl.Replace("wwww.", "");
            }

            if (expires!=null&&expires>0)
            {
                cookie.Expires = DateTime.Now.AddDays(Convert.ToInt32(expires));
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }
    }
}
