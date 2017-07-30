using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SendMessageToWindow
{
    internal class HandleAndName
    {
        public HandleAndName(IntPtr handle, string name)
        {
            Handle = handle;
            Name = name;
        }
        public IntPtr Handle;
        public string Name;
    }

    internal enum MessageType : uint
    {
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
    }

    [Flags]
    internal enum MessageParamAtMouseButton
    {
        None = 0,
        LeftButton = 0x0001,
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
