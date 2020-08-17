using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Shi.App
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //ThreadTest(); // 线程练习


            //异步练习
            //Console.WriteLine("1");
            //Console.WriteLine("2");
            //Task1();
            //Console.WriteLine("3");
            //Console.WriteLine("4");
            //Console.WriteLine("5");


            //SetTaskAtFiexdTime(); //定时任务 每天
            //SetTaskMinutes(); // 每分钟



            //new Thread(new ThreadStart(ShowNowStatu)).Start();

            var shijizhi = Convert.ToInt32(Console.ReadLine());
            erjinzhi(shijizhi);

            Console.ReadKey();

            System.Environment.Exit(0);


            return;
        }

        #region 线程练习


        static async Task ThreadTest()
        {

            new Thread(new ThreadStart(TimeTime)).Start();//启动一条新的线程
            new Thread(new ThreadStart(Time1)).Start();

        }

        static async void TimeTime()
        {
            var slee = 0;
            while (true)
            {
                Thread.Sleep(1000);//将线程挂起指定时间（阻塞）  整个线程都会等待
                slee += 1;
                Console.WriteLine($"第一个方法：第{slee}秒");
            }
        }

        static void Time1()
        {
            var slee = 0;
            while (true)
            {
                Thread.Sleep(2000);
                slee += 2;
                Console.WriteLine($"第二个方法：第{slee}秒");
            }
        }

        #endregion


        #region 异步练习

        static async Task Task1()
        {
            Console.WriteLine("异步方法进入");
            await Task.Run(() => { Thread.Sleep(5000); });
            Console.WriteLine("异步方法结束");
        }


        #endregion




        #region 定时任务练习

        #region 每日执行

        /// <summary>
        /// 每日执行
        /// </summary>
        static void SetTaskAtFiexdTime()
        {
            DateTime nowDate = DateTime.Now; // 当前时间
            DateTime oneClock = DateTime.Today.AddHours(1); //目标时间

            if (oneClock < nowDate) // 若当前时间大于目标时间，则顺延至下一个时间
            {
                oneClock = nowDate.AddDays(1);
            }

            int muUntilFour = (int)((oneClock - nowDate).TotalMilliseconds);//获取目标时间与当前时间 的 间隔毫秒数

            var t = new System.Threading.Timer(doAtlAM);//要执行的目标方法

            t.Change(muUntilFour, Timeout.Infinite); // 更新计时器执行时间

        }
        /// <summary>
        /// 目标方法
        /// </summary>
        /// <param name="obj"></param>
        static void doAtlAM(object obj)
        {
            Console.WriteLine("定时到了");

            SetTaskAtFiexdTime();
        }

        #endregion

        #region  每分钟执行

        static void SetTaskMinutes()
        {
            DateTime nowData = DateTime.Now;
            DateTime targetDate = nowData.AddMinutes(1);

            int muUntilFour = (int)((targetDate - nowData).TotalMilliseconds);

            var t = new Timer(doAtlMinuter);

            t.Change(muUntilFour, -1);

            Console.WriteLine("当前时间:" + nowData);
            Console.WriteLine("目标时间:" + targetDate);
        }

        static void doAtlMinuter(object obj)
        {
            Console.WriteLine("定时到了");

            SetTaskMinutes();
        }



        #endregion

        #endregion


        #region 递归练习


        public static int pow1(int n)
        {
            if (n <= 2)
            {
                return 1;
            }

            return pow1(n - 1) + pow1(n - 2);
        }








        #endregion


        #region 获取计算机运行状态 cpu,内存....


        /*
        性能计数器 PerformanceCounter 类
        命名空间 System.Diagnostics
        初始化 PerformanceCounter 类的新的只读实例，并将其与本地计算机上指定的系统性能计数器或自定义性能计数器及类别实例关联
        常用的计数器类别有： “Cache”（缓存）、“Memory”（内存）、“Objects”（对象）、“PhysicalDisk”（物理磁盘）、“Process”（进程）
        、“Processor”（处理器）、“Server”（服务器）、“System”（系统）和“Thread”（线程）等
         */

        public static void ShowNowStatu()
        {

            //PerformanceCounter Cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total"); //处理器
            //PerformanceCounter PhysicalDisk = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total"); //物理磁盘
            PerformanceCounter NetWork = new PerformanceCounter(); //网络速度
            PerformanceCounter Cpu = PerfromCpu(); //处理器
            PerformanceCounter PhysicalDisk = PerfromDisk(); //物理磁盘
            PerformanceCounter Cache = PerfromCache(); //内存


            PerformanceCounterCategory performance = new PerformanceCounterCategory("NetWork Interface");
            var iName = performance.GetInstanceNames();
            PerformanceCounter[] netPerform = performance.GetCounters(iName[0]);
            NetWork = netPerform[0];

            while (true)
            {
                Thread.Sleep(2000);
                var cc = Cpu.NextValue().ToString("F2");
                var pp = PhysicalDisk.NextValue().ToString("F2");
                var nn = NetWork.NextValue().ToString("F2");
                var caa = Cache.NextValue().ToString("F2");
                Console.WriteLine("时间：" + DateTime.Now);
                Console.WriteLine($"Cpu：{cc}%   PhysicalDisk：{pp}%  NetWork：{nn}    Cache：{caa}");
                Console.WriteLine("");
            }


        }

        public static PerformanceCounter PerfromCpu()
        {
            PerformanceCounterCategory perform = new PerformanceCounterCategory("Processor"); // 创建一个性能计数器，指定类别
            var cName = perform.GetInstanceNames(); //检索与此类别关联的性能对象实例列表。
            PerformanceCounter[] cpuPerform = perform.GetCounters(cName[4]); //检索包含一个或多个实例的性能计数器类别中的计数器列表。
            return cpuPerform[0]; // 选择需要的计数器，可能会有按时间计数的、按大小计数的、按百分比计数的等等。。。。。。
        }
        public static PerformanceCounter PerfromDisk()
        {
            PerformanceCounterCategory perform = new PerformanceCounterCategory("PhysicalDisk");
            var dName = perform.GetInstanceNames();
            PerformanceCounter[] DiskPerform = perform.GetCounters(dName[0]);
            return DiskPerform[20];
        }
        public static PerformanceCounter PerfromCache()
        {
            PerformanceCounterCategory perform = new PerformanceCounterCategory("Cache");
            var dName = perform.GetInstanceNames();
            PerformanceCounter[] DiskPerform = perform.GetCounters(dName[0]);
            return DiskPerform[20];
        }




        #endregion


        #region 二进制



        public static int erjinzhi(int shijinzhi)
        {

            var a = shijinzhi % 2;
            var b = shijinzhi / 2;
            if (b <= 0)
            {
                Console.Write(a);
                return 0;
            }
            Console.Write(a);
            return erjinzhi(b);
        }





        #endregion


    }
}
