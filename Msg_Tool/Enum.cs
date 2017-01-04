/*
 * Enum.cs
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
using System.Text.RegularExpressions;

namespace Msg_Tool
{
    class Enum
    {
        public const uint REQ_HEARTBEAT = 1;  //发送心跳到gate_server
        public const uint RES_HEARTBEAT = 1;

        public const uint REQ_SELECT_GATE = 2;    //选择gate_server
        public const uint RES_SELECT_GATE = 2;

        public const uint REQ_CONNECT_GATE = 3;  //登录gate_server
        public const uint RES_CONNECT_GATE = 3;

        public const uint REQ_FETCH_ROLE = 4;  //获取角色
        public const uint RES_FETCH_ROLE = 4;

        public const uint REQ_CREATE_ROLE = 5;    //创建角色

        public const uint RES_ERROR_CODE = 5;    //返回错误号
    }

    class Error_Code
    {
        static private string error_code_pattern_ = "[0-9]+ *=> *";
        static private Dictionary<uint, string> error_list_ = new Dictionary<uint, string>();

        static public void load_error_code(string path, bool clear_map = true)
        {
            if (clear_map)
            {
                error_list_.Clear();
            }
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line = null;
            while ((line = sr.ReadLine()) != null)
            {
                Match match = Regex.Match(line, error_code_pattern_);
                string[] error_code_str = match.Groups[0].ToString().Split('=');
                uint error_code = uint.Parse(error_code_str[0]);
                string error_code_string = line.Replace(match.Groups[0].ToString(), "");
                error_list_[error_code] = error_code_string;
            }
            sr.Close();
        }

        static public string message(uint error_code)
        {
            if (error_list_.ContainsKey(error_code))
            {
                return error_list_[error_code];
            }
            else
            {
                return "";
            }
        }
    }
}
