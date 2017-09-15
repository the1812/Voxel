using System.Diagnostics;
using System.Threading.Tasks;

namespace Ace
{
    /// <summary>
    /// 封装执行DOS指令后的输出信息
    /// </summary>
    public class CommandOutput
    {
        /// <summary>
        /// 获取指示是否发生错误的值
        /// </summary>
        public bool HasError { get; private set; }
        /// <summary>
        /// 获取指示是否有输出文本的值
        /// </summary>
        public bool HasOutput { get; private set; }
        /// <summary>
        /// 获取错误信息
        /// </summary>
        public string Error { get; private set; }
        /// <summary>
        /// 获取输出信息
        /// </summary>
        public string Output { get; private set; }
        /// <summary>
        /// 使用指定的信息初始化CommandOutput的新实例
        /// </summary>
        /// <param name="output">输出信息</param>
        /// <param name="error">错误信息</param>
        public CommandOutput(string output, string error)
        {
            Error = error;
            Output = output;
            HasError = !string.IsNullOrEmpty(error);
            HasOutput = !string.IsNullOrEmpty(output);
        }
    }
    /// <summary>
    /// 封装执行DOS命令的类
    /// </summary>
    public static class Command
    {
        #region 执行DOS命令需要的函数
        private static ProcessStartInfo getStartInfo(string command, string workDir)
        {
            ProcessStartInfo psi = new ProcessStartInfo("cmd", "/c " + command)
            {
                UseShellExecute = false,
                CreateNoWindow = true
            };
            if (!string.IsNullOrEmpty(workDir))
                psi.WorkingDirectory = workDir;
            return psi;
        }
        private static ProcessStartInfo getStartInfo(string command, string workDir, bool getOutput)
        {
            ProcessStartInfo psi = new ProcessStartInfo("cmd", "/c " + command)
            {
                UseShellExecute = false,
                CreateNoWindow = true
            };
            if (getOutput)
            {
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
            }
            if (!string.IsNullOrEmpty(workDir))
                psi.WorkingDirectory = workDir;
            return psi;
        }
        #endregion
        /// <summary>
        /// 在默认目录中异步执行DOS命令
        /// </summary>
        /// <param name="command">要执行的命令</param>
        public static Task RunAsync(string command)
        {
            return Task.Run(() =>
            {
                Process.Start(getStartInfo(command, null)).WaitForExit();
            });
        }
        /// <summary>
        /// 异步执行DOS命令
        /// </summary>
        /// <param name="command">要执行的命令</param>
        /// <param name="workDir">工作目录</param>
        public static Task RunAsync(string command, string workDir)
        {
            return Task.Run(() =>
            {
                Process.Start(getStartInfo(command, workDir)).WaitForExit();
            });
        }
        /// <summary>
        /// 在默认目录中同步执行DOS命令
        /// </summary>
        /// <param name="command"></param>
        public static void Run(string command)
        {
            Process p;
            p = Process.Start(getStartInfo(command, null, true));
            p.StandardOutput.ReadToEnd();
            p.StandardError.ReadToEnd();
        }
        /// <summary>
        /// 同步执行DOS命令
        /// </summary>
        /// <param name="command"></param>
        /// <param name="workDir"></param>
        public static void Run(string command,string workDir)
        {
            Process p;
            p = Process.Start(getStartInfo(command, workDir, true));
            p.StandardOutput.ReadToEnd();
            p.StandardError.ReadToEnd();
        }
        /// <summary>
        /// 在默认目录中异步执行DOS命令，并获取输出
        /// </summary>
        /// <param name="command">要执行的命令</param>
        public static Task<CommandOutput> GetOutputAsync(string command)
        {
            return Task.Run(()=>{
                Process p;
                p = Process.Start(getStartInfo(command, null, true));
                return new CommandOutput(p.StandardOutput.ReadToEnd(), p.StandardError.ReadToEnd());
            });            
        }
        /// <summary>
        /// 异步执行DOS命令，并获取输出
        /// </summary>
        /// <param name="command">要执行的命令</param>
        /// <param name="workDir">工作目录</param>
        public static Task<CommandOutput> GetOutputAsync(string command, string workDir)
        {
            return Task.Run(() => {
                Process p;
                p = Process.Start(getStartInfo(command, workDir, true));
                return new CommandOutput(p.StandardOutput.ReadToEnd(), p.StandardError.ReadToEnd());
            });
        }
        /// <summary>
        /// 在默认目录中同步执行DOS命令，并获取输出
        /// </summary>
        /// <param name="command">要执行的命令</param>
        public static CommandOutput GetOutput(string command)
        {
            Process p;
            p = Process.Start(getStartInfo(command, null, true));
            return new CommandOutput(p.StandardOutput.ReadToEnd(), p.StandardError.ReadToEnd());
        }
        /// <summary>
        /// 同步执行DOS命令，并获取输出
        /// </summary>
        /// <param name="command">要执行的命令</param>
        /// <param name="workDir">工作目录</param>
        public static CommandOutput GetOutput(string command, string workDir)
        {
            Process p;
            p = Process.Start(getStartInfo(command, workDir, true));
            return new CommandOutput(p.StandardOutput.ReadToEnd(), p.StandardError.ReadToEnd());
        }
    }
}
