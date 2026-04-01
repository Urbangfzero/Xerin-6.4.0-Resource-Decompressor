using System;
using System.Threading;

namespace Core
{
    public static class Logger
    {
        private const string BannerText = @"
  =============================================
       Xerin v6.4.0 Resource Decompressor
  =============================================
";

        public static void Banner(int delay = 1) 
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            foreach (char c in BannerText)
            {
                Console.Write(c);
                Thread.Sleep(delay); 
            }

            Console.ResetColor();
        }

        public static void Info(string msg) => Console.WriteLine("[*] " + msg);
        public static void Success(string msg) => Console.WriteLine("[+] " + msg);
        public static void Error(string msg) => Console.WriteLine("[-] " + msg);
        public static void Warn(string msg) => Console.WriteLine("[!] " + msg);
    }
}