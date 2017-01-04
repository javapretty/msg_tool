/*
 * Player.cs
 *
 *  Created on: Dec 12, 2016
 *      Author: zhangyalei
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Msg_Tool
{
    class Player
    {
        private string account_;
        private string token_;
        private long login_time_ = 0;
        private long last_heartbeat_tick_ = 0;
        private long last_sendmsg_tick_ = 0;
        private int server_tick_ = 0;
        private long send_interval_ = 0;
        private Endpoint end_point_ = null;
        private int robot_status_ = 0;//0:准备连接center 1:准备连接gate 2:连上gate并收到返回消息
        private bool is_player_ = false;
        private bool robot_log_ = false;

        public Player(bool robot_log)
        {
            account_ = get_ran_name();
            is_player_ = false;
            robot_log_ = robot_log;
            end_point_ = new Endpoint(this);
        }

        public Player(string role_name)
        {
            account_ = role_name;
            end_point_ = new Endpoint(this);
            is_player_ = true;
        }

        private string get_ran_name()
        {
            Random ran = new Random();
            int robot_seq = ran.Next(1, 999999);
            return "Robot_" + robot_seq.ToString();
        }

        public bool is_player
        {
            get
            {
                return is_player_;
            }
        }

        public int robot_status
        {
            get
            {
                return robot_status_;
            }
        }

        public Endpoint end_point
        {
            get 
            {
                return end_point_; 
            }
        }

        public string role_name
        {
            get
            {
                return account_;
            }
        }

        public void do_tick(long tick)
        {
            if (!end_point.connect_status)
                return;

            if (tick - last_heartbeat_tick_ >= 5000)
            {
                last_heartbeat_tick_ = tick;
                req_heartbeat(tick);
            }

            if (!is_player)
            {
                //机器人未登陆前不发消息
                if (robot_status_ != 2)
                    return;

                if (login_time_ == 0)
                    login_time_ = tick;
                Random ran = new Random();
                if (tick - last_sendmsg_tick_ >= send_interval_)
                {
                    last_sendmsg_tick_ = tick;
                    send_msg_tick();
                    send_interval_ = (long)((double)Game_Manager.instance.send_interval * ( 0.8 + 0.4 * ran.NextDouble()));
                }
                if (tick - login_time_ >= Game_Manager.instance.run_time)
                {
                    login_time_ = 0;
                    logout_tick(tick);
                }
            }
        }

        private void send_msg_tick()
        {
            req_any_data(Msg_Parse.get_cmd_random());
        }

        private void logout_tick(long tick)
        {
            Game_Manager.instance.push_drop_player(this);
        }

        public void req_heartbeat(long tick)
        {
            if (robot_status_ != 2 || !end_point_.connect_status)
                return;

            Bit_Buffer buffer = new Bit_Buffer();
            buffer.write_int((int)(tick / 1000), 32);
            end_point_.send_to_server(Enum.REQ_HEARTBEAT, buffer);
            player_log("同步心跳至服务器");
        }

        public void req_select_gate() 
        {
            Bit_Buffer buffer = new Bit_Buffer();
            buffer.write_string(account_);
            end_point_.send_to_server(Enum.REQ_SELECT_GATE, buffer);
        }

        public int res_select_gate(Bit_Buffer buffer)
        {
            string gate_ip = buffer.read_string();
            uint gate_port = buffer.read_uint(16);
            token_ = buffer.read_string();
            player_log("\r\n" +
                "{" + "\r\n" +
                "  gate_ip:" + gate_ip+ "\r\n" +
                "  gate_port:" + gate_port.ToString() + "\r\n" + 
                "  token:" + token_ + "\r\n" +
                "}");
            robot_status_ = 1;//准备连接gate服务器
            end_point_.disconnect();
            end_point_.connect(gate_ip, (int)gate_port);
            return 1;
        }

        public int req_connect_gate()
        {
            Bit_Buffer buffer = new Bit_Buffer();
            buffer.write_string(account_);
            buffer.write_string(token_);
            end_point_.send_to_server(Enum.REQ_CONNECT_GATE, buffer);
            return 0;
        }

        public int res_connect_gate(Bit_Buffer buffer)
        {
            player_log("登录成功");
            req_fetch_role();
            return 0;
        }

        public int res_heartbeat(Bit_Buffer buffer)
        {
            server_tick_ = buffer.read_int(32);
            player_log("服务器心跳时间:" + server_tick_.ToString());
            return 0;
        }

        public int res_error_code(Bit_Buffer buffer)
        {
            uint code = buffer.read_uint(16);
            if (code == 1)
            {
                req_create_role();
                player_log("创建角色:" + account_);
            }
            else
            {
                string err_msg = Error_Code.message(code);
                if (err_msg != "")
                {
                    player_log("服务器返回错误消息:" + err_msg);
                }
                else {
                    player_log("服务器返回错误代码:" + code.ToString());
                }
            }
            return 0;
        }

        public int req_fetch_role()
        {
            Bit_Buffer buffer = new Bit_Buffer();
            buffer.write_string(account_);
            end_point_.send_to_server(Enum.REQ_FETCH_ROLE, buffer);
            return 0;
        }

        public int req_create_role()
        {
            Random ran = new Random();
            Bit_Buffer buffer = new Bit_Buffer();
            buffer.write_string(account_);
            buffer.write_string(account_);
            buffer.write_uint((uint)ran.Next(0,2), 1);
            buffer.write_uint((uint)ran.Next(0, 3), 2);
            end_point_.send_to_server(Enum.REQ_CREATE_ROLE, buffer);
            return 0;
        }

        public int res_role_info(uint msg_id, Bit_Buffer buffer)
        {
            Msg_Struct msg = Struct_Manager.instance.get_recv_msg_struct((int)msg_id);
            if (msg == null)
                return 0;
            player_log(msg.print_msg(buffer));
            robot_status_ = 2;//已经连接上gate准备发消息
            return 0;
        }

        public int res_recv_data(uint msg_id, Bit_Buffer buffer)
        {
            Msg_Struct msg = Struct_Manager.instance.get_recv_msg_struct((int)msg_id);
            if (msg == null)
                return 0;
            player_log(msg.print_msg(buffer));
            return 0;
        }

        public void req_any_data(int seq)
        {
            try
            {
                if (!end_point.connect_status)
                    return;
                JObject obj = Msg_Parse.get_cmd_jsonobj(seq);
                uint msg_id = uint.Parse(obj["msg_id"].ToString());
                if (msg_id < 5)
                {
                    player_log("小于5的命令号是系统命令号");
                    return;
                }
                Bit_Buffer buffer = new Bit_Buffer();
                Msg_Struct msg = Struct_Manager.instance.get_send_msg_struct((int)msg_id);
                msg.scan(buffer, obj);

                if (-1 == msg.scan(buffer, obj) || msg == null)
                {
                    throw new Exception("命令参数错误");
                }
                end_point_.send_to_server(msg_id, buffer);
            }
            catch (Exception ex)
            {
                Log.debug_log(ex.Message);
            }
        }

        public void player_log(string logstr)
        {
            if (is_player)
            {
                Log.debug_log("[" + account_ + "]: " + logstr, 0);
            }
            else if (robot_log_)
            {
                Log.debug_log("[" + account_ + "]: " + logstr, 2);
            }
            else
            {
                return;
            }
        }
    }
}
