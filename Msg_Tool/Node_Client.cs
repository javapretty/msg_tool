using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Msg_Tool
{
    public partial class Node_Client : Form
    {
        public Node_Client()
        {
            InitializeComponent();
            Log.set_form(this);
            if (Game_Manager.instance.init_conf() != 0)
            {
                
            }
            Thread_Manager.instance.start_timer();
        }

        private void Connect_Button_Click(object sender, EventArgs e)
        {
            if (Game_Manager.instance.status())
            {
                Log.debug_log("连接已建立");
                return;
            }
 
            string name = Text_Box_Name.Text;
            if (name == "")
            {
                Log.debug_log("请输入正确的用户名");
                return;
            }
            Game_Manager.instance.init_user(name);

            string ip = Text_Box_IP.Text;
            int port = int.Parse(Text_Box_Port.Text);
            Game_Manager.instance.connect(ip, port);
        }

        private void Text_Box_Double_Click(object sender, MouseEventArgs e)
        {
            Log.clear_log();
        }

        private void Button_Send_Click(object sender, EventArgs e)
        {
            if (!Game_Manager.instance.status()) 
            {
                Log.debug_log("请先连接服务器");
                return;
            }
            if (Text_Box_Cmd.Text == "")
            {
                Log.debug_log("请输入需要发送的数据");
                return;
            }

            Msg_Parse.parse_cmd_and_send_buffer(Text_Box_Cmd.Text);
        }

        private void Text_Box_Cmd_Double_Click(object sender, MouseEventArgs e)
        {
            Text_Box_Cmd.Text = "";
        }

        private void Button_Disconnect_Click(object sender, EventArgs e)
        {
            Game_Manager.instance.fini_user();
        }

        private void Button_Init_Conf_Click(object sender, EventArgs e)
        {
            Game_Manager.instance.init_conf();
        }

        private void Button_Robot_Login_Click(object sender, EventArgs e)
        {
            Game_Manager.instance.robot_num = int.Parse(Text_Box_Robot_Num.Text);
            Game_Manager.instance.send_interval = long.Parse(Text_Box_Send_Interval.Text) * (Radio_Sec.Checked == true ? 1000 : 1);
            Game_Manager.instance.login_interval = long.Parse(Text_Box_Login_Interval.Text) * (Radio_Sec.Checked == true ? 1000 : 1);
            Game_Manager.instance.run_time = long.Parse(Text_Box_Run_Time.Text) * (Radio_Sec.Checked == true ? 1000 : 1);
            Game_Manager.instance.cent_ip = Text_Box_IP.Text;
            Game_Manager.instance.cent_port = int.Parse(Text_Box_Port.Text);
            Game_Manager.instance.begin_robot();
        }

        private void Button_Reset_Robot_Click(object sender, EventArgs e)
        {
            Game_Manager.instance.reset_robot();
        }
    }
}
