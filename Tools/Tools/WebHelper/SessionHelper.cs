using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Tools.WebHelper
{
    /// <summary>
    /// Session操作类
    /// 1、GetSession(string name)根据session名获取session对象
    /// 2、SetSession(string name, object val)设置session
    /// </summary>
    public class SessionHelper
    {
        /// <summary>
        /// 根据session名获取session对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetSession(string name)
        {
            return HttpContext.Current.Session[name];
        }

        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="name">Session名</param>
        /// <param name="val">Session值</param>
        public static void SetSession(string name, object val)
        {
            HttpContext.Current.Session.Remove(name);
            HttpContext.Current.Session.Add(name, val);
        }

        /// <summary>
        /// 添加Session，调动有效期为20分钟
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="strValue">Session值</param>
        public static void Add(string sessionName, string value)
        {
            Add(sessionName, value, 20);
        }

        /// <summary>
        /// 添加Session，调动有效期为20分钟
        /// </summary>
        /// <param name="sessionName">Session对象名称</param>
        /// <param name="value">Session值</param>
        public static void Adds(string sessionName, string[] value)
        {
            Add(sessionName, value, 20);
        }

        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="sessionName">Session对象名称</param>
        /// <param name="value">Session值</param>
        /// <param name="expires">调动有效期（分钟）</param>
        public static void Add(string sessionName, string value, int expires)
        {
            HttpContext.Current.Session[sessionName] = value;
            HttpContext.Current.Session.Timeout = expires;
        }

        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="sessionName">Session对象名称</param>
        /// <param name="value">Session值</param>
        /// <param name="expires">调动有效期（分钟）</param>
        public static void Add(string sessionName, string[] value, int expires)
        {
            HttpContext.Current.Session[sessionName] = value;
            HttpContext.Current.Session.Timeout = expires;
        }

        /// <summary>
        /// 读取某个Session对象值
        /// </summary>
        /// <param name="sessionName">Session对象名称</param>
        /// <returns>Session对象值</returns>
        public static string Get(string sessionName)
        {
            if (HttpContext.Current.Session[sessionName] == null)
            {
                return null;
            }
            else
            {
                return HttpContext.Current.Session[sessionName].ToString();
            }
        }

        /// <summary>
        /// 读取某个Session对象值数组
        /// </summary>
        /// <param name="sessionName">Session对象名称</param>
        /// <returns>Session对象值数组</returns>
        public static string[] Gets(string sessionName)
        {
            if (HttpContext.Current.Session[sessionName] == null)
            {
                return null;
            }
            else
            {
                return (string[])HttpContext.Current.Session[sessionName];
            }
        }

        /// <summary>
        /// 删除某个Session对象
        /// </summary>
        /// <param name="sessionName">Session对象名称</param>
        public static void Del(string sessionName)
        {
            HttpContext.Current.Session[sessionName] = null;
        }

        /// <summary>
        /// 移除Session
        /// </summary>
        /// <param name="sessionName"></param>
        public static void Remove(string sessionName)
        {
            if (HttpContext.Current.Session[sessionName]!=null)
            {
                HttpContext.Current.Session.Remove(sessionName);
                HttpContext.Current.Session[sessionName] = null;
            }
        }

    }
}
