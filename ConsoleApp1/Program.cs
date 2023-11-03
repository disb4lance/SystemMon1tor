using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Management;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Системный монитор. Нажмите enter для выхода.");
            while (!Console.KeyAvailable) { // пока не будет enter
                GetCpuUsage();
                //AvailableAndUsedRam();
                DiscInfo();
                Operating_System();
                Console.WriteLine("\n");
                DataTransmissionAndReception();
                Console.WriteLine("\nОтображение процессов, потребляющих память");
                ProcessDisplay();
                Console.WriteLine("\n\n");
                Console.Clear();
            }
        }


        private static void Operating_System() {
            Console.WriteLine("Операционная система: " + Environment.OSVersion.Platform.ToString());
            Console.WriteLine("Версия операционной системы: " + Environment.OSVersion.VersionString);
            Console.WriteLine("64 Bit операционная система? : {0}",
               Environment.Is64BitOperatingSystem ? "Да" : "Нет");
            Console.WriteLine("Имя компьютера : {0}",
               Environment.MachineName);
            Console.WriteLine("Число процессоров : {0}",
               Environment.ProcessorCount);
            Console.WriteLine("Системная папка : {0}",
               Environment.SystemDirectory);
            Console.WriteLine("Логические диски : {0}",
                Environment.GetLogicalDrives());
        }

        private static void DataTransmissionAndReception() {
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface networkInter in networkInterfaces)
            {
                if (networkInter == null)
                {
                    Console.WriteLine("Сетевой интерфейс не найден. Убедитесь, что вы указали правильное имя интерфейса.");
                    return;
                }
                else
                {
                    if (networkInter.Name == "Ethernet" || networkInter.Name == "Беспроводная сеть")
                    {
                        Console.WriteLine("Имя интерфейса: " + networkInter.Name);
                        Console.WriteLine($"Скорость передачи: {networkInter.GetIPStatistics().BytesSent / (1024 * 1024)} Мб/с");
                        Console.WriteLine($"Скорость приема: {networkInter.GetIPStatistics().BytesReceived / (1024 * 1024)} Мб/с");
                        Thread.Sleep(1000);
                    }
                }
            }
        }
        private static void DiscInfo() {
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                Console.WriteLine($"Диск: {drive.Name}");
                Console.WriteLine($"Тип диска: {drive.DriveType}");
                Console.WriteLine($"Файловая система: {drive.DriveFormat}");
                Console.WriteLine($"Общее дисковое пространство: {drive.TotalSize / (1024 * 1024 * 1024)} ГБ");
                Console.WriteLine($"Свободное дисковое пространство: {drive.AvailableFreeSpace / (1024 * 1024 * 1024)} ГБ");
                Console.WriteLine($"Занятое дисковое пространство: {(drive.TotalSize - drive.AvailableFreeSpace) / (1024 * 1024 * 1024)} ГБ");
                Console.WriteLine();
            }
        }
        private static void GetCpuUsage() {
            using (PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total")) // используем пространство имен using System.Diagnostics;)
            {
                cpuCounter.NextValue(); //          вызываем 2 раза и 
                Thread.Sleep(1000); //              берем паузу
                Console.WriteLine($"Загрузка ЦПУ: {cpuCounter.NextValue()}%");//    чтобы получить правдивые значения
            }
        }
        private static float GetCpuUsage(Process process)
        {
            using (PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total")) // используем пространство имен using System.Diagnostics;
            {
                cpuCounter.NextValue(); //          вызываем 2 раза и 
                Thread.Sleep(1000); //              берем паузу
                return cpuCounter.NextValue();//    чтобы получить правдивые значения
            }
        }
        private static void ProcessDisplay()
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
