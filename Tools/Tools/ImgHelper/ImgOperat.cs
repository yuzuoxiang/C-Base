using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Drawing;

namespace Tools.ImgHelper
{
    public class ImgOperat
    {
        /// <summary>
        /// 根据url地址下载图片到本地并返回图片保存路径
        /// </summary>
        /// <param name="url">图片下载地址</param>
        /// <param name="path">图片保存文件地址</param>
        /// <returns></returns>
        public static string DownLoadImg(string url,string imgPath)
        {
            Bitmap img = null;
            HttpWebRequest request;
            HttpWebResponse response = null;
            string url2 = string.Empty;
            Uri httpUrl = null;
            try
            {
                httpUrl = new Uri(url);
                request = (HttpWebRequest)(WebRequest.Create(httpUrl));
                request.Timeout = 180000;   //设置超时时间
                request.Method = "GET";
                response = (HttpWebResponse)(request.GetResponse());
                img = new Bitmap(response.GetResponseStream());

                int index = url.LastIndexOf("/") + 1;
                string picName = url.Substring(index);

                if (!FileHelper.FilesOperat.DirectoryPathExists(imgPath))
                {
                    return "";
                }

                imgPath = string.Format("{0}\\{1}", imgPath, picName);
                img.Save(imgPath);
                return imgPath;
            }
            catch (Exception)
            {
                throw;
            } 
        }
    }
}
