using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Tools.StrHelper
{
    public class StrOperat
    {
        #region 日期操作
        /// <summary>
        /// 验证日期是否合法
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsDate(object date)
        {
            //如果为空，认为验证合格
            if (IsNullOrEmpty(date))
            {
                return false;
            }
            string strdate = date.ToString();
            try
            {
                //用转换测试是否为规则的日期字符
                date = Convert.ToDateTime(date).ToString("d");
                return true;
            }
            catch (Exception)
            {
                //如果日期字符串中存在非数字，则返回false
                if (!IsInt(strdate))
                {
                    return false;
                }

                #region 对纯数字进行解析
                //对8位纯数字进行解析
                if (strdate.Length == 8)
                {
                    //获取年月日
                    string year = strdate.Substring(0, 4);
                    string month = strdate.Substring(4, 2);
                    string day = strdate.Substring(6, 2);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    if (Convert.ToInt32(month) > 12 || Convert.ToInt32(day) > 31)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year + "-" + month + "-" + day).ToString("d");
                    return true;
                }

                //对6位纯数字进行解析
                if (strdate.Length == 6)
                {
                    //获取年月
                    string year = strdate.Substring(0, 4);
                    string month = strdate.Substring(4, 2);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }

                    if (Convert.ToInt32(month) > 12)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year + "-" + month).ToString("d");
                    return true;
                }

                //对5位纯数字进行解析
                if (strdate.Length == 5)
                {
                    //获取年月
                    string year = strdate.Substring(0, 4);
                    string month = strdate.Substring(4, 1);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }

                    //拼接日期
                    date = year + "-" + month;
                    return true;
                }

                //对4位纯数字进行解析
                if (strdate.Length == 4)
                {
                    //获取年
                    string year = strdate.Substring(0, 4);
                    
                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year).ToString("d");
                    return true;
                }
                #endregion 

                return false;
            }
        }

        /// <summary>
        /// 验证日期是否合法,对不规则的作了简单处理
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public static bool IsDate(ref string date)
        {
            //如果为空，认为验证合格
            if (IsNullOrEmpty(date))
            {
                return true;
            }

            //清除要验证字符串中的空格
            date = date.Trim();

            //替换\
            date = date.Replace(@"\", "-");

            //替换/
            date = date.Replace(@"/", "-");

            //如果查找到汉字"今",则认为是当前日期
            if (date.IndexOf("今") != -1)
            {
                date = DateTime.Now.ToString();
            }

            try
            {
                //用转换测试是否为规则的日期字符
                date = Convert.ToDateTime(date).ToString("d");
                return true;
            }
            catch (Exception)
            {
                //如果日期字符串中存在非数字，则返回false
                if (!IsInt(date))
                {
                    return false;
                }

                #region 对纯数字进行解析
                //对8位纯数字进行解析
                if (date.Length == 8)
                {
                    //获取年月日
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 2);
                    string day = date.Substring(6, 2);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    if (Convert.ToInt32(month) > 12 || Convert.ToInt32(day) > 31)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year + "-" + month + "-" + day).ToString("d");
                    return true;
                }

                //对6位纯数字进行解析
                if (date.Length == 6)
                {
                    //获取年月
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 2);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }

                    if (Convert.ToInt32(month) > 12)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year + "-" + month).ToString("d");
                    return true;
                }

                //对5位纯数字进行解析
                if (date.Length == 5)
                {
                    //获取年月
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 1);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }

                    //拼接日期
                    date = year + "-" + month;
                    return true;
                }

                //对4位纯数字进行解析
                if (date.Length == 4)
                {
                    //获取年
                    string year = date.Substring(0, 4);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year).ToString("d");
                    return true;
                }
                #endregion    

                return false;
            }
        }

        /// <summary>
        /// 获得两个日期的间隔
        /// </summary>
        /// <param name="dateTime1">日期一</param>
        /// <param name="dateTime2">日期二</param>
        /// <returns>日期间隔TimeSpan</returns>
        public static TimeSpan DateDiff(DateTime dateTime1, DateTime dateTime2)
        {
            TimeSpan ts1 = new TimeSpan(dateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(dateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            return ts;
        }

        /// <summary>
        /// 格式化时间日期
        /// </summary>
        /// <param name="dateTime">时间日期</param>
        /// <param name="dateMode">类型编号</param>
        /// <returns></returns>
        public static string FormatDate(DateTime dateTime, string dateMode)
        {
            switch (dateMode)
            {
                case "1":
                    return dateTime.ToString("yyyy-MM-dd");
                case "2":
                    return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                case "3":
                    return dateTime.ToString("yyyy/MM/dd");
                case "4":
                    return dateTime.ToString("yyyy年MM月dd日");
                case "5":
                    return dateTime.ToString("MM-dd");
                case "6":
                    return dateTime.ToString("MM/dd");
                case "7":
                    return dateTime.ToString("MM月dd日");
                case "8":
                    return dateTime.ToString("yyyy-MM");
                case "9":
                    return dateTime.ToString("yyyy/MM");
                case "10":
                    return dateTime.ToString("yyyy年MM月");
                default:
                    return dateTime.ToString();
            }
        }

        /// <summary>
        /// 得到两个日期之间的随机日期
        /// </summary>
        /// <param name="dateTime1">起始日期</param>
        /// <param name="dateTime2">结束日期</param>
        /// <returns>间隔日期之间的随机日期</returns>
        public static DateTime GetRandomTime(DateTime dateTime1, DateTime dateTime2)
        {
            Random random = new Random();
            DateTime minTime = new DateTime();
            DateTime maxTime = new DateTime();

            TimeSpan ts = new TimeSpan(dateTime1.Ticks - dateTime2.Ticks);

            //获取两个时间相隔的秒数
            double dTotalSecontds = ts.TotalSeconds;
            int iTotalSecontds = 0;

            if (dTotalSecontds>System.Int32.MaxValue)
            {
                iTotalSecontds = System.Int32.MaxValue;
            }
            else if (dTotalSecontds < System.Int32.MinValue)
            {
                iTotalSecontds = System.Int32.MinValue;
            }
            else 
            {
                iTotalSecontds = (int)dTotalSecontds;
            }

            if (iTotalSecontds>0)
            {
                minTime = dateTime2;
                maxTime = dateTime1;
            }
            else if (iTotalSecontds < 0)
            {
                minTime = dateTime1;
                maxTime = dateTime2;
            }
            else
            {
                return dateTime1;
            }

            int maxValue = iTotalSecontds;
            if (iTotalSecontds<=System.Int32.MinValue)
            {
                maxValue = System.Int32.MinValue + 1;
            }

            int i = random.Next(System.Math.Abs(maxValue));

            return minTime.AddSeconds(i);
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetRandomTimeSpan()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0,0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        #endregion

        /// <summary>
        /// 得到字符串长度，一个汉字长度为2
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static int StrLength(string inputString)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
            }

            return tempLen;
        }

        /// <summary>
        /// 截取指定长度字符串
        /// </summary>
        /// <param name="inputString">要处理的字符串</param>
        /// <param name="len">指定长度</param>
        /// <returns>返回处理后的字符串</returns>
        public static string ClipString(string inputString, int len)
        {
            bool isShowFix = false;
            if (len%2==1)
            {
                isShowFix = true;
                len--;
            }
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch (Exception)
                {
                    break;
                }

                if (tempLen>len)
                {
                    break;
                }
            }

            byte[] mybyte = Encoding.Default.GetBytes(inputString);
            if (isShowFix &&mybyte.Length>len)
            {
                tempString += "_";
            }

            return tempString;
        }

        /// <summary>
        /// 显示距离当前时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ForMatTimes(string time)
        {
            try
            {
                string s = string.Empty;
                DateTime startDate = DateTime.Parse(time.ToString());
                DateTime endDate = DateTime.Now;
                TimeSpan TS = new TimeSpan(endDate.Ticks - startDate.Ticks);
                double j = Math.Round(TS.TotalMinutes);

                if (j <= 1)
                {
                    s = "刚刚";
                }
                else if (j > 1 && j < 60)
                {
                    s = j + "分钟前";
                }
                else if (j >= 60 && j < 60 * 24)
                {
                    s = Math.Round(TS.TotalHours) + "小时前";
                }
                else if (j >= 60 * 24 && j < 60 * 24 * 30)
                {
                    s = Math.Round(TS.TotalDays) + "天前";
                }
                else if (j >= 60 * 24 * 30 && j < 60 * 24 * 30 * 12)
                {
                    s = Math.Floor(Math.Round(TS.TotalDays) / 30) + "月前";
                }
                else if (j >= 60 * 24 * 30 * 12)
                {
                    s = Math.Floor(Math.Round(TS.TotalDays) / (30 * 12)) + "年前";
                }
                return s;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// HTML转行成TEXT
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        public static string RemoveHtml(string strHtml)
        {
            if (string.IsNullOrEmpty(strHtml))
            {
                return string.Empty;
            }

            string[] aryReg = { @"<script[^>]*?>.*?</script>",
                                @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
                                @"([\r\n])[\s]+",
                                @"&(quot|#34);",
                                @"&(amp|#38);",
                                @"&(lt|#60);",
                                @"&(gt|#62);", 
                                @"&(nbsp|#160);",
                                @"&(iexcl|#161);",
                                @"&(cent|#162);",
                                @"&(pound|#163);",
                                @"&(copy|#169);",
                                @"&#(\d+);",
                                @"-->",
                                @"<!--.*\n",
                              };
            string newReg = aryReg[0];
            string strOutput = strHtml;
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = RegexHelper.ReplaceStr(strOutput, aryReg[i], string.Empty);//regex.Replace(strOutput, string.Empty);
            }

            strOutput = strOutput.Replace("<", "");
            strOutput = strOutput.Replace(">", "");
            strOutput = strOutput.Replace("\r\n", "");

            return strOutput;
        }

        /// <summary>
        /// 判断对象是否为空，为空返回true
        /// </summary>
        /// <typeparam name="T">要验证的对象的类型</typeparam>
        /// <param name="data">要验证的对象</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(T data)
        {
            //如果为null
            if (data==null)
            {
                return true;
            }

            //如果为""
            if (data.GetType()==typeof(String))
            {
                if (string.IsNullOrEmpty(data.ToString().Trim())||data.ToString()=="")
                {
                    return true;
                }
            }

            //如果为DBNull
            if (data.GetType()==typeof(DBNull))
            {
                return true;
            }

            //不为空
            return false;
        }

        /// <summary>
        /// 判断对象是否为空，为空返回true
        /// </summary>
        /// <param name="data">要验证的对象</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(object data)
        {
            //如果为null
            if (data==null)
            {
                return true;
            }

            //如果为""
            if (data.GetType()==typeof(string))
            {
                if (string.IsNullOrEmpty(data.ToString().Trim()))
                {
                    return true;                    
                }
            }

            //如果为DBNull
            if (data.GetType()==typeof(DBNull))
            {
                return true;
            }

            //不为空
            return false;
        }

        /// <summary>
        /// 验证是否浮点数
        /// </summary>
        /// <param name="floatNum"></param>
        /// <returns></returns>
        public static bool IsFloat(string floatNum)
        {
            //如果为空，认为验证不合格
            if (IsNullOrEmpty(floatNum))
            {
                return false;
            }

            //清除要验证字符串中的空格
            floatNum = floatNum.Trim();

            //模式字符串
            string pattern = @"^(-?\d+)(\.\d+)?$";

            //验证
            return RegexHelper.IsMatch(floatNum, pattern);
        }

        /// <summary>
        /// 验证是否为整数 如果为空，认为验证不合格 返回false
        /// </summary>
        /// <param name="number">要验证的整数</param>
        /// <returns></returns>
        public static bool IsInt(string number)
        {
            //如果为空，认为验证不合格
            if (IsNullOrEmpty(number))
            {
                return false;                
            }

            //清除要验证字符串中的空格
            number = number.Trim();

            //模式字符串
            string pattern = @"^[0-9]+[0-9]*$";

            //验证
            return RegexHelper.IsMatch(number, pattern);
        }

        /// <summary>
        /// 验证是否为数字
        /// </summary>
        /// <param name="number">要验证的数字</param>
        /// <returns></returns>
        public static bool IsNumber(string number)
        {
            //如果为空，认为验证不合格
            if (IsNullOrEmpty(number))
            {
                return false;
            }

            //清除要验证字符串中的空格
            number = number.Trim();

            //模式字符串
            string pattern = @"^[0-9]+[0-9]*[.]?[0-9]*$";

            //验证
            return RegexHelper.IsMatch(number, pattern);
        }
         
        /// <summary>
        /// 字符转整形
        /// </summary>
        /// <param name="val"></param>
        /// <param name="defaultVal">默认值</param>
        /// <returns></returns>
        public static int StrToInt(string val, int defaultVal)
        {
            int iRet;
            if (int.TryParse(val,out iRet)==false)
            {
                iRet = defaultVal;
            }

            return iRet;
        }

        /// <summary>
        /// 字符转整形
        /// </summary>
        /// <param name="val"></param>
        /// <param name="defaultVal">默认值</param>
        /// <returns></returns>
        public static int StrToInt(string val)
        {
            return StrToInt(val, 0);
        }

        /// <summary>
        /// 如果为true返回1，否则返回0
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string BoolToInt(object b)
        {
            bool bb = (bool)b;
            if (bb)
            {
                return "1";
            }

            return "0";
        }

        /// <summary>
        /// 如果等于1返回true,否则返回false
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IntToBool(int s)
        {
            if (s == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// HTML转字符串
        /// </summary>
        /// <param name="Readme"></param>
        /// <returns></returns>
        public static string HtmlToStr(string Readme)
        {
            if (string.IsNullOrEmpty(Readme))
            {
                return string.Empty;
            }
            else
            {
                Readme = Readme.Replace("&lt;", "<");
                Readme = Readme.Replace("&gt;", ">");
                Readme = Readme.Replace("<br>", "\r\n");
                Readme = Readme.Replace("&nbsp;&nbsp;", "  ");
                Readme = Readme.Replace("&#64;", "@");
            }

            return Readme;
        }

        /// <summary>
        /// 字符串转HTML
        /// </summary>
        /// <param name="Readme"></param>
        /// <returns></returns>
        public static string StrToHtmL(string Readme)
        {
            if (string.IsNullOrEmpty(Readme))
            {
                return string.Empty;
            }
            else
            {
                Readme = Readme.Replace("<", "&lt;");
                Readme = Readme.Replace(">", "&gt;");
                Readme = Readme.Replace("\r\n", "<br>");
                Readme = Readme.Replace("  ", "&nbsp;&nbsp;");
                Readme = Readme.Replace("@", "&#64;");
            }

            return Readme;
        }

        static public string HtmlEncode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            str = HttpUtility.HtmlEncode(str);
            str = str.Replace("&#183;", "·");
            str = str.Replace("&quot;", "\"");
            str = str.Replace("&amp;", "&");
            return str;
        }

        /// <summary>
        /// 检查字符串是否存在与一个,组合到一起的字符串数组中
        /// </summary>
        /// <param name="strSplit">未分割的字符串</param>
        /// <param name="split">分割符号</param>
        /// <param name="targetValue">目标字符串</param>
        /// <returns></returns>
        public static bool CheckStringHasValue(string strSplit, char split, string targetValue)
        {
            string[] strList = strSplit.Split(split);
            foreach (string str in strList)
            {
                if (targetValue ==str)
                {
                    return true;
                }
            }

            return false;
        }

        #region 其他
        /// <summary>
        /// 前台显示邮箱的掩码替换(由tzh@qq.com等替换成t*****@qq.com)
        /// </summary>
        /// <param name="Email">邮箱</param>
        /// <returns></returns>
        public static string GetEmail(string Email)
        {
            string strArg = "";
            string SendEmail = "";
            Match match = Regex.Match(Email, @"(\w)\w+@");

            if (match.Success)
            {
                strArg = match.Groups[1].Value + "*****@";
                SendEmail = Regex.Replace(Email, @"\w+@", strArg);
            }
            else
            {
                SendEmail = Email;
            }

            return SendEmail;
        }
        #endregion

        #region 枚举型相关操作
        /// <summary>
        /// 功能描述；获取枚举名称.传入枚举类型和枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="intEnumValue"></param>
        /// <returns></returns>
        public static string GetEnumText<T>(int intEnumValue)
        {
            return Enum.GetName(typeof(T), intEnumValue);
        }

        /// <summary>
        /// 功能描述:获取枚举项集合，传入枚举类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IList<object> BindEnums<T>()
        {
            IList<object> list = new List<object>();
            //遍历枚举集合
            foreach (int i in Enum.GetValues(typeof(T)))
            {
                var selItem = new
                {
                    Value = i,
                    Text = Enum.GetName(typeof(T), i)
                };
                list.Add(selItem);
            }
            return list;
        }

        ///<summary>
        /// 返回 Dic 枚举项，描述
        ///</summary>
        ///<param name="enumType"></param>
        ///<returns>Dic枚举项，描述</returns>
        public static Dictionary<string, string> BindEnums(Type enumType)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            FieldInfo[] fieldinfos = enumType.GetFields();
            foreach (FieldInfo field in fieldinfos)
            {
                if (field.FieldType.IsEnum)
                {
                    Object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

                    dic.Add(field.Name, ((DescriptionAttribute)objs[0]).Description);
                }
            }

            return dic;
        }
        #endregion

        #region 获取集合中某个字段的拼接，例：获取姓名拼接
        /// <summary>
        /// 功能描述：获取集合中某个字段的拼接，例：获取姓名拼接
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">集合</param>
        /// <param name="strFieldName">字段名</param>
        /// <param name="strSplit">分隔符</param>
        /// <returns></returns>
        public static string GetFieldValueJoin<T>(IList<T> list, string strFieldName, string strSplit)
        {
            //判断入口
            if (list == null || list.Count <= 0 || string.IsNullOrEmpty(strFieldName))
                return string.Empty;

            //获取属性
            PropertyInfo _pro = typeof(T).GetProperty(strFieldName);
            if (_pro == null)
                return string.Empty;
            //变量，记录返回值
            string _strReturn = string.Empty;
            foreach (T _entityI in list)
            {
                //获取属性值
                object _objValue = _pro.GetValue(_entityI, null);
                if (_objValue == null || string.IsNullOrEmpty(_objValue.ToString()))
                    //没有属性值，则跳过
                    continue;

                //有属性值，则拼接
                _strReturn += _objValue.ToString() + strSplit;
            }

            if (string.IsNullOrEmpty(_strReturn))
            {
                return string.Empty;
            }

            return _strReturn.Substring(0, _strReturn.Length - strSplit.Length);
        }
        #endregion

        #region 检测是否有Sql危险字符
        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns></returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 检查危险字符
        /// </summary>
        /// <param name="sInput"></param>
        /// <returns></returns>
        public static string Filter(string sInput)
        {
            if (sInput == null || sInput == "")
            {
                return null;
            }

            string sInput1 = sInput.ToLower();
            string output = sInput;
            string pattern = @"*|and|exec|insert|select|delete|update|count|master|truncate|declare|char(|mid(|chr(|'";
            if (Regex.Match(sInput1, Regex.Escape(pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
            {
                throw new Exception("字符串中含有非法字符!");
            }
            else
            {
                output = output.Replace("'", "''");
            }
            return output;
        }

        #endregion
    }
}
