using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SendMessageToWindow
{
    public class HandleAccesser
    {
        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        
        [DllImport("user32", CharSet = CharSet.Auto)]
        static extern int GetWindowText(IntPtr handle, StringBuilder lpString, int cch);

        [DllImport("user32")]
        private static extern bool PostMessage(IntPtr handle, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32")]
        private static extern bool EnumChildWindows(IntPtr handle, EnumWindowsProc callback, IntPtr param);

        [DllImport("user32")]
        private static extern IntPtr WindowFromPoint(POINT Point);

        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetClassName(IntPtr handle, StringBuilder lpClassName, int nMaxCount);

        private delegate bool EnumWindowsProc(IntPtr handle, IntPtr param);
        public delegate bool EnumWindowsDelegate(IntPtr handle, IntPtr lparam);


        public static IntPtr SearchHandleName(IntPtr handle, string str)
        {
            var list = new List<HandleAndName>();

            EnumChildWindows(handle, (h, l) =>
                {
                    StringBuilder csb = new StringBuilder(256);
                    GetClassName(h, csb, csb.Capacity);
                    list.Add(new HandleAndName(h, csb.ToString()));
                    return true;
                },
            IntPtr.Zero);

            return list.Where(i => i.Name.Contains(str)).FirstOrDefault().Handle;
        }
        
        private static bool EnumWindowCallBack(IntPtr handle, IntPtr lparam)
        {
            StringBuilder csb = new StringBuilder(256);
            GetClassName(handle, csb, csb.Capacity);
            
            return true;
        }

        /// <summary>
        /// マウスクリックメッセージ送付
        /// </summary>
        /// <param name="handle">メッセージ送付先窓ハンドル</param>
        /// <param name="x">クリック座標x ハンドル内座標</param>
        /// <param name="y">クリック座標y ハンドル内座標</param>
        public static void PostMouseClick(IntPtr handle, int x, int y)
        {
            UInt32 pos = ((UInt32)y << 16) | (UInt32)x;

            PostMessage(handle, (uint)MessageType.WM_LBUTTONDOWN, new IntPtr((int)MessageParamAtMouseButton.LeftButton), new IntPtr(pos));
            Thread.Sleep(10);
            PostMessage(handle, (uint)MessageType.WM_LBUTTONUP, new IntPtr((int)MessageParamAtMouseButton.None), new IntPtr(pos));
        }

        /// <summary>
        /// マウスクリックメッセージ送付
        /// </summary>
        /// <param name="handle">メッセージ送付先窓ハンドル</param>
        /// <param name="mousePoint">クリック座標 ハンドル内座標</param>
        public static void PostMouseClick(IntPtr handle, Point mousePoint)
        {
            PostMouseClick(handle, mousePoint.X, mousePoint.Y);
        }
    }
}
