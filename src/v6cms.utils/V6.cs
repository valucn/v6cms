using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace v6cms.utils
{
    public static class V6
    {
        /// <summary>
        /// 时间戳转换为日期（时间戳单位秒）
        /// </summary>
        /// <param name="TimeStamp"></param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(int timeStamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return start.AddSeconds(timeStamp).AddHours(8);
        }

        /// <summary>
        /// 时间戳转换为日期（时间戳单位秒）
        /// </summary>
        /// <param name="TimeStamp"></param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(long timeStamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return start.AddMilliseconds(timeStamp).AddHours(8);
        }

        /// <summary>
        /// 根据日期返回 星期(返回结果为中文)
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>星期</returns>
        public static string ConvertDateToZHWeek(DateTime date)
        {
            string week = string.Empty;
            DayOfWeek weekstr = new DateTime(date.Year, date.Month, date.Day).DayOfWeek;
            switch (weekstr.ToString())
            {
                case "Monday": week = "星期一"; break;
                case "Tuesday": week = "星期二"; break;
                case "Wednesday": week = "星期三"; break;
                case "Thursday": week = "星期四"; break;
                case "Friday": week = "星期五"; break;
                case "Saturday": week = "星期六"; break;
                case "Sunday": week = "星期天"; break;
            }
            return week;
        }

        public static string ChkSql(string str)
        {
            string result = "";
            if (str != null)
            {
                result = str.Replace("'", "&#39;");

                result = result.Replace("(", "（");
                result = result.Replace(")", "）");
                result = result.Replace("[", "【");
                result = result.Replace("]", "】");
                result = result.Replace("{", "｛");
                result = result.Replace("}", "｝");
                result = result.Replace(",", "，");
                result = result.Replace("--", "—");
                result = Regex.Replace(result, @"(U|u)(P|p)(D|d)(A|a)(T|t)(E|e)", "");
                result = Regex.Replace(result, @"(I|i)(N|n)(S|s)(E|e)(R|r)(T|t)", "");
                result = Regex.Replace(result, @"(C|c)(R|r)(E|e)(A|a)(T|t)(E|e)", "");
                result = Regex.Replace(result, @"(A|a)(L|l)(T|t)(E|e)(R|r)", "");
                result = Regex.Replace(result, @"(D|d)(R|r)(O|o)(P|p)", "");
                result = Regex.Replace(result, @"(T|t)(A|a)(B|b)(L|l)(E|e)", "");
                result = Regex.Replace(result, @"(D|d)(E|e)(L|l)(E|e)(T|t)(E|e)", "");
                result = Regex.Replace(result, @"(D|d)(E|e)(C|c)(L|l)(A|a)(R|r)(E|e)", "");
                result = Regex.Replace(result, @"(F|f)(R|r)(O|o)(M|m)", "");
            }
            return result;
        }

        public static string CutStr(this string str, int len)
        {
            if (str.Length > len)
            {
                str = str.Substring(0, len);
            }
            return str;
        }

        /// <summary>
        /// 去除HTML标记(用正则彻底去除HTML\CSS\script代码 )
        /// </summary>
        /// <param name="html_str">包括HTML的源码 </param>
        /// <returns>已经去除后的文字</returns>
        public static string NoHTML(this string html_str)
        {
            //删除脚本
            html_str = Regex.Replace(html_str, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除样式
            html_str = Regex.Replace(html_str, @"<style[^>]*?>.*?</style>", "", RegexOptions.IgnoreCase);
            //删除HTML
            html_str = Regex.Replace(html_str, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            html_str = Regex.Replace(html_str, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            html_str = Regex.Replace(html_str, @"-->", "", RegexOptions.IgnoreCase);
            html_str = Regex.Replace(html_str, @"<!--.*", "", RegexOptions.IgnoreCase);

            html_str = Regex.Replace(html_str, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            html_str = Regex.Replace(html_str, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            html_str = Regex.Replace(html_str, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            html_str = Regex.Replace(html_str, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            html_str = Regex.Replace(html_str, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            html_str = Regex.Replace(html_str, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            html_str = Regex.Replace(html_str, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            html_str = Regex.Replace(html_str, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            html_str = Regex.Replace(html_str, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            html_str = Regex.Replace(html_str, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            html_str.Replace("<", "");
            html_str.Replace(">", "");
            html_str.Replace("\r\n", "");
            //str = HttpContext.Current.Server.HtmlEncode(str).Trim();

            html_str = Regex.Replace(html_str, @"\s{1,}", " ", RegexOptions.IgnoreCase);//将多个连续的空格替换成一个空格
            html_str = html_str.Trim();

            return html_str;
        }

        public static string list2string(string[] list)
        {
            return string.Join(",", list.ToArray());
        }

        public static string list2string(List<string> list)
        {
            return string.Join(",", list.ToArray());
        }

        public static string sha1(string source, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            byte[] buffer = encoding.GetBytes(source);
            var sha = SHA1.Create();
            var hash = BitConverter.ToString(sha.ComputeHash(buffer)).Replace("-", "");
            return hash;
        }

        /// <summary>  
        /// 加密  
        /// </summary>  
        /// <param name="str"></param>  
        /// <param name="key"></param>  
        /// <returns></returns>  
        public static string Encode(string str, string key)
        {
            StringBuilder builder;
            using (DESCryptoServiceProvider provider = new DESCryptoServiceProvider())
            {
                provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
                provider.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8));
                byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(str);
                using (MemoryStream stream = new MemoryStream())
                {
                    CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
                    stream2.Write(bytes, 0, bytes.Length);
                    stream2.FlushFinalBlock();
                    builder = new StringBuilder();
                    foreach (byte num in stream.ToArray())
                    {
                        builder.AppendFormat("{0:X2}", num);
                    }
                }
            }
            return builder.ToString();
        }

        public static string get_page(string post_url, string post_data)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(post_data);
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(post_url) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                throw new Exception(err);
            }
        }

        public static void write_log(string memo)
        {
            write_log("log", memo);
        }

        /// <summary>
        /// 写日志(用于跟踪)
        /// </summary>
        public static void write_log(string file_name, string log_contents)
        {
            var now = DateTime.Now;
            string filename = Path.Combine("logs", $"{file_name}-{now.ToString("yyyy-MM-dd")}.log");
            if (!Directory.Exists(Path.Combine("logs")))
            {
                Directory.CreateDirectory(Path.Combine("logs"));
            }
            StreamWriter sr = null;
            try
            {
                if (!System.IO.File.Exists(filename))
                {
                    sr = System.IO.File.CreateText(filename);
                }
                else
                {
                    sr = System.IO.File.AppendText(filename);
                }
                sr.Write("==========================================\r\n" + now + "\r\n" + log_contents + "\r\n\r\n\r\n");
            }
            catch { }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
        }
    }
}
