// Copyright © 2017 Paddy Xu
// 
// This file is part of QuickLook program.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using QuickLook.Common.NativeMethods;

namespace QuickLook.Common.Helpers
{
    public static class DpiHelper
    {
        public const int DefaultDpi = 96;

        public static ScaleFactor GetScaleFactorFromWindow(Window window)
        {
            return GetScaleFactorFromWindow(new WindowInteropHelper(window).EnsureHandle());
        }

        public static ScaleFactor GetCurrentScaleFactor()
        {
            return GetScaleFactorFromWindow(User32.GetForegroundWindow());
        }

        public static ScaleFactor GetScaleFactorFromWindow(IntPtr hwnd)
        {
            var dpiX = DefaultDpi;
            var dpiY = DefaultDpi;

            try
            {
                if (Environment.OSVersion.Version > new Version(6, 2)) // Windows 8.1 = 6.3.9200
                {
                    var hMonitor = MonitorFromWindow(hwnd, MonitorDefaults.TOPRIMARY);
                    GetDpiForMonitor(hMonitor, MonitorDpiType.EFFECTIVE_DPI, out dpiX, out dpiY);
                }
                else
                {
                    var g = Graphics.FromHwnd(IntPtr.Zero);
                    var desktop = g.GetHdc();

                    dpiX = GetDeviceCaps(desktop, DeviceCap.LOGPIXELSX);
                    dpiY = GetDeviceCaps(desktop, DeviceCap.LOGPIXELSY);
                }
            }
            catch (Exception e)
            {
                ProcessHelper.WriteLog(e.ToString());
            }

            return new ScaleFactor {Horizontal = (float) dpiX / DefaultDpi, Vertical = (float) dpiY / DefaultDpi};
        }

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern int GetDeviceCaps(IntPtr hDC, DeviceCap nIndex);

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr hWnd, MonitorDefaults dwFlags);

        [DllImport("shcore.dll")]
        private static extern uint
            GetDpiForMonitor(IntPtr hMonitor, MonitorDpiType dpiType, out int dpiX, out int dpiY);

        private enum MonitorDpiType
        {
            EFFECTIVE_DPI = 0,
            ANGULAR_DPI = 1,
            RAW_DPI = 2
        }

        private enum MonitorDefaults
        {
            TONULL = 0,
            TOPRIMARY = 1,
            TONEAREST = 2
        }

        private enum DeviceCap
        {
            /// <summary>
            ///     Logical pixels inch in X
            /// </summary>
            LOGPIXELSX = 88,
            /// <summary>
            ///     Logical pixels inch in Y
            /// </summary>
            LOGPIXELSY = 90
        }

        public struct ScaleFactor
        {
            public float Horizontal;
            public float Vertical;
        }
    }
}