using NSoup.Nodes;
using NSoup.Select;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Tools.WebHelper
{
    public class WebOperat
    {
        /// <summary>
        /// 根据URL获取HTMLcode
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetPage2(string url)
        {
            return GetPage2(url, "");
        }
        /// <summary>
        /// 根据URL获取HTMLcode
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Encoding"></param>
        /// <returns></returns>
        public static string GetPage2(string url, string Encoding)
        {
            try
            {
                string s1;
                using (WebClient webc = new WebClient())
                {
                    byte[] bstr = webc.DownloadData(new Uri(url));
                    if (Encoding == "")
                    {
                        s1 = System.Text.Encoding.UTF8.GetString(bstr);
                    }
                    else
                    {
                        s1 = System.Text.Encoding.GetEncoding(Encoding).GetString(bstr);
                    }
                }
                if (string.IsNullOrEmpty(s1) == false)
                {
                    if (s1.IndexOf("<title>404错误页面信息</title>") != -1)
                    {
                        return "{error}采集失败(" + url + ")，404页面！";
                    }
                    return s1;
                }
                else
                {
                    return "{error}采集失败(" + url + ")，无法加载页面！";
                }
            }
            catch (Exception ex)
            {
                return "{error}采集出错！" + ex.Message;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wstr"></param>
        /// <param name="start"></param>
        /// <param name="over"></param>
        /// <param name="str"></param>
        /// <param name="tag"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string body3st(string wstr, string start, string over, string str, bool tag, string t)
        {
            if (string.IsNullOrEmpty(wstr))
            {
                return null;
            }
            start = start.Replace("(", @"\(");
            start = start.Replace(")", @"\)");
            start = start.Replace("?", @"\?");
            start = start.Replace("$", @"\$");
            start = start.Replace("[", @"\[");
            start = start.Replace("]", @"\]");

            over = over.Replace("(", @"\(");
            over = over.Replace(")", @"\)");
            over = over.Replace("?", @"\?");
            over = over.Replace("$", @"\$");
            over = over.Replace("[", @"\[");
            over = over.Replace("]", @"\]");

            string parr = "";
            Regex reg = new Regex(start + "(\n|.)*?" + over, RegexOptions.IgnoreCase);
            MatchCollection mc = reg.Matches(wstr);
            if (mc.Count > 0)
            {
                foreach (Match m in mc)
                {
                    string k1 = m.ToString();
                    if (tag)
                    {
                        k1 = k1.Replace(start, "");
                        k1 = k1.Replace(over, "");
                    }
                    parr += (t + k1 + str);
                }
            }
            //if (str != "" && parr != "") parr = Left2(parr, str, str.Length);
            return parr;
        }

        /// <summary>
        /// 截取html字符串
        /// </summary>
        /// <param name="strOriginal">需要截取的html</param>
        /// <param name="strFirst">开始位置</param>
        /// <param name="strLast">结束位置</param>
        /// <param name="t">0为不保留开始结束字符，1为保留开始结束字符</param>
        /// <returns></returns>
        public static string GetContent(string strOriginal, string strFirst, string strLast, string t)
        {
            if (string.IsNullOrEmpty(strOriginal) == true)
            {
                return "";
            }
            string s = "";
            int t1, t2, t3;
            if (t == "0")
            {
                string strOriginal1 = strOriginal, strFirst1 = strFirst, strLast1 = strLast;
                t1 = strOriginal1.IndexOf(strFirst1);
                if (t1 >= 0)
                {
                    t2 = strOriginal1.Length;
                    t3 = t1 + strFirst1.Length;
                    strOriginal1 = strOriginal1.Substring(t3);

                    t1 = strOriginal1.IndexOf(strLast1);
                    t3 = t1;
                    if (t3 > 0)
                    {
                        s = strOriginal1.Substring(0, t3);
                    }
                }
            }
            else if (t == "1")
            {
                s = GetContent(strOriginal, strFirst, strLast, "0");
                s = strFirst + s + strLast;
            }
            return s;
        }


        /// <summary>
        /// 根据url返回Document格式的html
        /// </summary>
        /// <param name="url"></param>
        public static Document GetHtmlCodeByUrl(string url, string encoding)
        {
            Document doc = null;
            try
            {
                doc = NSoup.NSoupClient.Parse(WebRequest.Create(url).GetResponse().GetResponseStream(), encoding);
            }
            catch (Exception)
            {
                throw;
            }

            return doc;
        }


        /// <summary>
        /// 根据url返回Document格式的html并查询
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <param name="str">用来匹配html标签的字符</param>
        /// <returns></returns>
        public static Elements GetHtmlCodeByUrl(string url, string encoding,string str)
        {
            Elements eles = null;
            try
            {
                eles = NSoup.NSoupClient.Parse(WebRequest.Create(url).GetResponse().GetResponseStream(), encoding).Select(str);
            }
            catch (Exception)
            {
                throw;
            }

            return eles;
        }


        /// <summary>
        /// 使用代理采集数据，永久有效的代理IP：60.13.74.143:81
        /// </summary>
        /// <param name="ipFilePath">代理IP</param>
        /// <param name="query">url带的条件</param>
        /// <param name="encoding">编码格式</param>
        /// <param name="url">链接地址</param>
        /// <returns></returns>
        public static Document ProxyGetHtmlCodeByUrl(string ip,string query, string encoding,string url)
        {
            Document doc = null;
            ip = string.IsNullOrEmpty(ip) ? "60.13.74.143:81" : ip;
            WebProxy proxyObject = new WebProxy(string.Format("http://{0}",ip), true);
            WebRequest req = WebRequest.Create(string.Format("{0}?{1}",url,query));
            req.Timeout = 60 * 1000;
            req.Proxy = proxyObject;
            string html = req.GetResponse().GetResponseStream().ToString();
            doc = NSoup.NSoupClient.Parse(req.GetResponse().GetResponseStream(), encoding);

            if (req!=null)
            {
                req.Abort();
            }
            return doc;

        } 
    }
}
