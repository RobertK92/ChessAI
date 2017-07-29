using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SharpUtils
{
    public enum LogType : byte
    {
        Message,
        Warning,
        Error,
        Exception,
        Success
    }

    /// <summary>
    /// Utility for logging and handling errors, warnings, exceptions and messages.
    /// </summary>
    public static class Log
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        /// <summary>
        /// If no dumpFileName is given, a new guid will be used.
        /// </summary>
        public static string DumpFileName { get; set; }
        
        public static string DumpFileFullName { get; private set; }

        private static bool isInitialized;
        private static int stackFrame;
        private static Dictionary<byte, bool> debugChannels = new Dictionary<byte, bool>();
        private static RichTextBox logBox;
        private static Form logBoxForm;
        
        public static Action<string, LogType> OnLogged = delegate { };

        /// <summary>
        /// If the memory usage in MB is above this number, the ReportMemory function will be logged using an error.
        /// </summary>
        public static int MemoryErrorThreshold { get; set; }

        /// <summary>
        /// If the memory usage in MB is above this number, the ReportMemory function will be logged using a warning.
        /// </summary>
        public static int MemoryWarningThreshold { get; set; }

        /// <summary>
        /// Whether or not the logs will be displayed in the console.
        /// (default = true).
        /// </summary>
        public static bool UseConsole { get; set; }

        /// <summary>
        /// Whether or not the logs will be displayed in the log file.
        /// (default = true).
        /// </summary>
        public static bool UseDumpFile { get; set; }

        /// <summary>
        /// Whether or not executing should pause when an error occurs.
        /// </summary>
        public static bool PauseOnError { get; set; }

        /// <summary>
        /// Whether or not an error should also cause an assert.
        /// </summary>
        public static bool AssertOnError { get; set; }

        /// <summary>
        /// Whether or not caller info (Class, Method, Line numbers) should be logged for errors.
        /// (default = true).
        /// </summary>
        public static bool LogStackInfoForErrors { get; set; }

        /// <summary>
        /// Whether or not caller info (Class, Method, Line numbers) should be logged for warnings.
        /// (default = true).
        /// </summary>
        public static bool LogStackInfoForWarnings { get; set; }

        /// <summary>
        /// Whether or not caller info (Class, Method, Line numbers) should be logged for messages.
        /// (default = true).
        /// </summary>
        public static bool LogStackInfoForMessages { get; set; }

        /// <summary>
        /// The minimum amount of characters printed in console before logging the stack info.
        /// If the log has fewer characters that this number, spaces will be added.
        /// </summary>
        public static int CharacterCountBeforeStackInfoConsole { get; set; }

        /// <summary>
        /// The minimum amount of characters logged to file before logging the stack info.
        /// If the log has fewer characters that this number, spaces will be added.
        /// </summary>
        public static int CharacterCountBeforeStackInfoFile { get; set; }

        
        internal static void Init()
        {
            if (isInitialized)
                return;

            if(string.IsNullOrEmpty(DumpFileName))
            {
                DumpFileName = Guid.NewGuid().ToString() + ".txt";
            }

            DumpFileFullName = "Logs\\" + DumpFileName;

            UseConsole = (GetConsoleWindow() != IntPtr.Zero);
            UseDumpFile = true;

            AssertOnError = false;
            LogStackInfoForErrors = true;
            LogStackInfoForWarnings = true;
            LogStackInfoForMessages = true;
            CharacterCountBeforeStackInfoConsole = 48;
            CharacterCountBeforeStackInfoFile = 100;
            MemoryErrorThreshold = 5000;
            MemoryWarningThreshold = 500;
            stackFrame = 3;

            Directory.CreateDirectory("Logs");
            File.Create(DumpFileFullName).Close();
            isInitialized = true;
            Message("Log.Init complete");
        }

        private static void LogAny(string msg, LogType type)
        {
            if (!isInitialized)
            {
                Init();
            }

            string stackInfo = LogStackInfoForMessages ? GetStackInfo() : string.Empty;

            if (UseDumpFile)
            {
                LogToDumpFile(msg.PadRight(CharacterCountBeforeStackInfoFile) + stackInfo, type);
            }
            if (UseConsole)
            {
                LogToConsole(msg.PadRight(CharacterCountBeforeStackInfoConsole) + stackInfo, type);
            }

            OnLogged.Invoke(msg, type);
        }

        private static void LogToDumpFile(string msg, LogType type)
        {
            using (StreamWriter writer = new StreamWriter(DumpFileFullName, true))
            {
                writer.WriteLine(msg);
            }
        }

        private static void LogToConsole(string msg, LogType type)
        {
            switch (type)
            {
                case LogType.Message:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogType.Exception:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogType.Success:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                default:
                    break;
            }

            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }


        public static void BindLogToRichTextBox(RichTextBox richTextBox) 
        {
            if (richTextBox == null)
                return;

            logBox = richTextBox;
            logBoxForm = richTextBox.FindForm();
            OnLogged += OnLogRedirect;
        }

        private static void OnLogRedirect(string aMsg, LogType aLogType)
        {
            if (logBoxForm == null)
                return;

            if (logBoxForm.InvokeRequired)
            {
                try
                {
                    logBoxForm.Invoke(new Action<string, LogType>(OnLogged), aMsg, aLogType);
                }
                catch (ObjectDisposedException ex) { }
                return;
            }

            RedirectLog(aMsg, aLogType);
        }

        private static void RedirectLog(string aMsg, LogType aLogType)
        {
            if (logBox == null)
                return;

            if (logBox.Disposing || logBox.IsDisposed)
                return;

            logBox.SelectionStart = logBox.TextLength;
            logBox.SelectionLength = aMsg.Length;
            Color errorColor = Color.FromArgb(255, 10, 20);

            switch (aLogType)
            {
                case LogType.Message:
                    logBox.SelectionColor = Color.White;
                    break;
                case LogType.Warning:
                    logBox.SelectionColor = Color.Yellow;
                    break;
                case LogType.Error:
                    logBox.SelectionColor = errorColor;
                    break;
                case LogType.Exception:
                    logBox.SelectionColor = errorColor;
                    break;
                case LogType.Success:
                    logBox.SelectionColor = Color.SpringGreen;
                    break;
                default:
                    break;
            }

            logBox.AppendText(aMsg + "\n");
            logBox.SelectionColor = logBox.ForeColor;
            logBox.ScrollToCaret();
        }

        /// <summary>
        /// Dumps the current memory usage.
        /// </summary>
        public static void ReportMemoryUsage()
        {
            long mb = -1;
            try
            {
                //mb = Process.GetCurrentProcess().WorkingSet64 / 1000000;
                mb = GC.GetTotalMemory(true) / 1000000;
            }
            catch (OutOfMemoryException e)
            {
                Exception(e);
            }

            string msg = string.Format("Memory usage: {0} MB", mb);
            if (mb > MemoryErrorThreshold)
            {
                Error(msg);
                return;
            }
            if (mb > MemoryWarningThreshold)
            {
                Warning(msg);
                return;
            }
            Success(msg);
        }

        /// <summary>
        /// Enables or disabled a specific log channel.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="enabled"></param>
        public static void SetDebugChannel(byte debugChannel, bool enabled)
        {
            if (!debugChannels.ContainsKey(debugChannel))
            {
                debugChannels.Add(debugChannel, enabled);
                return;
            }
            debugChannels[debugChannel] = enabled;
        }

        /// <summary>
        /// Dumps an empty line.
        /// </summary>
        public static void NewLine()
        {
            LogAny("--> ", LogType.Message);
        }

        /// <summary>
        /// Dumps the last win32 error (success message if error code = 0x0)
        /// </summary>
        public static void LastWin32Error(string prefix = "")
        {
            stackFrame += 1;
            int errorCode = Marshal.GetLastWin32Error();
            string errorMessage = string.Format("{0}: {1}", prefix, new Win32Exception(errorCode).Message);
            if (errorCode == 0)
                Success(errorMessage, "LastWin32Error");
            else
                Error(errorMessage, "LastWin32Error");
            stackFrame -= 1;
        }

        /// <summary>
        /// Dumps a debug info message using a certain debug channel.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="debugChannel"></param>
        public static void DebugMessage(object message, byte debugChannel = 0)
        {
            if (!debugChannels.ContainsKey(debugChannel))
            {
                debugChannels.Add(debugChannel, true);
            }
            
            if (debugChannels[debugChannel])
            {
                LogAny(string.Format("--> {0}", message.ToString()), LogType.Message);
            }
        }

        /// <summary>
        /// Dumps an info message.
        /// </summary>
        /// <param name="message"></param>
        public static void Message(object message)
        {
            LogAny(string.Format("--> {0}", message.ToString()), LogType.Message);
        }

        /// <summary>
        /// Dumps a success message.
        /// </summary>
        /// <param name="message"></param>
        public static void Success(object message, string successPrefix = null)
        {
            if(successPrefix == null)
                LogAny(string.Format("--> {0}", message.ToString()), LogType.Success);
            else
                LogAny(string.Format("--> [{0}] {1}", successPrefix, message.ToString()), LogType.Success);
        }

        /// <summary>
        /// Dumps a warning message.
        /// </summary>
        /// <param name="warningMessage"></param>
        public static void Warning(object warningMessage)
        {
            LogAny(string.Format("--> [Warning]: {0}", warningMessage.ToString()), LogType.Warning);
        }

        /// <summary>
        /// Dumps an error message.
        /// </summary>
        /// <param name="errorMessage"></param>
        public static void Error(object errorMessage, string errorPrefix = "Error")
        {
            LogAny(string.Format("--> [{0}]: {1}", errorPrefix, errorMessage.ToString()), LogType.Error);
            Trace.Assert(!AssertOnError, string.Format("[{0}]", errorPrefix) + Environment.NewLine, errorMessage.ToString() + Environment.NewLine);

            if (PauseOnError)
            {
                // TODO: pause execution.
            }
        }

        /// <summary>
        /// Dumps an exception message.
        /// </summary>
        /// <param name="e"></param>
        public static void Exception(Exception e)
        {
            LogAny(string.Format("--> [Exception]: {0}", e.Message), LogType.Exception);
        }

        private static string GetStackInfo()
        {
            StackTrace stack = new StackTrace(true);
            StackFrame lastFrame = stack.GetFrame(stackFrame);
            MethodBase method = lastFrame.GetMethod();
            string className = method.DeclaringType.ToString();
            string methodName = method.Name;
            string line = lastFrame.GetFileLineNumber().ToString();

            if (line == "0")
                line = "?";

            return string.Format("({0}.{1}, line:{2})", className, methodName, line.ToString());
        }
    }
}
