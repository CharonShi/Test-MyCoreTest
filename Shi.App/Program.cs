using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shi.App
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ThreadTest(); // 线程练习


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


            //进制转换
            //ScaleMian();





            //while (true)
            //{
            //    var erjinzhi = Console.ReadLine();
            //    erzhuanshi(erjinzhi);
            //}


            Console.ReadKey();

            System.Environment.Exit(0);


            return;
        }

        #region 线程练习

        static object locker = new object();
        static long mark = 0;

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
                //consoleWrite($" 第一个方法：mark:{mark}");
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
                //consoleWrite($"第二个方法：mark:{mark}");
            }
        }


        static void consoleWrite(string content)
        {
            lock (locker)
            {
                mark++;
                Console.WriteLine(content);
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

        /// <summary>
        /// 未完成
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
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


        #region 进制转换


        #region 进制转换入口


        public static void ScaleMian()
        {

            Console.WriteLine("");
            Console.WriteLine(" * * * * * * * * * * * * * * * * *");
            Console.WriteLine("请选择要使用的功能:");
            Console.WriteLine("- - - - - - - - - - - - - - - - - - -");
            Console.WriteLine("");
            Console.WriteLine("1、二进制转十进制");
            Console.WriteLine("2、十进制转二进制");
            Console.WriteLine("3、二进制转八进制");
            Console.WriteLine("4、八进制转二进制");
            Console.WriteLine("5、十进制转八进制");
            Console.WriteLine("6、八进制转十进制");
            Console.WriteLine("7、二进制转十六进制");
            Console.WriteLine("8、十六进制转二进制");
            Console.WriteLine("");
            Console.WriteLine("- - - - - - - - - - - - - - - - - - -");
            Console.WriteLine("");

            Console.Write("我选择：");
            var states = Console.ReadLine();

            states = states.ToUpper();

            switch (states)
            {
                case "1":
                    while (true)
                    {
                        Console.Write("请输入一个二进制数：");
                        var erjinzhi = Console.ReadLine();
                        Map(erjinzhi);
                        erzhuanshi(erjinzhi);
                        Console.WriteLine("");
                    }
                    break;
                case "2":

                    while (true)
                    {
                        Console.Write("请输入一个十进制数：");
                        var shijizhi = Console.ReadLine();
                        Map(shijizhi);
                        shizhuaner_(shijizhi);
                        //Console.Write(" 从右往左读");
                        Console.WriteLine("");
                    }
                    break;
                case "3":

                    while (true)
                    {
                        Console.Write("请输入一个二进制数：");
                        var shijizhi = Console.ReadLine();
                        Map(shijizhi);
                        erzhuanba(shijizhi);
                        Console.WriteLine("");
                    }
                    break;
                case "4":

                    while (true)
                    {
                        Console.Write("请输入一个八进制数：");
                        var bajinzhi = Console.ReadLine();
                        Map(bajinzhi);
                        bazhuaner(bajinzhi);
                        Console.WriteLine("");
                    }
                    break;
                case "5":

                    while (true)
                    {
                        Console.Write("请输入一个十进制数：");
                        var shijinzhi = Console.ReadLine();
                        Map(shijinzhi);
                        shizhuanba(shijinzhi);
                        Console.WriteLine("");
                    }
                    break;
                case "6":

                    while (true)
                    {
                        Console.Write("请输入一个八进制数：");
                        var bajinzhi = Console.ReadLine();
                        Map(bajinzhi);
                        bazhuanshi(bajinzhi);
                        Console.WriteLine("");
                    }
                    break;
                case "7":

                    while (true)
                    {
                        Console.Write("请输入一个二进制数：");
                        var erjinzhi = Console.ReadLine();
                        Map(erjinzhi);
                        erzhuanshiliu(erjinzhi);
                        Console.WriteLine("");
                    }
                    break;
                case "8":

                    while (true)
                    {
                        Console.Write("请输入一个十六进制数：");
                        var shiliujinzhi = Console.ReadLine();
                        Map(shiliujinzhi);
                        shiliuzhuaner(shiliujinzhi);
                        Console.WriteLine("");
                    }
                    break;


                case "EXIT":
                    System.Environment.Exit(0);
                    break;
                case "BACK":
                    ScaleMian();
                    break;
                default:
                    Console.WriteLine("");
                    Console.Write("***输出错误，请重新输入***");
                    Console.WriteLine("");
                    ScaleMian();
                    break;
            }
        }



        public static string Map(string serch)
        {

            serch = serch.ToUpper();

            switch (serch)
            {
                case "EXIT":
                    System.Environment.Exit(0);
                    return serch;
                    break;
                case "BACK":
                    ScaleMian();
                    return serch;
                    break;
                default:
                    return serch;
                    break;
            }


        }

        #endregion





        #region 十&二

        /// <summary>
        /// 十进制转二进制 递归 但是得到的结果需要翻转过来看
        /// </summary>
        /// <param name="shijinzhi">十进制数</param>
        /// <returns></returns>
        public static int shizhuaner(int shijinzhi)
        {

            var a = shijinzhi % 2; // 模，取余数
            var b = shijinzhi / 2; // 除，取剩余数
            if (b <= 0)
            {
                Console.Write(a);
                return 0;
            }
            Console.Write(a);

            return shizhuaner(b);
        }

        /// <summary>
        /// 十进制转二进制 循环
        /// </summary>
        /// <param name="shijinzhi">十进制数</param>
        /// <returns></returns>
        public static void shizhuaner_(string shijinzhi)
        {
            var numbe = Convert.ToInt32(shijinzhi);
            var result = "";

            while (numbe > 0)
            {
                var a = numbe % 2; // 模，取余数
                numbe = numbe / 2; // 除，取剩余数

                result = a + result;
            }

            Console.WriteLine($"十进制转换二进制的结果为：{result}");
        }

        /// <summary>
        /// 二进制转十进制
        /// </summary>
        /// <param name="erjinzhi">二进制数</param>
        public static void erzhuanshi(string erjinzhi)
        {
            //字符串顺序翻转
            //var arr = erjinzhi.ToCharArray();
            //Array.Reverse(arr);
            //var str = new string(arr);

            double result = 0;

            for (int i = 0, j = erjinzhi.Length - 1; i < erjinzhi.Length; i++, j--)
            {
                var erjin = Convert.ToInt32(erjinzhi[j].ToString()); // 单个数字
                var rrr = Math.Pow(2, i);  // 2取j次方
                result += erjin * rrr;
            }

            Console.WriteLine($"二进制转换十进制的结果为：{result}");
        }

        #endregion



        #region 八&二

        /// <summary>
        /// 二进制转八进制
        /// ************
        /// 将二进制数从右往左读，每三个一组，最后一组若是不足三个数，则补零至三个数
        /// 每组从右往左计算，如：100，计算为 1*2^2 + 0*2^1 + 0*2^0 
        /// 每组计算的结果拼在一起（不相加），则得到8进驻数
        /// ************
        /// </summary>
        /// <param name="erjinzhi"></param>
        public static void erzhuanba(string erjinzhi)
        {
            List<string> arr = new List<string>();

            string a = "";
            for (int i = erjinzhi.Length - 1, j = 0; i >= 0; i--, j++)
            {
                a += erjinzhi[i].ToString();
                if ((j + 1) % 3 == 0)
                {
                    //var arara = a.ToCharArray();
                    //var aasw = arara.Reverse();
                    //a = string.Join(null, aasw);
                    arr.Add(a);
                    a = "";
                }
            }
            if (!string.IsNullOrWhiteSpace(a))
            {
                //var arara = a.ToCharArray();
                //var aasw = arara.Reverse();
                //a = string.Join(null, aasw);
                a = a.PadRight(3, '0');
                arr.Add(a);
            }

            var result = "";
            foreach (var item in arr)
            {
                var ara = item.ToArray();

                double mark = 0;
                for (int i = 0; i < ara.Length; i++)
                {
                    mark += Convert.ToInt32(ara[i].ToString()) * Math.Pow(2, i);
                }
                result = mark + result;
            }

            Console.WriteLine($"二进制转八进制结果：{result}");
        }

        /// <summary>
        /// 八进制转二进制
        /// ************
        /// 将八进制数的每个数字拿出来，进行除二取余法，将得到的余数拼起来，不足三个数补零至三个数
        /// 最后把每个数字得到的二进制结果拼在一起，得到完整二进制数
        /// 二进制数最前面的0没有意义，可以去除
        /// ************
        /// </summary>
        /// <param name="bajinzhi"></param>
        public static void bazhuaner(string bajinzhi)
        {

            var arr = bajinzhi.ToArray();

            var result = "";
            for (int i = 0; i < arr.Length; i++)
            {
                var rer = "";
                var mark = Convert.ToInt32(arr[i].ToString());

                while (mark > 0) // 被除数小于等于0，跳出循环
                {
                    var aa = mark % 2;  // 取余
                    mark = mark / 2;  //取相除的结果，做下次循环的被除数
                    rer = aa + rer; // 拼接此次循环的二进制结果
                }

                if (rer.Length < 3) // 不足三个数字补零
                {
                    rer = rer.PadLeft(3, '0');
                }

                result += rer; // 最后得到的二进制数
            }

            while (result.StartsWith('0'))
            {
                result = result.Remove(0, 1);
            }

            Console.WriteLine($"八进制转二进制结果为：{result}");
        }


        #endregion



        #region 十六&二


        /// <summary>
        /// 二进制转十六进制
        /// *************
        /// 将二进制数从右往左开始，每四个一组，不足四个补零至4个数
        /// 每一组数从右往左开始，乘以2的下标次方，累计这组数的和，如
        /// 二进制数 10010110，分组得 1001、0110 ， 计算 1*2^3 + 0*2^2 + 0*2^1 + 1*2^0  、 0*2^3 + 1*2^2 + 1*2^1 + 0*2^0 
        /// 最后拼接每组的结果就是16进制数， 96 
        /// 十六进制的 10--15 用ABCDEF来表示
        /// *************
        /// </summary>
        /// <param name="erjinzhi"></param>
        public static void erzhuanshiliu(string erjinzhi)
        {

            List<string> fire = new List<string>();
            var result = "";

            var arr = erjinzhi.ToArray();

            var a = "";
            for (int i = arr.Length - 1, j = 0; i >= 0; i--, j++)
            {
                a += arr[i].ToString();
                if ((j + 1) % 4 == 0) // 每四个一组
                {
                    fire.Add(a);
                    a = "";
                }
            }
            if (!string.IsNullOrWhiteSpace(a)) //不足4个数补零
            {
                a = a.PadRight(4, '0');
                fire.Add(a);
            }

            foreach (var item in fire)
            {
                var aa = item.ToString();

                double mark = 0;
                for (int i = 0; i < aa.Length; i++)
                {
                    mark += Convert.ToInt32(aa[i].ToString()) * Math.Pow(2, i);
                }

                result = shiliuzim(mark) + result;
            }
            Console.WriteLine($"二进制转十六进制结果为：{result}");
        }

        /// <summary>
        /// 十六进制转二进制
        /// ************
        /// 将十六进制数拆分开，每个数字使用除二取余法，将余数从后往前拼接，拼够四个数字，不够的补零
        /// 把十六进制每个数字计算出的二进制拼接起来，得到二进制数
        /// *************
        /// </summary>
        /// <param name="shiliujinzhi"></param>
        public static void shiliuzhuaner(string shiliujinzhi)
        {
            var arr = shiliujinzhi.ToArray();

            var result = "";
            for (int i = arr.Length - 1; i >= 0; i--)
            {
                var item = arr[i].ToString();
                var a = shiliushuzi(item);

                var mark = "";
                while (a > 0)
                {
                    var b = a % 2;
                    a = a / 2;
                    mark = b + mark;
                }
                if (mark.Length < 4)
                {
                    mark = mark.PadLeft(4, '0');
                }

                result = mark + result;
            }

            while (result.StartsWith('0'))
            {
                result = result.Remove(0, 1);
            }

            Console.WriteLine($"十六进制转二进制的结果为：{result}");
        }



        /// <summary>
        /// 十六进制10--15项为字母
        /// </summary>
        /// <param name="shil"></param>
        /// <returns></returns>
        public static string shiliuzim(double shil)
        {
            switch (shil)
            {
                case 10: return "A";
                case 11: return "B";
                case 12: return "C";
                case 13: return "D";
                case 14: return "E";
                case 15: return "F";
                default: return shil.ToString();
            }
        }
        public static int shiliushuzi(string shil)
        {
            shil = shil.ToUpper();
            switch (shil)
            {
                case "A": return 10;
                case "B": return 11;
                case "C": return 12;
                case "D": return 13;
                case "E": return 14;
                case "F": return 15;
                default: return Convert.ToInt32(shil);
            }
        }

        #endregion



        #region 十&八

        /// <summary>
        /// 十进制转八进制
        /// *********
        /// 通过除8取余法，将得到的余数从后往前拼起来，得到八进制数
        /// 
        /// *********
        /// </summary>
        /// <param name="shijinzhi"></param>
        public static void shizhuanba(string shijinzhi)
        {
            var result = "";
            var mark = Convert.ToInt32(shijinzhi);

            while (mark > 0)
            {
                var yu = mark % 8;
                mark = mark / 8;
                result = yu + result;
            }

            Console.WriteLine($"十进制转换八进制的结果为：{result}");
        }


        /// <summary>
        /// 八进制转十进制
        /// ***********
        /// 将八进制数的每个数字拿出，从右往左开始，当前数字乘以8的下标，如：
        /// 把八进制数 226，得到 2*8^2 + 2*8^1 + 6*8^0 
        /// 最后得到的十进制数结果
        /// ***********
        /// </summary>
        /// <param name="bajinzhi"></param>
        public static void bazhuanshi(string bajinzhi)
        {

            double result = 0;
            var arr = bajinzhi.ToArray();

            for (int i = arr.Length - 1, j = 0; i >= 0; i--, j++)
            {
                var a = Convert.ToInt32(arr[i].ToString());
                var bb = a * Math.Pow(8, j);
                result += bb;
            }

            Console.WriteLine($"八进制转换十进制的结果为：{result}");
        }

        #endregion



        #endregion


    }
}
