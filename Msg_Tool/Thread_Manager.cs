using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Msg_Tool
{
    class Thread_Manager
    {
        static private Thread_Manager instance_ = null;
        private Thread time_tick_thread_ = null;
        static public object time_moni = new object();

        static public Thread_Manager instance
        {
            get
            {
                if (instance_ == null)
                    instance_ = new Thread_Manager();
                return instance_;
            }
        }

        private Thread_Manager() 
        { 
        }

        public void start_timer()
        {
            time_tick_thread_ = new Thread(new ThreadStart(timer_thread_woker));
            time_tick_thread_.IsBackground = true;
            time_tick_thread_.Start();
            Log.debug_log("定时器线程启动");
        }

        private void timer_thread_woker()
        {
            int ret = 0;
            while((ret = Game_Manager.instance.time_tick()) == 0);
            Log.debug_log("定时器线程退出");
        }

        public void wait(int msec)
        {
            Thread.Sleep(msec);
        }
    }
}
