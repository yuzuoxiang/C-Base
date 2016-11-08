using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;

namespace Tools.OfficeHelper
{
    public class WordsOperat
    {
        /// <summary>
        /// 上传文件并转存为html
        /// </summary>
        /// <param name="wordFilePath">word文档在客户机的位置</param>
        /// <returns></returns>
        public static string wordToHtml(HtmlInputFile wordFilePath,string path)
        {
            StringBuilder output = new StringBuilder();

            //先把文件上传至服务器,返回word路径
            string filePath = uploadWord(wordFilePath,path);

            if (filePath == "0" || filePath == "1")
            {
                return filePath;
            }

            Aspose.Words.Document doc = new Aspose.Words.Document(filePath);

            #region 把word另存为html格式
            string filename = DateTime.Now.ToString("yyyyMMddHHmmss");

            // 如果指定目录不存在文件夹，则创建
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //被转换的html文档保存的位置  
            string ConfigPath = path+"\\" + filename + ".html";
            //object saveFileName = ConfigPath;

            doc.Save(ConfigPath);

            //删除word
            FileInfo fo = new FileInfo(filePath);
            fo.Delete();
            #endregion

            #region 读取html文件,显示到编辑框
            FileStream fs = new FileStream(ConfigPath, FileMode.Open, FileAccess.Read);
            //StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);

            string rl;
            while ((rl = sr.ReadLine()) != null)
            {
                output.Append(rl + "<br>");
            }
            sr.Close();
            fs.Close();

            //删除html
            FileInfo fo2 = new FileInfo(ConfigPath);
            fo2.Delete();
            #endregion
            string str = output.ToString();

            #region 把文章中的图片另存为
            //MatchCollection result = Regex.Matches(str, "<img(\n|.)*?>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            //for (int i = 0; i < result.Count; i++)
            //{
            //    BoolStr bls = new BoolStr();
            //    string img = result[i].Value;
            //    Match src = Regex.Match(img, "src=\"(\n|.)*?\"", RegexOptions.IgnoreCase);
            //    string picPath = src.Value.Substring(5, src.Length - 5 - 1);
            //    string picname = picPath.Substring(picPath.LastIndexOf("/") + 1);
            //    string pathstr = "\\upfile\\pic\\" + DateTime.Now.Year + "\\" + DateTime.Now.ToString("yyyy-MM");
            //    Guid gud = Guid.NewGuid();
            //    bls = ImgService.Save(WebSiteConfig.EfuWebPath + "\\upfile\\temp\\" + picname, gud + ".jpg", pathstr);

            //    string img2 = Regex.Replace(img, "alt=\"(\n|.)*?\"", "", RegexOptions.IgnoreCase);
            //    if (bls.OK)
            //    {
            //        string re = "src=\"" + bls.Msg + "\"";
            //        img2 = img2.Replace(src.Value, re);
            //    }
            //    str = str.Replace(img, img2);
            //}
            #endregion

            #region 调整文章格式
            //str = Regex.Replace(str, "name=\"OLE_LINK(\n|.)*?\"", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);//word导入后自动添加的标签属性，会导致页面出现小图标
            //去除多余标签
            str = Regex.Replace(str, "<div(\n|.)*?>", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            str = Regex.Replace(str, "</div(\n|.)*?>", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            str = Regex.Replace(str, "<a(\n|.)*?>", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            str = Regex.Replace(str, "</a(\n|.)*?>", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            //找出所有img标签，为每个img标签加上居中属性，标签后面加</br>换行
            MatchCollection arr = Regex.Matches(str, "<img(\n|.)*?>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            for (int i = 0; i < arr.Count; i++)
            {
                string val = arr[i].Groups[0].Value;
                if (string.IsNullOrEmpty(val))
                    continue;

                if (!val.Contains("text-align"))
                {
                    string imgStr = val.Replace("<img", "<p style=\"text-align:center;\"><img style=\"text-align:center;\" ");
                    str = str.Replace(val, imgStr + "</p>");
                }
            }

            #endregion

            return str;
        }

        /// <summary>
        /// 上传word 返回保存路径
        /// </summary>
        /// <param name="uploadFils"></param>
        /// <returns></returns>
        public static string uploadWord(HtmlInputFile uploadFils,string path)
        {
            if (uploadFils.PostedFile == null)
            {
                return "0";
            }

            string fileName = uploadFils.PostedFile.FileName;
            if (string.IsNullOrEmpty(fileName))
                return "0";
            int extendNameIndex = fileName.LastIndexOf(".");
            string extendName = fileName.Substring(extendNameIndex);
            string newName = "";
            try
            {
                //验证是否为word格式
                if (extendName.ToLower() == ".doc" || extendName.ToLower() == ".docx")
                {
                    DateTime now = DateTime.Now;
                    newName = now.DayOfYear.ToString() + uploadFils.PostedFile.ContentLength.ToString();

                    // 判断指定目录下是否存在文件夹，如果不存在，则创建  
                    if (!Directory.Exists(path))
                    {
                        //创建up文件夹
                        Directory.CreateDirectory(path);
                    }
                    //上传路径 指当前上传页面的同一级的目录下面的wordTmp路径  
                    uploadFils.PostedFile.SaveAs(string.Format("{0}\\{1}.doc", path, newName));
                }
                else
                {
                    return "1";
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
            return string.Format("{0}\\{1}.doc", path, newName);
        }         
    }
}
