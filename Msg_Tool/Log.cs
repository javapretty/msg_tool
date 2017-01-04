/*
 * Log.cs
 *
 *  Created on: Dec 12, 2016
 *      Author: zhangyalei
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Msg_Tool
{
    class Log
    {
        static private Msg_Tool msg_tool_ = null;
        static private string log_path_ = "./log/log_";
        private delegate void debug_log_handler(string str, int log_mode = 0);
        static private debug_log_handler debug_log_handler_ = new debug_log_handler(debug_log);
        static private System.IO.StreamWriter sw_ = null;
        
        private Log()
        { 
        }

        ~Log()
        {
            sw_.Close();
            sw_.Dispose();
        }

        static public void set_form(Msg_Tool msg_tool)
        {
            msg_tool_ = msg_tool;
            if (!Directory.Exists("./log"))
            {
                Directory.CreateDirectory("./log");
            }
        }

        static public void debug_log(string logstr, int log_mode = 0)
        {
            /*
             * 日志记录模式：
             * 0：界面 + 文件
             * 1：只界面
             * 2：只文件
             * 3：什么都不记录
            */
            if (msg_tool_ == null)
            {
                return;
            }

            if (msg_tool_.TextBoxLog.InvokeRequired)
            {
                msg_tool_.TextBoxLog.Invoke(debug_log_handler_, new object[] { logstr, log_mode });
            }
            else
            {
                if (log_mode == 0 || log_mode == 1)//日志模式为正常或只显示界面日志
                {
                    if (msg_tool_.TextBoxLog.GetLineFromCharIndex(msg_tool_.TextBoxLog.Text.Length) > 1000)
                    {
                        msg_tool_.TextBoxLog.Text = "";
                    }
                    msg_tool_.TextBoxLog.AppendText(DateTime.Now.ToString("HH:mm:ss ") + logstr + "\r\n");
                }
                if (log_mode == 0 || log_mode == 2)
                {
                    write_to_file(logstr);
                }
            }
        }

        static private void write_to_file(string logstr)
        {
            DateTime dt = DateTime.Now;
            if (sw_ == null)
            {
                int pid = Process.GetCurrentProcess().Id;
                string str = pid.ToString() + "_" + dt.ToString("yyyy-MM-dd");
                string log_path = log_path_ + str + ".txt";
                sw_ = System.IO.File.AppendText(log_path);
            }
            sw_.WriteLine(dt.ToString("HH:mm:ss ") + logstr);
            sw_.Flush();
        }

        static public void clear_log() 
        {
            if (msg_tool_ == null)
            {
                return;
            }
            msg_tool_.TextBoxLog.Text = "";
        }

        public static string get_line_and_name()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(true);
            return st.GetFrame(2).GetFileName() + ":" + st.GetFrame(2).GetFileLineNumber().ToString();
        }  
    }
}
