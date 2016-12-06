using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Msg_Tool
{
    class Log
    {
        static private Node_Client node_client_ = null;
        static private string log_path_ = "./logs/log_";
        private delegate void debug_log_handler(string str, bool showmsg = true);
        static private debug_log_handler debug_log_handler_ = new debug_log_handler(debug_log);
        
        private Log()
        { 
        }

        static public void set_form(Node_Client node_client)
        {
            node_client_ = node_client;
            if (Directory.Exists("./logs") == false)
            {
                Directory.CreateDirectory("./logs");
            }
        }

        static public void debug_log(string logstr, bool showmsg = true)
        {
            if (node_client_ == null)
            {
                return;
            }
            if (node_client_.TextBoxLog.InvokeRequired == true)
            {
                node_client_.TextBoxLog.Invoke(debug_log_handler_, new object[] { logstr, showmsg });
            }
            else 
            {
                if (showmsg)
                {
                    if (node_client_.TextBoxLog.GetLineFromCharIndex(node_client_.TextBoxLog.Text.Length) > 1000)
                    {
                        node_client_.TextBoxLog.Text = "";
                    }
                    node_client_.TextBoxLog.AppendText(DateTime.Now.ToString("HH:mm:ss ") + logstr + "\r\n");
                }
                write_to_file(logstr);
            }
        }

        static private void write_to_file(string logstr)
        {
            DateTime dt = DateTime.Now;
            string str = dt.ToString("yyyy-MM-dd");
            string log_path = log_path_ + str + ".txt";
            System.IO.StreamWriter sw = System.IO.File.AppendText(log_path);
            sw.WriteLine(DateTime.Now.ToString("HH:mm:ss ") + logstr);
            sw.Close();
            sw.Dispose();
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
