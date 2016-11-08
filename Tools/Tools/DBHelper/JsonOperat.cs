using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Tools.DBHelper
{
    public class JsonOperat
    {
        /// <summary>
        /// 把对象转化为json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ConvertToJson(object obj)
        {
            return new JavaScriptSerializer().Serialize(obj);
        }
    }
}
