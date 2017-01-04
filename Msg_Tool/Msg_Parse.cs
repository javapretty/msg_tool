/*
 * Msg_Parse.cs
 *
 *  Created on: Dec 12, 2016
 *      Author: zhangyalei
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Msg_Tool
{
    class Msg_Parse
    {
        static private Msg_Parse instance_ = null;
        static private string cmd_input_pattern_ = "@[0-9]+";
        static private string cmd_input_pattern2 = "^[0-9]*$";
        static private string cmd_seq_pattern_ = "[0-9]+ *=> *";
        static private string random_pattern_ = "#<[0-9]+, *[0-9]+> *";
        static private Dictionary<int, string> cmd_list_ = new Dictionary<int, string>();

        static public Msg_Parse instance
        {
            get
            {
                if (instance_ == null)
                    instance_ = new Msg_Parse();
                return instance_;
            }
        }

        private Msg_Parse()
        { 
        }

        static public void load_cmd_list(string path, bool clear_map = true)
        {
            if (clear_map)
            {
                cmd_list_.Clear();
            }
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line = null;
            while ((line = sr.ReadLine()) != null)
            {
                Match match = Regex.Match(line, cmd_seq_pattern_);
                string[] cmd_seq_str = match.Groups[0].ToString().Split('=');
                int cmd_seq = int.Parse(cmd_seq_str[0]);
                string cmd_string = line.Replace(match.Groups[0].ToString(), "");
                cmd_list_[cmd_seq] = cmd_string;
            }
            sr.Close();
        }

        static public void parse_cmd_and_send_buffer(string cmd)
        {
            Bit_Buffer buffer = new Bit_Buffer();

            try
            {
                JObject jsonobj = null;
                Match match = Regex.Match(cmd, cmd_input_pattern_);
                string cmd_string = match.Groups[0].ToString();
                if ( cmd_string != "")
                {
                    string[] cmd_str = cmd_string.Split('@');
                    int cmd_seq = int.Parse(cmd_str[1]);
                    Log.debug_log("脚本命令序列:" + cmd_seq.ToString() + " => " + Msg_Parse.parse_cmd_string(cmd_list_[cmd_seq]));
                    Game_Manager.instance.send_to_server(cmd_seq);
                    return;
                }
                else if (Regex.IsMatch(cmd, cmd_input_pattern2)) 
                {
                    int cmd_seq = int.Parse(cmd);
                    Log.debug_log("脚本命令序列:" + cmd_seq.ToString() + " => " + Msg_Parse.parse_cmd_string(cmd_list_[cmd_seq]));
                    Game_Manager.instance.send_to_server(cmd_seq);
                    return;
                }
                else
                {
                    string real_cmd = Msg_Parse.parse_cmd_string(cmd);
                    jsonobj = (JObject)JsonConvert.DeserializeObject(real_cmd);
                }
                uint msg_id = uint.Parse(jsonobj["msg_id"].ToString());
                if (msg_id < 5)
                {
                    Log.debug_log("小于5的命令号是系统命令号");
                    return;
                }
                Msg_Struct msg = Struct_Manager.instance.get_send_msg_struct((int)msg_id);
                
                if (-1 == msg.scan(buffer, jsonobj) || msg == null)
                {
                    throw new Exception("命令参数错误");
                }
                Game_Manager.instance.send_to_server(msg_id, buffer);
            }
            catch (Exception ex)
            {
                Log.debug_log(ex.Message);
            }
        }

        static public string parse_cmd_string(string ori)
        {
            string ret = ori;
            Match match = Regex.Match(ori, random_pattern_);
            string match_str = match.Groups[0].ToString();
            if (match_str != "")
            {
                string[] rand_str = match.Groups[0].ToString().Replace("#<", "").Replace(">", "").Split(',');
                Random ran = new Random();
                int ran_num = ran.Next(int.Parse(rand_str[0]), int.Parse(rand_str[1]));
                ret = ori.Replace(match.Groups[0].ToString(), ran_num.ToString());
            }
            return ret;
        }

        static public JObject get_cmd_jsonobj(int seq)
        {
            string cmd = parse_cmd_string(cmd_list_[seq]);
            JObject jsonobj = (JObject)JsonConvert.DeserializeObject(cmd);
            return jsonobj;
        }

        static public int get_cmd_random()
        {
            Random ran = new Random();
            int seq = ran.Next(0, cmd_list_.Count);
            int cmd_id = cmd_list_.Keys.ToList()[seq];
            return cmd_id;
        }
    }
}
