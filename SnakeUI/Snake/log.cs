using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Snake
{
    public class Log
    {
        public List<List<CString>> log { get; private set; } = new List<List<CString>>();
        public string Name { get; }
        public void AddLog(string raw_data)
        {
            log.Add(new List<CString>{ new CString(raw_data,ConsoleColor.White), new CString(", fromThread: " + Thread.CurrentThread.Name,ConsoleColor.Cyan)
            , new CString(", at: "+DateTime.Now.ToString("HH:MM:ss.mm"),ConsoleColor.Green)});
        }
        public void Write()
        {
            foreach (var l in log)
            { CString.Write(l); Console.WriteLine(); }
        }

        public Log(string Name)
        {
            log = new List<List<CString>>();
            this.Name = Name;
        }


    }
}
