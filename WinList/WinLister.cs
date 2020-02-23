using System;
using System.Linq;

namespace WinList
{
    public class WinLister
    {
        private WindowManager _wm = new WindowManager();
        
        private void ListWindows()
        {
            var windows = _wm.GetOpenWindows();
            ConsoleEx.WriteLine(
                "~--------------------------------- WINDOWS --------------------------------~"
                , ConsoleColor.Gray);
            foreach (var w in windows)
            {
                Console.Write("| ");
                ConsoleEx.Write($"{w.Key,-12}", ConsoleColor.Green);
                Console.Write(" | ");
                ConsoleEx.Write($"{string.Join("", w.Value.Take(57)),-57}", ConsoleColor.Yellow);
                Console.Write(" |");
                Console.Write("\n");
            }
            ConsoleEx.WriteLine(
                "~--------------------------------------------------------------------------~"
                , ConsoleColor.Gray);
        }

        private void PrintHelp()
        {
            Console.WriteLine(
@"====================================
  Okay. Commands: 
  --------------------------------
  X: eXit
  H: display this Help
  U: Unpin a window
  P: Pin a window
  R: Refresh the windows
====================================
"
            );
        }
        
        public void Run()
        {
            ListWindows();
            Console.WriteLine("Welcome to Rude Window Manager. Not.\n"
                              + "I'm only gonna say this once. You press H for help.\n" 
                              + "That's 'H' on your keyboard ye dimwit. ");
            
            
            bool run = true;
            while (run)
            {   
                Console.WriteLine("Whatcha want?");
                
                var c = Console.ReadKey(intercept: true);
                switch (c.Key)
                {
                    case ConsoleKey.X:
                        run = false;
                        break;
                    
                    case ConsoleKey.H:
                        PrintHelp();
                        break;
                    
                    case ConsoleKey.U:
                        Console.Write("Watcha want unpinned? (index) ");
                        try
                        {
                            var idx = int.Parse(Console.ReadLine());
                            _wm.SetWindowNotOnTop(idx);
                            Console.Clear();
                            ListWindows();
                            ConsoleEx.WriteLine("Done mate. Piss off.", ConsoleColor.Green);
                        }
                        catch
                        {
                            ConsoleEx.WriteLine("Bad input mate. Expecting an integer.", ConsoleColor.Red);
                        }
                        break;
                    
                    case ConsoleKey.P:
                        Console.Write("Watcha wanna pin? (index) ");
                        try
                        {
                            var idx = int.Parse(Console.ReadLine());
                            _wm.SetWindowOnTop(idx);
                            Console.Clear();
                            ListWindows();
                            ConsoleEx.WriteLine("Done mate. Piss off.", ConsoleColor.Green);
                        }
                        catch
                        {
                            ConsoleEx.WriteLine("Bad input mate. Expecting an integer.", ConsoleColor.Red);
                        }
                        break;
                    
                    case ConsoleKey.R:
                        Console.Clear();
                        ListWindows();
                        break;
                    
                    case ConsoleKey.Enter:
                        break;
                    
                    default:
                        ConsoleEx.WriteLine($"I DON'T SPEAK MORONESE!!! WHAT'S A `{c.KeyChar}`?", ConsoleColor.Red);
                        break;
                }
            }
        }
    }
}