using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using HWND = System.IntPtr;

namespace WinList
{
    public class WindowInfo
    {
        public HWND Handle { get; set; }
        public string Description { get; set; }
    }
    
    public class WindowManager
    {
        public static Dictionary<int, WindowInfo> Dict { get; set; } = new Dictionary<int, WindowInfo>();

        public static readonly HWND HWND_TOPMOST = new HWND(-1);
        public static readonly HWND HWND_NOT_TOPMOST = new HWND(-2);
        const UInt32 SWP_SHOWWINDOW = 0x0040;

        public void SetWindowOnTop(int hWndIdx)
        {
            SetWindowOnTop(Dict[hWndIdx].Handle);
        }
        
        public void SetWindowNotOnTop(int hWndIdx)
        {
            SetWindowNotOnTop(Dict[hWndIdx].Handle);
        }
        
        public void SetWindowOnTop(HWND hWnd)
        {
            GetWindowRect(new HandleRef(this, hWnd), out var rect);
            SetWindowPos(hWnd, HWND_TOPMOST, rect.X, rect.Y, rect.Width, rect.Height, SWP_SHOWWINDOW);
        }
        
        public void SetWindowNotOnTop(HWND hWnd)
        {
            GetWindowRect(new HandleRef(this, hWnd), out var rect);
            SetWindowPos(hWnd, HWND_NOT_TOPMOST, rect.X, rect.Y, rect.Width, rect.Height, SWP_SHOWWINDOW);
        }
        
        /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
        /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
        public IDictionary<int, string> GetOpenWindows()
        {
            HWND shellWindow = GetShellWindow();
            Dictionary<HWND, string> windows = new Dictionary<HWND, string>();

            EnumWindows(
                delegate(HWND hWnd, int lParam)
                {
                    if (hWnd == shellWindow)
                    {
                        return true;
                    }
                    if (!IsWindowVisible(hWnd))
                    {
                        return true;
                    }

                    int length = GetWindowTextLength(hWnd);
                    if (length == 0)
                    {
                        return true;
                    }

                    var builder = new StringBuilder(length);
                    GetWindowText(hWnd, builder, length + 1);

                    windows[hWnd] = builder.ToString();
                    return true;
                }, 
                0
            );

            Dict.Clear();
            for (int i = 0; i < windows.Count; ++i)
            {
                Dict[i] = new WindowInfo
                {
                    Handle = windows.ElementAt(i).Key,
                    Description = windows.ElementAt(i).Value
                };
            }

            return Dict.Select(x => new
                {
                    Key = x.Key,
                    Value = x.Value.Description
                })
                .ToDictionary(x => x.Key, x => x.Value);
        }

        private delegate bool EnumWindowsProc(HWND hWnd, int lParam);

        [DllImport("USER32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowText(HWND hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowTextLength(HWND hWnd);

        [DllImport("USER32.DLL")]
        private static extern bool IsWindowVisible(HWND hWnd);

        [DllImport("USER32.DLL")]
        private static extern HWND GetShellWindow();
        
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(HandleRef hWnd, out Rect lpRect);
 
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);
    }
}