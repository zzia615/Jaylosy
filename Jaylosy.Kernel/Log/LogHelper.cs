using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Jaylosy.Kernel.Configurations;
namespace Jaylosy.Kernel.Log
{
    /// <summary>
    /// 自定义记录日志的类
    /// </summary>
    public class LogHelper
    {
        AppSettings AppSettings = new AppSettings();
        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="obj"></param>
        public void Info(object obj)
        {
            //格式化字符串格式，并赋值
            string msg = string.Format("Info {0}", obj);
            //调用写日志方法
            WriteMsgToFile(msg);
        }
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="obj"></param>
        public void Error(object obj)
        {
            //格式化字符串格式，并赋值
            string msg = string.Format("Error {0}", obj);
            //调用写日志方法
            WriteMsgToFile(msg);
        }
        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="obj"></param>
        public void Warn(object obj)
        {
            //格式化字符串格式，并赋值
            string msg = string.Format("Warn {0}", obj);
            //调用写日志方法
            WriteMsgToFile(msg);
        }
        /// <summary>
        /// 将日志内容写入文件
        /// </summary>
        /// <param name="msg"></param>
        void WriteMsgToFile(string msg)
        {
            try
            {
                //格式化日志内容并赋值
                string content = string.Format("{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msg);
                //格式化日志文件名并赋值
                string logFileName = string.Format("{0}.log", DateTime.Now.ToString("yyyyMMdd"));
                //拼接日志文件名的完整路径+名称
                string logFullFileName = Path.Combine(AppSettings.LogDir, logFileName);
                //如果储存日志文件的文件夹不存在，则创建文件夹
                if (!Directory.Exists(AppSettings.LogDir))
                {
                    Directory.CreateDirectory(AppSettings.LogDir);
                }
                //读取文件流
                FileStream fs = File.Open(logFullFileName, FileMode.OpenOrCreate, FileAccess.Write);
                //写入文件内容的位置（追加）
                fs.Position = fs.Length;
                //如果文件有内容，则新的日志内容需要换行写入
                if (fs.Length > 0)
                {
                    content = string.Format("\r\n{0}", content);
                }
                //以Utf-8的字符编码读取要写入的日志文件内容
                byte[] bytes = Encoding.UTF8.GetBytes(content);
                //将字节写入到文件
                fs.Write(bytes, 0, bytes.Length);
                //释放文件流
                fs.Flush();
                //关闭文件流
                fs.Close();
            }
            catch (Exception ex)
            {
                //抛出异常
                throw ex;
            }

        }
    }
}
