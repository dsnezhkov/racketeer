using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace FileConnectorCommon
{
    public class Logger
    {
        private Target target;
        private Level currentLevel;
        MemoryStream ms;

        public enum Target
        {
            Console,
            Memory,
        }

        public enum Level
        {
            Debug,
            Info,
            Warn,
            Error,
            Fatal
        }
        public Logger(Target target)
        {
            this.target = target;
            ms = new MemoryStream();
            currentLevel = Level.Info;
        }

        public Target GetLogTarget()
        {
            return target;
        }
        public void SetLogTarget(Logger.Target t)
        {
            target = t;
        }
        public void WriteToConsole(Level level, DateTime dateTime, String message)
        {
            var dateTime2 = dateTime.ToString("yyyy.MM.dd HH:mm:ss.fff");
            var line = string.Format("{0}\t{1}\t{2}", dateTime2, level, message);

            switch (level)
            {
                case Level.Debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case Level.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case Level.Warn:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case Level.Error:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case Level.Fatal:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    break;
            }

            Console.WriteLine(line);
        }

        public void WriteToFile()
        {
            /*            var date = DateTime.Now.ToString("yyyyMMdd");
                        var logFileName = string.Format("{0}.log", date);
                        using (var streamWriter = new StreamWriter(logFileName, true))
                        {
                            var time = e.ToString("HH:mm:ss.fff");
                            var line = string.Format("{0}\t{1}\t{2}\t{3}", time, level, threadId, message);
                            streamWriter.WriteLine(line);
                        }*/
        }
        public void WriteToMemory(Level level, DateTime dateTime, String message)
        {
            var dateTime2 = dateTime.ToString("yyyy.MM.dd HH:mm:ss.fff");
            var line = string.Format("{0}\t{1}\t{2}", dateTime2, level, message);

            StreamWriter sw = new StreamWriter(ms, Encoding.UTF8);
            sw.AutoFlush = true;
            try
            {
                sw.WriteLine(line);

            } catch (Exception e)
            {
                Console.WriteLine("Ex: {0} {1}", e.Message, e.InnerException);
            }
        }

        public void SetLevel(Level level)
        {
            currentLevel = level;
        }

        public void ClearMemoryLogs()
        {
            try
            {
                ms.Seek(0, SeekOrigin.Begin);
                ms.SetLength(0); 
            }catch (Exception)
            {
                // eat it. May not recover from memory ops and crash agent
                ;
            }
        }
        public string GetLogsFromMemory()
        {
            string logsContent = string.Empty;
            try
            {
                ms.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms, Encoding.UTF8);
                logsContent = sr.ReadToEnd();
                
            } catch (Exception e)
            {
                Console.WriteLine("Ex: {0} {1}", e.Message, e.InnerException);
            }
            return logsContent;
        }
        public void Log(Level level, string message)
        {
            // Console.WriteLine($"Loggging : Asked level {level}, current level:{currentLevel}");
            if (level >= currentLevel)
            {
                var timeStamp = DateTime.Now;
                if (target == Target.Console)
                    WriteToConsole(level, timeStamp, message);
                if (target == Target.Memory)
                    WriteToMemory(level, timeStamp, message);
            }
        }
        public void Debug(string message) { Log(Level.Debug, message); }
        public void Info( string message) { Log(Level.Info, message); }
        public void Warn(string message) { Log(Level.Warn, message); }
        public void Error(string message) { Log(Level.Error,  message); }
        public void Fatal( string message) { Log(Level.Fatal, message); }

    }
}
