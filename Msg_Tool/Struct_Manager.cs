/*
 * Struct_Manager.cs
 *
 *  Created on: Dec 12, 2016
 *      Author: zhangyalei
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Msg_Tool
{
    class Struct_Manager
    {
        static private Struct_Manager instance_ = null;
        private Dictionary<string, Msg_Struct> msg_name_map = new Dictionary<string, Msg_Struct>();
        private Dictionary<int, Msg_Struct> send_msg_id_map = new Dictionary<int, Msg_Struct>();
        private Dictionary<int, Msg_Struct> recv_msg_id_map = new Dictionary<int, Msg_Struct>(); 

        static public Struct_Manager instance
        {
            get
            {
                if (instance_ == null)
                    instance_ = new Struct_Manager();
                return instance_;
            }
        }

        private Struct_Manager()
        { 
        }

        public Msg_Struct get_send_msg_struct(int msg_id)
        {
            try
            {
                Msg_Struct msg = send_msg_id_map[msg_id];
                return msg;
            }
            catch (Exception ex)
            {
                Log.debug_log(ex.Message);
                Log.debug_log("send_msg_id_map 找不到关键字:" + msg_id.ToString());
                return null;
            }
        }

        public Msg_Struct get_recv_msg_struct(int msg_id)
        {
            try
            {
                Msg_Struct msg = recv_msg_id_map[msg_id];
                return msg;
            }
            catch (Exception ex)
            {
                Log.debug_log(ex.Message);
                Log.debug_log("recv_msg_id_map 找不到关键字:" + msg_id.ToString());
                return null;
            }
        }

        public Msg_Struct get_msg_struct(string struct_name)
        {
            try
            {
                Msg_Struct msg = msg_name_map[struct_name];
                return msg;
            }
            catch (Exception ex)
            {
                Log.debug_log(ex.Message);
                Log.debug_log("msg_name_map 找不到关键字:" + struct_name);
                return null;
            }
        }

        public void load_config(string path, bool clear_map = true)
        {
            try
            {
                if (clear_map)
                {
                    msg_name_map.Clear();
                    send_msg_id_map.Clear();
                    recv_msg_id_map.Clear();
                }

                XmlDocument doc = new XmlDocument();
                //加载xml配置时候忽略注释
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                XmlReader reader = XmlReader.Create(path, settings);
                doc.Load(reader);
                XmlNode root_node = doc.SelectSingleNode(doc.DocumentElement.Name);
                if (root_node != null)
                {
                    foreach (XmlNode node in root_node.ChildNodes)
                    {
                        Msg_Struct msg = new Msg_Struct(node);
                        msg_name_map.Add(msg.struct_name, msg);
                        if (msg.msg_id > 0)
                        {
                            if (msg.msg_name.IndexOf("RES") == 0)
                            {
                                recv_msg_id_map.Add(msg.msg_id, msg);
                            }
                            else if (msg.msg_name.IndexOf("REQ") == 0)
                            {
                                send_msg_id_map.Add(msg.msg_id, msg);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.debug_log(ex.Message);
            }
        }
    }
}
