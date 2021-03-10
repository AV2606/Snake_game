using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    public class CString
    {
        public string Data { get; set; }
        public ConsoleColor Color { get; set; }

        public static void Write(IEnumerable<CString> cStrings)
        {
            foreach (var s in cStrings)
                s.Write();
        }
        public static void WriteLine(IEnumerable<CString> cStrings)
        {
            foreach (var s in cStrings)
                s.WriteLine();
        }

        public CString(string data, ConsoleColor color)
        {
            this.Data = data;
            this.Color = color;
        }
        public CString(char data, ConsoleColor color)
        {
            this.Data = data + "";
            this.Color = color;
        }
        public CString()
        {
            Data = "";
            this.Color = ConsoleColor.White;
        }

        public void Write()
        {
            if (Data.Length == 0)
                return;
            if (Console.ForegroundColor != Color)
                Console.ForegroundColor = Color;
            Console.Write(Data);
        }
        public void WriteLine()
        {
            if (Data.Length == 0)
                return;
            if (Console.ForegroundColor != Color)
                Console.ForegroundColor = Color;
            Console.WriteLine(Data);
        }

        public static CString operator +(CString left, string right)
        {
            left.Data += right;
            return left;
        }
        public static CString operator +(string left, CString right)
        {
            right.Data += left;
            return right;
        }
        public static CString operator +(CString left, char right)
        {
            left.Data += right;
            return left;
        }
        public static CString operator +(char left, CString right)
        {
            right.Data += left;
            return right;
        }
    }
}
