using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Tools.OfficeHelper
{
    public class EmailOperat
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="userName">发件人邮箱登陆名</param>
        /// <param name="password">发件人邮箱密码</param>
        /// <param name="FromName">收件人名称</param>
        /// <param name="address">收件人邮箱地址</param>
        /// <param name="title">邮件主题</param>
        /// <param name="content">邮件正文</param>
        /// <param name="isBodyHrml">邮件正文是否为HTML格式</param>
        public static void SendMail(string userName,string password,string address,string addressName,string title, string content,bool isBodyHrml)
        {
            try
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(userName, addressName, Encoding.UTF8);   //发送人邮箱地址
                msg.To.Add(new MailAddress(address));   //收件人邮箱地址
                msg.Subject =title;                     //主题
                msg.Body = content;                     //正文
                msg.IsBodyHtml = isBodyHrml;            //正文是否html格式
                SmtpClient smtp = new SmtpClient();
                smtp.EnableSsl = true;
                smtp.Host = "smtp.qq.com";             //smtp服务器名称
                smtp.Credentials = new NetworkCredential(userName, password);//发送人的登陆名和密码
                smtp.Send(msg);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
