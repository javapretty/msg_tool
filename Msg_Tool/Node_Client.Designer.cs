namespace Msg_Tool
{
    partial class Node_Client
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Text_Box_IP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Text_Box_Port = new System.Windows.Forms.TextBox();
            this.Connect_Button = new System.Windows.Forms.Button();
            this.TextBoxLog = new System.Windows.Forms.TextBox();
            this.Text_Box_Cmd = new System.Windows.Forms.TextBox();
            this.Button_Send = new System.Windows.Forms.Button();
            this.Button_Disconnect = new System.Windows.Forms.Button();
            this.Text_Box_Name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Button_Init_Conf = new System.Windows.Forms.Button();
            this.Text_Box_Robot_Num = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Text_Box_Send_Interval = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Text_Box_Run_Time = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Button_Robot_Login = new System.Windows.Forms.Button();
            this.Button_Reset_Robot = new System.Windows.Forms.Button();
            this.Radio_Sec = new System.Windows.Forms.RadioButton();
            this.Radio_Msec = new System.Windows.Forms.RadioButton();
            this.Text_Box_Login_Interval = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Text_Box_IP
            // 
            this.Text_Box_IP.Location = new System.Drawing.Point(73, 12);
            this.Text_Box_IP.Name = "Text_Box_IP";
            this.Text_Box_IP.Size = new System.Drawing.Size(100, 21);
            this.Text_Box_IP.TabIndex = 0;
            this.Text_Box_IP.Text = "10.1.8.226";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "ip地址:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(186, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "端口:";
            // 
            // Text_Box_Port
            // 
            this.Text_Box_Port.Location = new System.Drawing.Point(231, 12);
            this.Text_Box_Port.Name = "Text_Box_Port";
            this.Text_Box_Port.Size = new System.Drawing.Size(100, 21);
            this.Text_Box_Port.TabIndex = 3;
            this.Text_Box_Port.Text = "8000";
            // 
            // Connect_Button
            // 
            this.Connect_Button.Location = new System.Drawing.Point(546, 12);
            this.Connect_Button.Name = "Connect_Button";
            this.Connect_Button.Size = new System.Drawing.Size(75, 23);
            this.Connect_Button.TabIndex = 4;
            this.Connect_Button.Text = "连接";
            this.Connect_Button.UseVisualStyleBackColor = true;
            this.Connect_Button.Click += new System.EventHandler(this.Connect_Button_Click);
            // 
            // TextBoxLog
            // 
            this.TextBoxLog.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxLog.Location = new System.Drawing.Point(27, 49);
            this.TextBoxLog.Multiline = true;
            this.TextBoxLog.Name = "TextBoxLog";
            this.TextBoxLog.ReadOnly = true;
            this.TextBoxLog.Size = new System.Drawing.Size(332, 425);
            this.TextBoxLog.TabIndex = 5;
            this.TextBoxLog.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Text_Box_Double_Click);
            // 
            // Text_Box_Cmd
            // 
            this.Text_Box_Cmd.Location = new System.Drawing.Point(378, 401);
            this.Text_Box_Cmd.Multiline = true;
            this.Text_Box_Cmd.Name = "Text_Box_Cmd";
            this.Text_Box_Cmd.Size = new System.Drawing.Size(412, 73);
            this.Text_Box_Cmd.TabIndex = 6;
            this.Text_Box_Cmd.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Text_Box_Cmd_Double_Click);
            // 
            // Button_Send
            // 
            this.Button_Send.Location = new System.Drawing.Point(715, 480);
            this.Button_Send.Name = "Button_Send";
            this.Button_Send.Size = new System.Drawing.Size(75, 23);
            this.Button_Send.TabIndex = 7;
            this.Button_Send.Text = "发送";
            this.Button_Send.UseVisualStyleBackColor = true;
            this.Button_Send.Click += new System.EventHandler(this.Button_Send_Click);
            // 
            // Button_Disconnect
            // 
            this.Button_Disconnect.Location = new System.Drawing.Point(633, 12);
            this.Button_Disconnect.Name = "Button_Disconnect";
            this.Button_Disconnect.Size = new System.Drawing.Size(75, 23);
            this.Button_Disconnect.TabIndex = 8;
            this.Button_Disconnect.Text = "断开";
            this.Button_Disconnect.UseVisualStyleBackColor = true;
            this.Button_Disconnect.Click += new System.EventHandler(this.Button_Disconnect_Click);
            // 
            // Text_Box_Name
            // 
            this.Text_Box_Name.Location = new System.Drawing.Point(426, 12);
            this.Text_Box_Name.Name = "Text_Box_Name";
            this.Text_Box_Name.Size = new System.Drawing.Size(100, 21);
            this.Text_Box_Name.TabIndex = 9;
            this.Text_Box_Name.Text = "testplayer";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(352, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "登录角色名:";
            // 
            // Button_Init_Conf
            // 
            this.Button_Init_Conf.Location = new System.Drawing.Point(715, 12);
            this.Button_Init_Conf.Name = "Button_Init_Conf";
            this.Button_Init_Conf.Size = new System.Drawing.Size(75, 23);
            this.Button_Init_Conf.TabIndex = 11;
            this.Button_Init_Conf.Text = "更新配置";
            this.Button_Init_Conf.UseVisualStyleBackColor = true;
            this.Button_Init_Conf.Click += new System.EventHandler(this.Button_Init_Conf_Click);
            // 
            // Text_Box_Robot_Num
            // 
            this.Text_Box_Robot_Num.Location = new System.Drawing.Point(449, 344);
            this.Text_Box_Robot_Num.Name = "Text_Box_Robot_Num";
            this.Text_Box_Robot_Num.Size = new System.Drawing.Size(34, 21);
            this.Text_Box_Robot_Num.TabIndex = 13;
            this.Text_Box_Robot_Num.Text = "10";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(376, 349);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "机器人数量:";
            // 
            // Text_Box_Send_Interval
            // 
            this.Text_Box_Send_Interval.Location = new System.Drawing.Point(650, 344);
            this.Text_Box_Send_Interval.Name = "Text_Box_Send_Interval";
            this.Text_Box_Send_Interval.Size = new System.Drawing.Size(32, 21);
            this.Text_Box_Send_Interval.TabIndex = 15;
            this.Text_Box_Send_Interval.Text = "2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(589, 349);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "发送间隔:";
            // 
            // Text_Box_Run_Time
            // 
            this.Text_Box_Run_Time.Location = new System.Drawing.Point(748, 344);
            this.Text_Box_Run_Time.Name = "Text_Box_Run_Time";
            this.Text_Box_Run_Time.Size = new System.Drawing.Size(34, 21);
            this.Text_Box_Run_Time.TabIndex = 17;
            this.Text_Box_Run_Time.Text = "3600";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(687, 349);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 16;
            this.label6.Text = "运行时间:";
            // 
            // Button_Robot_Login
            // 
            this.Button_Robot_Login.Location = new System.Drawing.Point(625, 372);
            this.Button_Robot_Login.Name = "Button_Robot_Login";
            this.Button_Robot_Login.Size = new System.Drawing.Size(75, 23);
            this.Button_Robot_Login.TabIndex = 18;
            this.Button_Robot_Login.Text = "机器人登录";
            this.Button_Robot_Login.UseVisualStyleBackColor = true;
            this.Button_Robot_Login.Click += new System.EventHandler(this.Button_Robot_Login_Click);
            // 
            // Button_Reset_Robot
            // 
            this.Button_Reset_Robot.Location = new System.Drawing.Point(709, 372);
            this.Button_Reset_Robot.Name = "Button_Reset_Robot";
            this.Button_Reset_Robot.Size = new System.Drawing.Size(75, 23);
            this.Button_Reset_Robot.TabIndex = 19;
            this.Button_Reset_Robot.Text = "机器人下线";
            this.Button_Reset_Robot.UseVisualStyleBackColor = true;
            this.Button_Reset_Robot.Click += new System.EventHandler(this.Button_Reset_Robot_Click);
            // 
            // Radio_Sec
            // 
            this.Radio_Sec.AutoSize = true;
            this.Radio_Sec.Checked = true;
            this.Radio_Sec.Location = new System.Drawing.Point(454, 374);
            this.Radio_Sec.Name = "Radio_Sec";
            this.Radio_Sec.Size = new System.Drawing.Size(35, 16);
            this.Radio_Sec.TabIndex = 20;
            this.Radio_Sec.TabStop = true;
            this.Radio_Sec.Text = "秒";
            this.Radio_Sec.UseVisualStyleBackColor = true;
            // 
            // Radio_Msec
            // 
            this.Radio_Msec.AutoSize = true;
            this.Radio_Msec.Location = new System.Drawing.Point(504, 374);
            this.Radio_Msec.Name = "Radio_Msec";
            this.Radio_Msec.Size = new System.Drawing.Size(47, 16);
            this.Radio_Msec.TabIndex = 21;
            this.Radio_Msec.Text = "毫秒";
            this.Radio_Msec.UseVisualStyleBackColor = true;
            // 
            // Text_Box_Login_Interval
            // 
            this.Text_Box_Login_Interval.Location = new System.Drawing.Point(549, 344);
            this.Text_Box_Login_Interval.Name = "Text_Box_Login_Interval";
            this.Text_Box_Login_Interval.Size = new System.Drawing.Size(34, 21);
            this.Text_Box_Login_Interval.TabIndex = 23;
            this.Text_Box_Login_Interval.Text = "1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(488, 349);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 22;
            this.label7.Text = "登录间隔:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(377, 377);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 24;
            this.label8.Text = "时间单位：";
            // 
            // Node_Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 521);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.Text_Box_Login_Interval);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.Radio_Msec);
            this.Controls.Add(this.Radio_Sec);
            this.Controls.Add(this.Button_Reset_Robot);
            this.Controls.Add(this.Button_Robot_Login);
            this.Controls.Add(this.Text_Box_Run_Time);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Text_Box_Send_Interval);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Text_Box_Robot_Num);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Button_Init_Conf);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Text_Box_Name);
            this.Controls.Add(this.Button_Disconnect);
            this.Controls.Add(this.Button_Send);
            this.Controls.Add(this.Text_Box_Cmd);
            this.Controls.Add(this.TextBoxLog);
            this.Controls.Add(this.Connect_Button);
            this.Controls.Add(this.Text_Box_Port);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Text_Box_IP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Node_Client";
            this.Text = "Node_Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Text_Box_IP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Text_Box_Port;
        private System.Windows.Forms.Button Connect_Button;
        public System.Windows.Forms.TextBox TextBoxLog;
        private System.Windows.Forms.TextBox Text_Box_Cmd;
        private System.Windows.Forms.Button Button_Send;
        private System.Windows.Forms.Button Button_Disconnect;
        private System.Windows.Forms.TextBox Text_Box_Name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Button_Init_Conf;
        private System.Windows.Forms.TextBox Text_Box_Robot_Num;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Text_Box_Send_Interval;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Text_Box_Run_Time;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button Button_Robot_Login;
        private System.Windows.Forms.Button Button_Reset_Robot;
        private System.Windows.Forms.RadioButton Radio_Sec;
        private System.Windows.Forms.RadioButton Radio_Msec;
        private System.Windows.Forms.TextBox Text_Box_Login_Interval;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}

