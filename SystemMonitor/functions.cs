using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SystemMonitor
{
    internal class functions
    {
        public static float GetCpuUsage()
        {
            using (PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total")) // используем пространство имен using System.Diagnostics;)
            {
                cpuCounter.NextValue(); //          вызываем 2 раза и 
                Thread.Sleep(1000); //              берем паузу
                //Console.WriteLine($"Загрузка ЦПУ: {cpuCounter.NextValue()}%");//    чтобы получить правдивые значения
                return cpuCounter.NextValue();
            }
        }
        public static float GetCpuUsage(Process process)
        {
            using (PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total")) // используем пространство имен using System.Diagnostics;
            {
                cpuCounter.NextValue(); //          вызываем 2 раза и 
                Thread.Sleep(1000); //              берем паузу
                return cpuCounter.NextValue();//    чтобы получить правдивые значения
            }
        }
        public static List<string> Operating_System()
        {
            List<string> operatingSystem = new List<string>();
            Console.WriteLine("Операционная система: " + Environment.OSVersion.Platform.ToString());
            Console.WriteLine("Имя компьютера : {0}",
               Environment.MachineName);
            Console.WriteLine("Число процессоров : {0}",
               Environment.ProcessorCount);
            operatingSystem.Add(Environment.OSVersion.Platform.ToString()); // операционная система
            operatingSystem.Add(Environment.MachineName); // имя пк
            operatingSystem.Add(Environment.ProcessorCount.ToString()); // колво процессоров
            return operatingSystem;
        }
        public static List<string> DataTransmissionAndReception()
        {
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            List<string> network = new List<string>();
            foreach (NetworkInterface networkInter in networkInterfaces)
            {
                if (networkInter == null)
                {
                    Console.WriteLine("Сетевой интерфейс не найден. Убедитесь, что вы указали правильное имя интерфейса.");
                    
                }
                else
                {
                    if (networkInter.Name == "Ethernet" || networkInter.Name == "Беспроводная сеть")
                    {
                        Console.WriteLine("Имя интерфейса: " + networkInter.Name);
                        Console.WriteLine($"Скорость передачи: {networkInter.GetIPStatistics().BytesSent / (1024 * 1024)} Мб/с");
                        Console.WriteLine($"Скорость приема: {networkInter.GetIPStatistics().BytesReceived / (1024 * 1024)} Мб/с");
                        network.Add(networkInter.Name); // имя интерфейса
                        network.Add((networkInter.GetIPStatistics().BytesSent / (1024 * 1024)).ToString()); // скорость передачи
                        network.Add((networkInter.GetIPStatistics().BytesReceived / (1024 * 1024)).ToString()); // скорость приема
                    }
                }
            }
            return network;
        }
        public static List<string> DiscInfo()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            List<string> diskinfo = new List<string>();

            foreach (DriveInfo drive in drives)
            {
                diskinfo.Add(drive.Name);
                diskinfo.Add((drive.TotalSize / (1024 * 1024 * 1024)).ToString());
                diskinfo.Add((drive.AvailableFreeSpace / (1024 * 1024 * 1024)).ToString());
                diskinfo.Add(((drive.TotalSize - drive.AvailableFreeSpace) / (1024 * 1024 * 1024)).ToString());
                break;
            }
            return diskinfo;
        }
        public static void ProcessDisplay()
        {
            Process[] processes = Process.GetProcesses();
            var sorted = processes.OrderByDescending(p => GetCpuUsage(p)); // сортируем по убыванию 
            var top10 = sorted.Take(10);// берем 10 самых загруженных 
            foreach (Process process in top10)
            {
                Console.WriteLine($"Имя процесса: {process.ProcessName}, нагрузка цпу: {GetCpuUsage(process)}, " +
                    $"потребление оперативной памяти {process.WorkingSet64 / (1024 * 1024)} Мб");
            }
            Thread.Sleep(1000);
        }

    }
}
