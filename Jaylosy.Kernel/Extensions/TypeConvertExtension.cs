using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jaylosy.Kernel.Extensions
{
    public static class TypeConvertExtension
    {
        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <param name="obj">待转换的对象</param>
        /// <param name="ret">默认值</param>
        /// <returns></returns>
        public static string AsString(this object obj, string ret = "")
        {
            if (obj == null)
                return ret;
            return obj.ToString();
        }
        /// <summary>
        /// 转换成Int32类型
        /// </summary>
        /// <param name="obj">待转换的对象</param>
        /// <param name="ret">默认值</param>
        /// <returns></returns>
        public static int AsInt(this object obj, int ret = 0)
        {
            if (obj == null)
                return ret;
            try
            {
                return int.Parse(obj.AsString());
            }
            catch
            {

                return ret;
            }
        }
        /// <summary>
        /// 转换成Int64类型
        /// </summary>
        /// <param name="obj">待转换的对象</param>
        /// <param name="ret">默认值</param>
        /// <returns></returns>
        public static long AsLong(this object obj, long ret = 0)
        {
            if (obj == null)
                return ret;
            try
            {
                return long.Parse(obj.AsString());
            }
            catch
            {

                return ret;
            }
        }
        /// <summary>
        /// 转换成DateTime类型
        /// </summary>
        /// <param name="obj">待转换的对象</param>
        /// <returns></returns>
        public static DateTime AsDateTime(this object obj)
        {
            if (obj == null)
                return DateTime.Parse("1900-01-01");
            try
            {
                return DateTime.Parse(obj.AsString());
            }
            catch
            {

                return DateTime.Parse("1900-01-01");
            }
        }
        /// <summary>
        /// 转换成DateTime类型
        /// </summary>
        /// <param name="obj">待转换的对象</param>
        /// <param name="ret">默认值</param>
        /// <returns></returns>
        public static DateTime AsDateTime(this object obj, DateTime ret)
        {
            if (obj == null)
                return ret;
            try
            {
                return DateTime.Parse(obj.AsString());
            }
            catch
            {

                return ret;
            }
        }
        /// <summary>
        /// 转换成布尔类型
        /// </summary>
        /// <param name="obj">待转换的对象</param>
        /// <param name="ret">默认值</param>
        /// <returns></returns>
        public static bool AsBoolean(this object obj, bool ret = false)
        {
            if (obj == null)
                return ret;
            try
            {
                return bool.Parse(obj.AsString());
            }
            catch
            {

                return ret;
            }
        }
    }
}
