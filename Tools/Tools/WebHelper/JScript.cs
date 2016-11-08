using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Tools.WebHelper
{
    /// <summary>
    /// 一些常用的js调用
    /// 添加新版说明：由于旧版普遍采用Response.Write(string msg)的方式输出js脚本，这种
    /// 方式输出的js脚本会在html元素的&lt;html&gt;&lt;/html&gt;标签之外，破坏了整个xhtml的结构,
    /// 而新版本则采用ClientScript.RegisterStartupScript(string msg)的方式输出，不会改变xhtml的结构,
    /// 不会影响执行效果。
    /// 为了向下兼容，所以新版本采用了重载的方式，新版本中要求一个System.Web.UI.Page类的实例。
    /// </summary>
    public class JScript
    {
        #region 旧版本
        /// <summary>
        /// 弹出JavaScript小窗口
        /// </summary>
        /// <param name="message">消息内容</param>
        public static void Alert(string message)
        {
            string js = @"<Script language='JavaScript'>
                            alert('{0}');
                        </Script>";
            HttpContext.Current.Response.Write(string.Format(js, message));
        }

        /// <summary>
        /// 弹出消息框并跳转到新的URL
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="toUrl">连接地址</param>
        public static void AlertAndRedirect(string message, string toUrl)
        {
            string js = @"<Script language='JavaScript'>
                            alert('{0}');
                            window.location.replace('{0}')
                        </Script>";
            HttpContext.Current.Response.Write(string.Format(js, message, toUrl));
        }

        /// <summary>
        /// 回到历史页面
        /// </summary>
        /// <param name="value">-1/1</param>
        public static void GoHistory(int value)
        {
            string js = @"<Script language='JavaScript'>
                            history.go({0});
                        </Script>";
            HttpContext.Current.Response.Write(string.Format(js, value));
        }

        /// <summary>
        /// 关闭当前串口窗口
        /// </summary>
        public static void CloseWindow()
        {
            string js = @"<Script language='JavaScript'>
                            parent.opener=null;
                            window.close();
                        </Script>";
            HttpContext.Current.Response.Write(js);
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 刷新父窗口
        /// </summary>
        /// <param name="url"></param>
        public static void RefreshParent(string url)
        {
            string js = @"<Script language='JavaScript'>
                            window.opener.location.href='{0}';
                            window.close();
                        </Script>";
            HttpContext.Current.Response.Write(string.Format(js, url));
        }

        /// <summary>
        /// 刷新开口窗口
        /// </summary>
        public static void RefreshOpener()
        {
            string js = @"<Script language='JavaScript'>
                            opener.location.reload();
                        </Script>";
            HttpContext.Current.Response.Write(js);
        }

        /// <summary>
        /// 打开一个指定大小的新窗体
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="top">距离头位置</param>
        /// <param name="left">距离左位置</param>
        public static void OpenWebFormSize(string url, int width, int height, int top, int left)
        {
            string js = @"<Script language='JavaScript'>
                            window.open('{0}','','height={1},width={2},top={3},left={4},location=no,menubar=no,
                            resizable=yes,scrollbars=yes,status=yes,titlebar=no,toolbar=no,directories=no');
                        </Script>";
            HttpContext.Current.Response.Write(string.Format(js, url, height, width, top, left));
        }

        /// <summary>
        /// 转向Url指定的页面
        /// </summary>
        /// <param name="url">链接地址</param>
        public static void JavaScriptLocationHref(string url)
        {
            string js = @"<Script language='JavaScript'>
                            window.location.replace('{0}');
                        </Script>";
            HttpContext.Current.Response.Write(string.Format(js, url));
        }

        /// <summary>
        /// 打开指定大小位置的模式对话框
        /// </summary>
        /// <param name="webFormUrl">连接地址</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="top">距离头位置</param>
        /// <param name="left">距离左位置</param>
        public static void ShowModalDialogWindow(string webFormUrl, int width, int height, int top, int left)
        {
            string features = @"dialogWidth:{0}px;
                                dialogHeight:{1}px;
                                dialogLeft:{2}px;
                                dialogTop:{3}px;
                                center:yes;help=no;resizable:no;status:no;scroll=yes";
            string js = ShowModalDialogWindow(webFormUrl, string.Format(features, width, height, left, top));

            HttpContext.Current.Response.Write(js);
        }

        /// <summary>
        /// 弹出模态窗口
        /// </summary>
        /// <param name="webFromUrl"></param>
        /// <param name="features"></param>
        public static string ShowModalDialogWindow(string webFormUrl, string features)
        {
            string js = @"<Script language='JavaScript'>
                            showModalDialog('{0}','','{1}');
                        </Script>";
            return string.Format(js, webFormUrl, features);
        }
        #endregion

        #region 新版本
        /// <summary>
        /// 弹出JavaScript小窗口
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="page">Page类的实例</param>
        public static void Alert(string message, Page page)
        {
            string js = @"<Script language='JavaScript'>
                            alert('{0}');
                        </Script>";
            PageClientScript("alert", page, string.Format(js, message));
        }

        /// <summary>
        /// 弹出消息框并且转向到新的URL
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="toURL">跳转链接</param>
        /// <param name="page">Page类实例</param>
        public static void AlertAndRedirect(string message, string toURL, Page page)
        {
            string js = @"<Script language='JavaScript'>
                            alert('{0}');
                            window.location.replace('{1}');
                        </Script>";
            PageClientScript("AlertAndRedirect", page, string.Format(js, message, toURL));
        }

        /// <summary>
        /// 回到历史页面
        /// </summary>
        /// <param name="value">-1/1</param>
        /// <param name="page">Page类实例</param>
        public static void GoHistory(int value, Page page)
        {
            string js = @"<Script language='JavaScript'>
                            history.go({0});
                        </Script>";
            PageClientScript("GoHistory", page, string.Format(js, value));
        }

        /// <summary>
        /// 刷新父窗口
        /// </summary>
        /// <param name="url">要刷新的链接</param>
        /// <param name="page">Page类的实例</param>
        public static void RefreshParent(string url, Page page)
        {
            string js = @"<Script language='JavaScript'>
                            window.opener.location.href='{0}';
                            window.close();
                        <Script>";
            PageClientScript("RefreshParent", page, string.Format(js, url));
        }

        /// <summary>
        /// 刷新打开窗口
        /// </summary>
        /// <param name="page">Page类的实例</param>
        public static void RefreshOpener(Page page)
        {
            string js = @"<Script language='JavaScript'>
                            opener.location.reload();
                        </Script>";
            PageClientScript("RefreshOpener", page, js);
        }

        /// <summary>
        /// 打开指定大小的新窗体
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="top">距离上位置</param>
        /// <param name="left">距离左位置</param>
        /// <param name="page">Page类的实例</param>
        public static void OpenWebFormSize(string url, int width, int height, int top, int left, Page page)
        {
            string js = @"<Script language='JavaScript'>
                            window.open('{0}','','height={1},width={2},top={3},left={4},location=no,menubar=no,
                            resizable=yes,scrollbars=yes,status=yes,titlebar=no,toolbar=no,directories=no');
                        </Script>')";
            PageClientScript("OpenWebFormSize", page, string.Format(js, url, height, width, top, left));
        }

        /// <summary>
        /// 转向Url指定的页面
        /// </summary>
        /// <param name="url">链接地址</param>
        /// <param name="page">Page类的实例</param>
        public static void JavaScriptLocationHref(string url, Page page)
        {
            string js = @"<Script language='JavaScript'>
                            window.location.replace('{0}')
                        </Script>";
            PageClientScript("JavaScriptLocationHref", page, string.Format(js, url));
        }

        /// <summary>
        /// 打开指定大小位置的模式对话框
        /// </summary>
        /// <param name="webFormUrl">链接地址</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="top">距离上位置</param>
        /// <param name="left">距离左位置</param>
        /// <param name="page">Page类的实例</param>
        public static void ShowModalDialogWindow(string webFormUrl, int width, int height, int top, int left, Page page)
        {
            string features = @"dialogWidth:{0}px;
                                dialogHeight:{1}px;
                                dialogLeft:{2}px;
                                dialogTop:{3}px;
                                center:yes;help=no;resizable:no;status:no;scroll=yes";
            string js = ShowModalDialogWindow(webFormUrl, string.Format(features, width, height, left, top));
            PageClientScript("ShowModalDialogWindow", page, js);
        }

        /// <summary>
        /// 向当前页面动态输出客户端脚本代码
        /// </summary>
        /// <param name="javascript">javascript脚本段</param>
        /// <param name="page">Page类的实例</param>
        /// <param name="afterForm">是否紧跟在&lt;form&gt;标记之后输出javascript脚本，如果不是则在&lt;/form&gt;标记之前输出脚本代码</param>
        public static void AppendScript(string javascript, Page page, bool afterForm)
        {
            if (!afterForm)
            {
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), page.ToString(), javascript);
            }
            else
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), page.ToString(), javascript);
            }
        }

        /// <summary>
        /// 注册启动脚本
        /// </summary>
        /// <param name="scriptName">脚本名</param>
        /// <param name="page">Page类的实例</param>
        /// <param name="js">脚本源码</param>
        private static void PageClientScript(string scriptName, Page page, string js)
        {
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), scriptName))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), scriptName, js);
            }
        }
        #endregion
    }
}
