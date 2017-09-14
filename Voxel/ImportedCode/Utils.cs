using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Security.Principal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Ace
{
    /// <summary>
    /// 工具方法类
    /// </summary>
    public static class Utils
    {
	    /// <summary>
        /// 判断文件夹内是否存在指定的子目录
        /// </summary>
        /// <param name="directoryInfo">文件夹</param>
        /// <param name="subFolderName">子目录的名称</param>
        /// <returns>判断结果</returns>
        public static bool HasSubFolder(this DirectoryInfo directoryInfo, string subFolderName)
        {
            subFolderName = subFolderName.ToLower();
            foreach (var directory in directoryInfo.EnumerateDirectories())
            {
                if (directory.Name.ToLower() == subFolderName)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 判断文件夹内是否存在指定的文件
        /// </summary>
        /// <param name="directoryInfo">文件夹</param>
        /// <param name="fileName">文件的名称</param>
        /// <returns>判断结果</returns>
        public static bool HasFile(this DirectoryInfo directoryInfo, string fileName)
        {
            fileName = fileName.ToLower();
            foreach (var file in directoryInfo.EnumerateFiles())
            {
                if (file.Name.ToLower() == fileName)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 从路径创建新的RegistryKey
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="create">指示路径不存在时是否创建</param>
        /// <param name="writable">指示打开时的读写权限</param>
        /// <returns>注册表键</returns>
        public static RegistryKey OpenRegistryKey(string path, bool create = false, bool writable = false)
        {
            int firstBackslash = path.IndexOf(@"\");
            string hive;
            RegistryKey key = null;
            if (firstBackslash > -1)
            {
                hive = path.Substring(0, firstBackslash).ToUpper();
            }
            else
            {
                hive = path;
            }
            switch (hive)
            {
                case "HKEY_CLASSES_ROOT":
                    key = Registry.ClassesRoot; break;
                case "HKEY_CURRENT_USER":
                    key = Registry.CurrentUser; break;
                case "HKEY_LOCAL_MACHINE":
                    key = Registry.LocalMachine; break;
                case "HKEY_USERS":
                    key = Registry.Users; break;
                case "HKEY_CURRENT_CONFIG":
                    key = Registry.CurrentConfig; break;
                case "HKEY_PERFORMANCE_DATA":
                    key = Registry.PerformanceData; break;
                default:
                    key = null; break;
            }
            if (firstBackslash > -1 && key != null)
            {
                string subKey = path.Remove(0, firstBackslash + 1);
                if (create)
                {
                    key = key.CreateSubKey(subKey);
                }
                else
                {
                    key = key.OpenSubKey(subKey, writable);
                }
            }
            return key;
        }
        /// <summary>
        /// 获取文件夹的大小
        /// </summary>
        /// <param name="di">要获取大小的文件夹</param>
        /// <returns>文件夹的大小</returns>
        public static Task<DataSize> GetSizeAsync(this DirectoryInfo di)
        {
            return Task.Run(() =>
            {
                long size = 0;
                var subDirs = di.EnumerateDirectories();
                foreach (var dir in subDirs)
                {
                    size += dir.GetSizeAsync().Result.ToInt64();
                }
                var files = di.EnumerateFiles();
                foreach (var file in files)
                {
                    size += file.Length;
                }
                return new DataSize(size);
            });
        }
        /// <summary>
        /// 递归复制文件夹到新位置
        /// </summary>
        /// <param name="di">要复制的文件夹</param>
        /// <param name="path">新位置</param>
        /// <param name="exclude">排除项</param>
        /// <returns>关联的Task对象</returns>
        public static Task CopyTo(this DirectoryInfo di, string path)
        {
            return Task.Run(()=>{
                var dirs = di.EnumerateDirectories();
                foreach (var dir in dirs)
                {
                    dir.CopyTo(path.Backslash() + dir.Name);
                }
                var files = di.EnumerateFiles();
                foreach (var file in files)
                {
                    file.CopyTo(path.Backslash() + file.Name, true);
                }
            });
        }
        /// <summary>
        /// 获取系统内核版本，仅包含主次版本号和生成号
        /// </summary>
        public static Version OSVersion
        {
            get
            {
                string strVer = Command.GetOutput("ver").Output;
                strVer = strVer.Substring(strVer.IndexOf("版本 ") + 3);
                strVer = strVer.Remove(strVer.IndexOf("]"));
                string[] strVers = strVer.Split('.');
                int[] vers = new int[]{ int.Parse(strVers[0]), int.Parse(strVers[1]), int.Parse(strVers[2]), };
                Version ver = new Version(vers[0], vers[1], vers[2]);
                return ver;
            }
        }
        /// <summary>
        /// 将Version解析为Windows版本
        /// </summary>
        /// <param name="ver">要解析的Version</param>
        /// <returns>对应的Windows版本</returns>
        public static WindowsVersion GetWindowsVersion(this Version ver)
        {
            if (ver.Major == 6)
            {
                if (ver.Minor == 0) return WindowsVersion.Vista;
                else if (ver.Minor == 1)
                {
                    if (ver.Build == 7600) return WindowsVersion.Seven;
                    else if (ver.Build == 7601) return WindowsVersion.SevenSP1;
                }
                else if (ver.Minor == 2) return WindowsVersion.Eight;
                else if (ver.Minor == 3)
                {
                    if (ver.Build == 9200) return WindowsVersion.EightDotOne;
                    else if (ver.Build == 9600) return WindowsVersion.EightDotOneWithUpdate;
                }
            }
            else if (ver.Major == 10)
            {
                switch (ver.Build)
                {
                    case 10240: return WindowsVersion.Ten;
                    case 10586: return WindowsVersion.Ten1511;
                    case 14393: return WindowsVersion.Ten1607;
                    case 15063: return WindowsVersion.Ten1703;
                    default:
                        break;
                }
            }
            return WindowsVersion.Unknown;
        }
        /// <summary>
        /// 获取由本地IPv4地址构成的数组
        /// </summary>
        public static string[] LocalIPv4
        {
            get
            {
                //事先不知道ip的个数，数组长度未知，因此用StringCollection储存
                IPAddress[] localIPs;
                localIPs = Dns.GetHostAddresses(Dns.GetHostName());
                StringCollection IpCollection = new StringCollection();
                foreach (IPAddress ip in localIPs)
                {
                    //根据AddressFamily判断是否为ipv4,如果是InterNetWorkV6则为ipv6
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                        IpCollection.Add(ip.ToString());
                }
                string[] IpArray = new string[IpCollection.Count];
                IpCollection.CopyTo(IpArray, 0);
                return IpArray;
            }
        }        
        /// <summary>
        /// 以管理员身份运行程序
        /// </summary>
        /// <param name="fileName">要运行的程序</param>
        /// <param name="args">程序运行的参数</param>
        /// <returns>关联的进程</returns>
        public static Process RunAsAdministrator(string fileName, params string[] args)
        {
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = fileName
            };
            if (args != null)          
                psi.Arguments = string.Join(" ", args);
            psi.Verb = "runas";
            try
            {
                var p = Process.Start(psi);
                return p;
            }
            catch(System.ComponentModel.Win32Exception) //User Canceled
            {
                return null;
            }
        }
        /// <summary>
        /// 判断当前进程是否具有管理员权限
        /// </summary>
        public static bool IsAdministratorProcess
        {
            get
            { 
                WindowsIdentity current = WindowsIdentity.GetCurrent();
                WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
                return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
        /// <summary>
        /// 交换同类型的两项数据
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="left">待交换的数据</param>
        /// <param name="right">待交换的数据</param>
        public static void Swap<T>(T left, T right)
        {
            T temp = left;
            left = right;
            right = temp;
        }
        /// <summary>
        /// 获取指定名称的进程的命令行参数
        /// </summary>
        /// <param name="processName">进程名称</param>
        /// <returns>命令行参数</returns>
        public static List<string> GetCommandLineArgs(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            if (processes != null && processes.Length > 0)
            {
                var result = GetCommandLineArgs(processes[0]);
                return result;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取指定进程的命令行参数
        /// </summary>
        /// <param name="process">指定的进程</param>
        /// <returns>命令行参数</returns>
        public static List<string> GetCommandLineArgs(Process process)
        {
            string commandLine = "";
            
            using (var searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id))
            {
                foreach (var managementBaseObject in searcher.Get())
                {
                    commandLine = managementBaseObject["CommandLine"].ToString();
                }
            }
            List<string> parseCommandLine(string commandLineToParse)
            {
                List<string> result = new List<string>();
                bool isInsideQuotes = false;
                string arg = "";
                foreach (var character in commandLineToParse)
                {
                    if (character == ' ' && !isInsideQuotes)
                    {
                        if (arg == "") continue;
                        result.Add(arg);
                        arg = "";
                    }
                    else
                    {
                        if (character == '"')
                        {
                            isInsideQuotes = !isInsideQuotes;
                        }
                        arg += character;
                        
                    }
                    
                }
                return result;
            }
            return parseCommandLine(commandLine);
        }
        /// <summary>
        /// 将枚举转换为指定的类型
        /// </summary>
        /// <typeparam name="T">指定的类型</typeparam>
        /// <param name="en">要转换的枚举对象</param>
        /// <returns>转换后的值</returns>
        public static T To<T>(this Enum en) where T : struct
        {
            try
            {
                return (T)Enum.ToObject(en.GetType(), en as object);
            }
            catch
            {
                throw new InvalidCastException();
            }
        }
        /// <summary>
        /// 检查指定的程序文件是否已经在运行
        /// </summary>
        /// <param name="fileName">指定的程序文件</param>
        /// <param name="processName">检查时使用的进程名，默认与文件名相同</param>
        /// <returns>检查结果</returns>
        public static bool IsRunning(string fileName, string processName = null)
        {
            if (processName == null)
            {
                processName = fileName.GetFileName().RemoveExtension();
            }
            var processes = Process.GetProcessesByName(processName);
            var result = from process in processes
                         where string.Equals(process.MainModule.FileName,
                         fileName, StringComparison.OrdinalIgnoreCase)
                         select process;
            return result.Count() > 0;
        }
    }
}
