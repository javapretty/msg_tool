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
        static private Node_Client node_client_ = null;
        static private string log_path_ = "./logs/log_";
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

        static public void set_form(Node_Client node_client)
        {
            node_client_ = node_client;
            if (Directory.Exists("./logs") == false)
            {
                Directory.CreateDirectory("./logs");
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
            if (node_client_ == null)
            {
                return;
            }
            if (node_client_.TextBoxLog.InvokeRequired == true)
            {
                node_client_.TextBoxLog.Invoke(debug_log_handler_, new object[] { logstr, log_mode });
            }
            else
            {
                if (log_mode == 0 || log_mode == 1)//日志模式为正常或只显示界面日志
                {
                    if (node_client_.TextBoxLog.GetLineFromCharIndex(node_client_.TextBoxLog.Text.Length) > 1000)
                    {
                        node_client_.TextBoxLog.Text = "";
                    }
                    node_client_.TextBoxLog.AppendText(DateTime.Now.ToString("HH:mm:ss ") + logstr + "\r\n");
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
            if (node_client_ == null)
            {
                return;
            }
            node_client_.TextBoxLog.Text = "";
        }

        public static string get_line_and_name()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(true);
            return st.GetFrame(2).GetFileName() + ":" + st.GetFrame(2).GetFileLineNumber().ToString();
        }  
    }
}
