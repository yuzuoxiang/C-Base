using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace Tools.FileHelper
{
    public class FilesOperat
    {
        /// <summary>
        /// 判断指定路径的文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool FilePathExists(string path)
        {
            try
            {
                //定义文件信息对象
                FileInfo file = new FileInfo(path);
                //检测指定目录是否存在
                if (!file.Exists)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 判断指定路径的文件是否存在，不存在就创建文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static void FilePathCreate(string path)
        {
            try
            {
                FileInfo file = new FileInfo(path);
                if (!file.Exists)           //检测指定目录是否存在
                {
                    FileStream fs;
                    fs = File.Create(path);
                    fs.Close();
                    file = new FileInfo(path);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 判断指定的文件夹是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool DirectoryPathExists(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 判断指定的文件夹是否存在，不存在创建文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static void DirectoryPathCreate(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取文本内容
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileContent(string path)
        {
            try
            {
                FileInfo file = new FileInfo(@path);
                if (!file.Exists)
                {
                    return "";
                }
                //c#文件流读文件 
                using (FileStream fsRead = new FileStream(@path, FileMode.Open))
                {
                    int fsLen = (int)fsRead.Length;
                    byte[] heByte = new byte[fsLen];
                    int r = fsRead.Read(heByte, 0, heByte.Length);
                    string str = System.Text.Encoding.UTF8.GetString(heByte);
                    return str;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 向一个txt写入文本，不存在就先创建
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static void AddFileContent(string path,string content)
        {
            try
            {
                //创建实例
                FileInfo finfo=new FileInfo(path);
                //检测指定txt是否存在
                if (!finfo.Exists)      
                {
                    FileStream fs;
                    fs=File.Create(path);
                    fs.Close();
                    finfo=new FileInfo(path);
                }
                //写入内容
                using (FileStream fs=finfo.OpenWrite())
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(content);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
