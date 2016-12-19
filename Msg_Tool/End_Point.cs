using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Msg_Tool
{
    class End_Point
    {
        private bool connect_status_ = false;
        private Socket socket_ = null;
        private Player player_ = null;
        private byte[] recv_bytes = new byte[65535];
        private string ip_;
        private int port_;
        private byte[] len_data_ = null;
        private Byte_Buffer buffer_data_ = null;
        private int remain_ = 0;
        private int merge_state_ = 0;//0:不需要组合 1：需要组合len 2：需要组合包体

        public End_Point(Player player) 
        {
            player_ = player;
        }

        public bool connect_status
        {
            get
            {
                return connect_status_;
            }
            set
            {
                connect_status_ = value;
            }
        }

        public byte[] len_data
        {
            get { return len_data_; }
            set { len_data_ = value; }
        }

        public Byte_Buffer buffer_data
        {
            get { return buffer_data_; }
            set { buffer_data_ = value; }
        }

        public int remain
        {
            get { return remain_; }
            set { remain_ = value; }
        }

        public int merge_state
        {
            get { return merge_state_; }
            set { merge_state_ = value; }
        }

        public bool connect(string ip, int port) 
        {
            try
            {
                ip_ = ip;
                port_ = port;
                IPEndPoint remote = new IPEndPoint(IPAddress.Parse(ip_), port_);      //远程服务器端地址； 
                socket_ = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket_.BeginConnect(remote, connect_callback, this);              //调用connect方法连接远端服务器；
             
                return true;
            }
            catch (Exception ex)
            {
                Log.debug_log(ex.Message);
                return false;
            }
        }

        static private void connect_callback(IAsyncResult ar)
        {
            End_Point ep = (End_Point)ar.AsyncState;
            try
            {
                ep.socket_.EndConnect(ar);
                ep.connect_status = true;
                ep.player_.player_log("连接" + ep.ip_ + ":" + ep.port_.ToString() + "成功");
                if (ep.player_.robot_status != 0)
                {
                    ep.player_.req_connect_gate();
                }
                else 
                {
                    Game_Manager.instance.add_player(ep.player_);
                    ep.player_.req_select_gate();
                }
                ep.recv();
            }
            catch(Exception ex)
            {
                Log.debug_log(ex.Message);
                ep.player_.player_log("连接" + ep.ip_ + ":" + ep.port_.ToString() + "失败");
            }
        }

        public bool disconnect()
        {
            if (connect_status_)
            {
                connect_status_ = false;
                socket_.Close();
                player_.player_log("和" + ip_ + ":" + port_.ToString() + "的连接断开。。。。。。");
                return true;
            }
            return false;
        }

        public int send(Byte_Buffer buffer)
        {
            send(buffer.rdata(), buffer.readable_length());
            return 0;
        }

        public int send(byte[] bytes, int length)
        {
            try
            {
                if (!connect_status_)
                    return -1;

                socket_.BeginSend(bytes, 0, length, 0, send_callback, this);
                return 0;
            }
            catch (Exception ex) 
            {
                //Log.debug_log(ex.Message);
                return -1;
            }
        }

        static private void send_callback(IAsyncResult ar)
        {
            End_Point ep = (End_Point)ar.AsyncState;
            try
            {
                int ret = ep.socket_.EndSend(ar);
                ep.player_.player_log("向服务器发送了" + ret + "字节数据");
            }
            catch (Exception ex)
            {
                //Log.debug_log(ex.Message);
            }
        }

        public int recv()
        {
            try
            {
                socket_.BeginReceive(recv_bytes, 0, 65535, 0, recv_callback, this);
                return 0;
            }
            catch (Exception ex)
            {
                //Log.debug_log(ex.Message);
                return -1;
            }
        }

        static private void recv_callback(IAsyncResult ar)
        {
            End_Point ep = (End_Point)ar.AsyncState;
            try
            {
                int ret = ep.socket_.EndReceive(ar);
                if (ret > 0)
                {
                    ep.player_.player_log("从服务器接收" + ret + "字节数据");
                    Byte_Buffer buffer = ep.get_buffer(ret);
                    Game_Manager.instance.process_buffer(ep, buffer);
                    ep.recv();
                }
            }
            catch (Exception ex)
            {
                ep.disconnect();
                if (ep.player_.robot_status != 0)
                {
                    Game_Manager.instance.rmv_player(ep.player_);
                }
                //Log.debug_log(ex.Message);
            }
        }

        private Byte_Buffer get_buffer(int length)
        {
            Byte_Buffer buffer = new Byte_Buffer();
            buffer.copy(recv_bytes, length);
            return buffer;
        }

        public void send_to_server(uint cmd_id, Bit_Buffer buffer)
        {
            Byte_Buffer buf = new Byte_Buffer();
            buf.make_tcp_begin();
            buf.write_uint8(cmd_id);
            buf.copy(buffer);
            buf.make_tcp_end();
            send(buf);
        }
    }
}
