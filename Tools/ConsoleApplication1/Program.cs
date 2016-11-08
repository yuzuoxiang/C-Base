using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.CryptHelper;
using Tools.OfficeHelper;

namespace ConsoleApplication1
{
    public class Program
    { 
        static void Main(string[] args)
        {
            test3();
            //GetBaiduMapInfo();
            Console.WriteLine("测试成功");
            Console.ReadLine();
        }


        private static void test1()
        {
            string str = "";
            int j = 119;
            for (int i = 0; i < 14; i++)
            {
                str += string.Format(" <li ><%=loadData(4,2,{0}) %></li>\n", j);
                j++;
            }
        }

        private static void test2()
        {
            string str = "";
            int j = 237;
            int k = 772;
            for (int i = 0; i < 33; i++)
            {
                str += string.Format("update fenxi_ad set adid='{0}' where id='{1}'", j,k);
                j++;
                k++;
            }
        }

        private static void test3()
        {
            string str = "";
            int j = 1;

            for (int i = 118; i <= 132; i++)
            {
                str += string.Format("('4','{0}','文字{0}'),", i);
                //str += string.Format("('4','{0}','图片广告{0}'),", i);
                //j++;
            }
        }







        /// <summary>
        /// 百度地图POI检索测试
        /// </summary>
        private static void GetBaiduMapInfo()
        {
            string url = "http://api.map.baidu.com/place/search?query=海底捞&radius=10000000000000&region=中国&output=html&src=yourCompanyName|yourAppName";
            var ddd = Tools.WebHelper.WebOperat.GetHtmlCodeByUrl(url, "utf-8");
        }

        /// <summary>
        /// 抓取网站源码测试
        /// </summary>
        private static void GetWebHtmlCode()
        {
            string url = "https://rate.tmall.com/list_detail_rate.htm?itemId=17288202315&sellerId=1652490016&currentPage=1";
            var ddd = Tools.WebHelper.WebOperat.GetHtmlCodeByUrl(url, "gbk");
        }

        /// <summary>
        /// 发送邮件测试
        /// </summary>
        private static void sendEmail()
        {
            string content = string.Empty;
            content += "<strong>尊敬的客户：</strong><br/>";
            content += "<strong>媒介通新闻自助发布平台提醒您</strong><br/>";
            content += "稿件文章：<span style='color:blue'>活动一个女人的24小时</span><br/>";
            content += "发布媒体：腾讯时尚<br/>";
            content += "拒稿原因：内容不合适发布<br/>";
            content += "发稿平台：http://rw.efu.com.cn/";

            EmailOperat.SendMail("3440326359@qq.com", "zhtozqlacsobchaj", "963526232@qq.com", "媒介通新闻自助发布平台", "今天天气好", content, true);
        }

        /// <summary>
        /// 加密测试
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        private static string crypt(string txt)
        {            
            string str= AESCrypt.Decrypt(txt, "11");//D0C103488E288D9C7C960EF005484A08
            return str;
        }
    }
}
