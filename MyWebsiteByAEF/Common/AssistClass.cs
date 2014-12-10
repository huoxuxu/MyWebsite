using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Specialized;

namespace Common
{
    public class AssistClass
    {

        #region 汉字和Unicode编码互转
        /// <summary>
        /// 汉字转换为Unicode编码
        /// </summary>
        /// <param name="str">要编码的汉字字符串</param>
        /// <returns>Unicode编码的的字符串</returns>
        public static string ToUnicode(string str)
        {
            byte[] bts = Encoding.Unicode.GetBytes(str);
            string r = "";
            for (int i = 0; i < bts.Length; i += 2) r += "\\u" + bts[i + 1].ToString("x").PadLeft(2, '0') + bts[i].ToString("x").PadLeft(2, '0');
            return r;
        }

        /// <summary>
        /// 将Unicode编码转换为汉字字符串
        /// </summary>
        /// <param name="str">Unicode编码字符串</param>
        /// <returns>汉字字符串</returns>
        public static string ToGB2312(string str)
        {
            string r = "";
            MatchCollection mc = Regex.Matches(str, @"\\u([\w]{2})([\w]{2})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            byte[] bts = new byte[2];
            foreach (Match m in mc)
            {
                bts[0] = (byte)int.Parse(m.Groups[2].Value, NumberStyles.HexNumber);
                bts[1] = (byte)int.Parse(m.Groups[1].Value, NumberStyles.HexNumber);
                r += Encoding.Unicode.GetString(bts);
            }
            return r;
        }

        /// <summary>Unicode转中文
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UnicodeToChinese(string s)
        {
            string res = s;
            MatchCollection reg = Regex.Matches(res, @"\\u\w{4}");
            for (int i = 0; i < reg.Count; i++)
            {
                res = res.Replace(reg[i].Groups[0].Value, "" + Regex.Unescape(reg[i].Value.ToString()).ToString());
            } return res;
        }
        #endregion

        #region HttpWebRequest
        /// <summary>HttpWebRequest返回请求json
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HttpWebRequest2Json(string url)
        {

            using (var response = HttpWebRequest.Create(url).GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    url = reader.ReadToEnd();
                }
            }
            return url;//上海市
        }

        /// <summary>
        /// HttpWebRequest的Post带参请求
        /// </summary>
        /// <param name="postUrl">Post的URL</param>
        /// <param name="paramData">Url参数不需要?</param>
        /// <param name="dataEncode">参数编码及返回值编码,一般指定Encoding.GetEncoding("GB2312")</param>
        /// <returns></returns>
        public static string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
        {
            string ret = string.Empty;
            try
            {
                //byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                byte[] byteArray = Encoding.UTF8.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), dataEncode);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {

            }
            return ret;
        }
        #endregion

        #region 处理JSON
        static IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
        static readonly string TimeFormater = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";

        static AssistClass()
        {
            //解决时间中带T问题
            timeConverter.DateTimeFormat = TimeFormater;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string SerializeObject(object o)
        {
            string jsonStr = JsonConvert.SerializeObject(o, Formatting.None, timeConverter);
            return jsonStr;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string jsonStr)
        {
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }
        #endregion

        #region DateTime与UTC互转
        /// <summary>
        /// DateTime转UTC时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long DateTime2UTC(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return Convert.ToInt64((time - startTime).TotalSeconds * 1000);
        }

        /// <summary>
        /// UTC转DateTime
        /// </summary>
        /// <param name="utc"></param>
        /// <returns></returns>
        public static DateTime UTC2Datetime(long utc)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            startTime = startTime.AddSeconds(utc / 1000);
            return startTime.AddHours(8);//转化为北京时间(北京时间=UTC时间+8小时 )
        }
        #endregion

        /// <summary>
        /// 四舍五入
        /// </summary>
        /// <param name="SourceVal">传入浮点数字</param>
        /// <param name="bitVal">返回位数</param>
        /// <returns>返回指定位数的浮点数字</returns>
        public static double DoubleRound(double SourceVal, int bitVal)
        {
            return Math.Round(SourceVal, bitVal, MidpointRounding.AwayFromZero);
        }
        
        /// <summary>
        /// 取出Html标签
        /// </summary>
        /// <param name="html"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ReplaceHtmlTag(string html, int length = 0)
        {
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }

        /// <summary>
        /// Linq分页跳过数据条数
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static int FYSkip(int pageNumber, int pageSize)
        {
            int a = (pageNumber - 1) * pageSize;
            return a < 0 ? 0 : a;
        }

        /// <summary>
        /// 判断Web传参不能为空，且值都不能是空、null、空白字符
        /// </summary>
        /// <param name="nvc">hc.Request.Form</param>
        /// <returns>是否满足：参数个数大于0，且值不是空、null、空白字符</returns>
      public static bool CheckFormParams(NameValueCollection nvc)
      {
          if (nvc.Count == 0)
          {
              return false;
          }
          else {
              foreach (string item in nvc)
              {
                  if (string.IsNullOrWhiteSpace(nvc[item]))
                  {
                      return false;
                  }
              }
          }
            return true;
      }
        //写判断指定参数可以为null、空的
    }
}
