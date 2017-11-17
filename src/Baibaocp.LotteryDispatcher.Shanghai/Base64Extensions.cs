using System;
using System.IO;
using System.Text;

namespace Baibaocp.Extensions
{
    /// <summary>
    /// Base64转码扩展
    /// </summary>
    public static class Base64Extensions
    {
        /// <summary>
        /// 将数据流转为base64字符串
        /// </summary>
        /// <param name="input">待转换数据</param>
        /// <returns>base64字符串</returns>
        public static string ToBase64(this MemoryStream input)
        {
            return ToBase64(input);
        }

        /// <summary>
        /// 将字节数组转换为base64字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToBase64(this byte[] input)
        {
            return Convert.ToBase64String(input);
        }

        /// <summary>
        /// 将base64字符串转换为内存流
        /// </summary>
        /// <param name="input">base64字符串</param>
        /// <returns>内存流</returns>
        public static byte[] FromBase64(this string input)
        {
            byte[] bytes = Convert.FromBase64String(input);
            return bytes;
        }

        /// <summary>
        /// base64编码解密
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string DecodeBase64(string code)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(code); //将2进制编码转换为8位无符号整数数组.   
            try
            {
                decode = Encoding.Default.GetString(bytes); //将指定字节数组中的一个字节序列解码为一个字符串。   
            }
            catch
            {
                decode = code;
            }
            return decode;
        }
    }
}
