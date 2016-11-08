using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Tools
{
    public class Log
    {
        /// <summary>
        /// 写入日志文件
        /// </summary>
        /// <param name="input"></param>
        public static void WriteLogFile(string input)
        {
            //指定日志文件目录
            string fname = Directory.GetCurrentDirectory() + string.Format("\\Log{0}.txt", DateTime.Now.ToString("yyyyMMdd"));

            //定义文件信息对象
            FileInfo finfo = new FileInfo(fname);

            //检测指定目录是否存在
            if (!finfo.Exists)
            {
                FileStream fs;
                fs = File.Create(fname);
                fs.Close();
                finfo = new FileInfo(fname);
            }

            //判断文件大小是否过大
            if (finfo.Length > 1024 * 1024 * 10)
            {
                //文件超过10MB则重命名
                File.Move(Directory.GetCurrentDirectory() + "\\LogFile.txt", Directory.GetCurrentDirectory() + DateTime.Now.TimeOfDay + "LogFile.txt");
                //删除文件
                //finfo.Delete();
            }

            using (FileStream fs = finfo.OpenWrite())
            {
                //根据上面的文件流创建写数据流
                StreamWriter w = new StreamWriter(fs);

                //设置文件流的起始位置为文件流的末尾
                w.BaseStream.Seek(0, SeekOrigin.End);
                w.Write("\n\r{0} {1} \n\r", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString() + ":");
                //写入日志内容并换行
                w.Write(input + "\n\r");
                //清空缓冲区内容，并把缓冲区内容写入基础流
                w.Flush();
                //关闭写数据流
                w.Close();
            }
        }
    }
}
