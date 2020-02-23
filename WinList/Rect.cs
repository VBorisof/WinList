using System.Runtime.InteropServices;

namespace WinList
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect 
    {
        public int Left, Top, Right, Bottom;
 
        public Rect(int left, int top, int right, int bottom) 
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
 
        public int X {
            get {
                return Left;
            }
            set {
                Right -= (Left - value);
                Left = value;
            }
        }
 
        public int Y {
            get {
                return Top;
            }
            set {
                Bottom -= (Top - value);
                Top = value;
            }
        }
 
        public int Height {
            get {
                return Bottom - Top;
            }
            set {
                Bottom = value + Top;
            }
        }
 
        public int Width {
            get {
                return Right - Left;
            }
            set {
                Right = value + Left;
            }
        }
    }
}