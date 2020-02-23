using System;

namespace WinList
{
    public static class ConsoleEx
    {
        public static void WriteLine(object o, ConsoleColor foregroundColor)
        {
            var oldForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = foregroundColor;
            
            Console.WriteLine(o);
            
            Console.ForegroundColor = oldForegroundColor;
        }
        
        public static void Write(object o, ConsoleColor foregroundColor)
        {
            var oldForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = foregroundColor;
            
            Console.Write(o);
            
            Console.ForegroundColor = oldForegroundColor;
        }
    }
}