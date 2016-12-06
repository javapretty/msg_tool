using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace Msg_Tool
{
    class Msg_Struct
    {
        private string struct_name_ = "";
	    private int msg_id_ = 0;
	    private string msg_name_ = "";
        private List<field_info> field_list_ = new List<field_info>();

        public Msg_Struct(XmlNode node)
        {
            struct_name_ = node.Name;
            XmlAttributeCollection attrc = node.Attributes;
            foreach(XmlAttribute attr in attrc)
            {
                if(attr.Name == "msg_id")
                {
                    msg_id_ = int.Parse(attr.Value);    
                }
                else if (attr.Name == "msg_name")
                {
                    msg_name_ = attr.Value;
                }
            }

            if (node.HasChildNodes)
            {
                XmlNodeList sub_node_list = node.ChildNodes;
                foreach(XmlNode sub_node in sub_node_list)
                {
                    field_info info = init_field(sub_node);
                    field_list_.Add(info);
                }
            }
        }

        public string struct_name
        {
            get { return struct_name_; }
            set { struct_name_ = value; }
        }

        public int msg_id
        {
            get { return msg_id_; }
            set { msg_id_ = value; }
        }

        public string msg_name
        {
            get { return msg_name_; }
            set { msg_name_ = value; }
        }

        public List<field_info> field_list
        {
            get { return field_list_; }
        }

        field_info init_field(XmlNode node) {
            field_info info = new field_info();
		    info.field_label = node.Name;
            
            XmlAttributeCollection attrc = node.Attributes;
            foreach(XmlAttribute attr in attrc)
            {
                if(attr.Name == "type")
                {
                    info.field_type = attr.Value;
                }
                else if(attr.Name == "bit")
                {
                    info.field_bit = int.Parse(attr.Value);
                }
                else if(attr.Name == "name")
                {
                    info.field_name = attr.Value;
                }
                else if(attr.Name == "vbit")
                {
                    info.field_vbit = int.Parse(attr.Value);
                }
                else if(attr.Name == "key_type")
                {
                    info.key_type = attr.Value;
                }
                else if(attr.Name == "key_bit")
                {
                    info.key_bit = int.Parse(attr.Value);
                }
                else if(attr.Name == "key_name")
                {
                    info.key_name = attr.Value;
                }
                else if(attr.Name == "val")
                {
                    info.case_val = int.Parse(attr.Value);
                }
            }
		   
		    if (info.field_label == "if" || info.field_label == "switch" || info.field_label== "case") {
                if (node.HasChildNodes) {
			        XmlNodeList sub_node_list = node.ChildNodes;
				    foreach(XmlNode sub_node in sub_node_list)
                    {
					    field_info sub_field = init_field(sub_node);
                        info.field_list.Add(sub_field);
                    }
			    }
	       }
	       return info;
        }

        public int scan(Bit_Buffer buffer, JObject jobject)
        {
            try
            {
                return set_msg_buffer(field_list, buffer, jobject);
            }
            catch(Exception ex)
            {
                Log.debug_log(ex.Message);
                return -1;
            }
        }

        private int set_msg_buffer(List<field_info> info_list, Bit_Buffer buffer, JObject jobject)
        {
            foreach (field_info info in info_list)
            {
                if (info.field_label == "arg")
                {
                    JValue obj = (JValue)jobject[info.field_name];
                    set_arg_buffer(info, buffer, obj);
                }
                else if (info.field_label == "vector")
                {
                    JArray jarray = (JArray)jobject[info.field_name];
                    set_vector_buffer(info, buffer, jarray);
                 
                }
                else if (info.field_label == "struct")
                {
                    JObject obj = (JObject)jobject[info.field_name];
                    set_struct_buffer(info, buffer, obj);
              
                }
                else if(info.field_label == "if") {
			        bool field_exist = bool.Parse(jobject[info.field_name].ToString());
			        buffer.write_bool(field_exist);

			        if(field_exist) {
				        int ret = set_msg_buffer(info.field_list, buffer, jobject);
				        if(ret < 0) {
					        return ret;
				        }
			        }
		        }
		        else if(info.field_label == "switch") {
			        uint case_val = uint.Parse(jobject[info.field_name].ToString());
			        buffer.write_uint(case_val, info.field_bit);

			        foreach(field_info sub_info in info.field_list) {
				        if(sub_info.field_label == "case" && sub_info.case_val == case_val) {
					        //找到对应的case标签，对case标签内的child数组进行build
                            int ret = set_msg_buffer(sub_info.field_list, buffer, jobject);
					        if(ret < 0) {
						        return ret;
					        }
					        break;
				        }
			        }
		        }
            }
            return 0;
        }

        public string print_msg(Bit_Buffer buffer)
        {
            string msg = get_print_msg(field_list, buffer);
            if(msg != "")
                msg = msg.Substring(0, msg.Length - 2);
            return "\r\n" + struct_name + ":{\r\n" + msg + "\r\n}";
        }

        private string get_print_msg(List<field_info> info_list, Bit_Buffer buffer)
        { 
            string ret = "";
            foreach(field_info info in info_list) {
		        if(info.field_label == "arg") {
			        string value = get_arg_string(info, buffer);
			        ret += value;
		        }
                else if (info.field_label == "vector")
                {
                    string value = get_vector_string(info, buffer);
                    ret += value;
                }
                else if (info.field_label == "struct")
                {
                    string value = get_struct_string(info, buffer);
                    ret += value;
                }
                else if (info.field_label == "if")
                {
                    if (buffer.read_bits_available() >= 1 && buffer.read_bool())
                    {
                        bool field_exist = buffer.read_bool();
                        if (field_exist)
                        {
                            ret += get_print_msg(info.field_list, buffer);
                        }
                    }
                }
                else if (info.field_label == "switch")
                {
                    if (buffer.read_bits_available() >= info.field_bit)
                    {
                        uint case_val = buffer.read_uint(info.field_bit);
                        foreach (field_info swinfo in info.field_list)
                        {
                            if (swinfo.field_label == "case" && swinfo.case_val == case_val)
                            {
                                //找到对应的case标签，对case标签内的child数组进行build
                                ret += get_print_msg(swinfo.field_list, buffer);
                                break;
                            }
                        }
                    }
                }
	        }
            return ret;
        }

        private string get_arg_string(field_info info, Bit_Buffer buffer, bool from_vector = false)
        {
            string ret = "";
            if(!from_vector)
                ret = (info.field_name + ":");

            if(info.field_type == "int") 
            {
			    int val = buffer.read_int(info.field_bit);
			    ret += val.ToString();
	        }
	        else if(info.field_type == "uint")
            {
		        uint val = buffer.read_uint(info.field_bit);
			    ret += val.ToString();
            }
	        else if(info.field_type == "int64") 
            {
			    long val = buffer.read_int64();
			    ret += val.ToString();
	        }
	        else if(info.field_type == "uint64") 
            {
			    ulong val = buffer.read_uint64();
			     ret += val.ToString();
	        }
	        else if(info.field_type == "float")
            {
			    float val = buffer.read_decimal(32);
			    ret += val.ToString();
	        }
	        else if(info.field_type == "bool") 
            {
			    bool val = buffer.read_bool();
			     ret += val.ToString();
	        }
	        else if(info.field_type == "string") 
            {
			    string val = buffer.read_string();
                ret += "\"" + val + "\"";
	        }
            return ret + ", ";
        }

        private string get_vector_string(field_info info, Bit_Buffer buffer)
        {
            string ret = info.field_name + ":[";
	        uint length = buffer.read_uint(info.field_vbit);
	        for (uint i = 0; i < length; ++i) {
		        if(is_struct(info.field_type)) {
			        ret += get_struct_string(info, buffer, true);
		        }
		        else {
			        ret += get_arg_string(info, buffer, true);
		        }
	        }
            if(length > 0)
                ret = ret.Substring(0, ret.Length - 2);
            return ret + "], ";
        }

        private string get_struct_string(field_info info, Bit_Buffer buffer, bool from_vector = false)
        {
            string ret = "{";
            if (!from_vector)
                ret = (info.field_name + ":{");

            Msg_Struct msg = Struct_Manager.instance.get_msg_struct(info.field_type);
            string msg_str = msg.get_print_msg(msg.field_list, buffer);
            if(msg_str != "")
                msg_str = msg_str.Substring(0, msg_str.Length - 2);
            ret += msg_str;
            return ret + "}, ";
        }

        private bool is_struct(string field_type)
        {
            if (field_type == "int" || field_type == "uint" || field_type == "int64" || field_type == "uint64" ||
                    field_type == "float" || field_type == "bool" || field_type == "string") 
                return false;
            return true;
        }

        private int set_arg_buffer(field_info info, Bit_Buffer buffer, JValue value)
        {
            if (info.field_type == "int")
            {
                int val = int.Parse(value.ToString());
                buffer.write_int(val, info.field_bit);
            }
            else if (info.field_type == "uint")
            {
                uint val = uint.Parse(value.ToString());
                buffer.write_uint(val, info.field_bit);
            }
            else if (info.field_type == "int64")
            {
                long val = long.Parse(value.ToString());
                buffer.write_int64(val);
            }
            else if (info.field_type == "uint64")
            {
                ulong val = ulong.Parse(value.ToString());
                buffer.write_uint64(val);
            }
            else if (info.field_type == "float")
            {
                float val = float.Parse(value.ToString());
                buffer.write_decimal(val, 32);
            }
            else if (info.field_type == "bool")
            {
                bool val = bool.Parse(value.ToString());
                buffer.write_bool(val);
            }
            else if (info.field_type == "string")
            {
                string val = value.ToString();
                buffer.write_string(val);
            }
            return 0;
        }

        private int set_vector_buffer(field_info info, Bit_Buffer buffer, JArray jarray)
        {
            int ret = 0;
            uint length = (uint)jarray.Count;
            buffer.write_uint(length, info.field_vbit);
            for (uint i = 0; i < length; ++i)
            {
                if (is_struct(info.field_type))
                {
                    ret = set_struct_buffer(info, buffer, (JObject)jarray[i]);
                }
                else
                {
                    ret = set_arg_buffer(info, buffer, (JValue)jarray[i]);
                }
            }
            return 0;
        }

        private int set_struct_buffer(field_info info, Bit_Buffer buffer, JObject jobject)
        {
            Msg_Struct msg = Struct_Manager.instance.get_msg_struct(info.field_type);
	        if (msg == null) {
		        return -1;
	        }
            set_msg_buffer(msg.field_list, buffer, jobject);
            return 0;
        }
    }

    class field_info
    { 
        private string field_label_ = "";	   //字段标签
        private int field_vbit_ = 0;		    //数组字段长度位数
        private int case_val_ = 0;				//case标签对应的值
        private string field_type_ = "";		//字段类型
        private int field_bit_ = 0;				//字段位数
        private string field_name_ = "";		//字段名称
        private string key_type_ = "";		//主键类型
        private int key_bit_ = 0;				//主键位数
        private string key_name_ = "";		//主键名称
        private List<field_info> field_list_ = new List<field_info>();//if/switch使用存放子字段

        public string field_label
        {
            get { return field_label_; }
            set { field_label_ = value; }
        }

        public int field_vbit
        {
            get { return field_vbit_; }
            set { field_vbit_ = value; }
        }

        public int case_val
        {
            get { return case_val_; }
            set { case_val_ = value; }
        }

        public string field_type
        {
            get { return field_type_; }
            set { field_type_ = value; }
        }

        public int field_bit
        {
            get { return field_bit_; }
            set { field_bit_ = value; }
        }

        public string field_name
        {
            get { return field_name_; }
            set { field_name_ = value; }
        }

        public string key_type
        {
            get { return key_type_; }
            set { key_type_ = value; }
        }

        public int key_bit
        {
            get { return key_bit_; }
            set { key_bit_ = value; }
        }

        public string key_name
        {
            get { return key_name_; }
            set { key_name_ = value; }
        }

        public List<field_info> field_list
        {
            get { return field_list; }
        }
    }
}
