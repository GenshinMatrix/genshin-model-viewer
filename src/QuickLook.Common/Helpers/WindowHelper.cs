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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using QuickLook.Common.NativeMethods;

namespace QuickLook.Common.Helpers
{
    public static class WindowHelper
    {
        public enum WindowCompositionAttribute
        {
            WcaAccentPolicy = 19
        }

        public static Size GetCurrentDesktopSize()
        {
            var scale = DpiHelper.GetCurrentScaleFactor();
            var rect = GetCurrentDesktopRectInPixel();

            return new Size(rect.Width / scale.Horizontal, rect.Height / scale.Vertical);
        }

        public static Rect GetCurrentDesktopRectInPixel()
        {
            return GetDesktopRectFromWindowInPixel(User32.GetForegroundWindow());
        }

        public static Rect GetDesktopRectFromWindowInPixel(Window window)
        {
            return GetDesktopRectFromWindowInPixel(new WindowInteropHelper(window).Handle);
        }

        public static Rect GetDesktopRectFromWindowInPixel(IntPtr hwnd)
        {
            var screen = Screen.FromHandle(hwnd).WorkingArea;

            var area = new Rect(new Point(screen.X, screen.Y),
                new Size(screen.Width, screen.Height));

            return area;
        }

        public static void BringToFront(this Window window, bool keep)
        {
            var handle = new WindowInteropHelper(window).Handle;
            keep |= window.Topmost;

            User32.SetWindowPos(handle, User32.HWND_TOPMOST, 0, 0, 0, 0,
                User32.SWP_NOMOVE | User32.SWP_NOSIZE | User32.SWP_NOACTIVATE);

            if (!keep)
                User32.SetWindowPos(handle, User32.HWND_NOTOPMOST, 0, 0, 0, 0,
                    User32.SWP_NOMOVE | User32.SWP_NOSIZE | User32.SWP_NOACTIVATE);
        }

        public static void MoveWindow(this Window window,
            double pxLeft,
            double pxTop,
            double width,
            double height)
        {
            var handle = new WindowInteropHelper(window).EnsureHandle();

            // scale the size to the primary display
            TransformToPixels(window, width, height,
                out var pxWidth, out var pxHeight);

            // Use absolute location and relative size. WPF will scale the size to the target display
            User32.MoveWindow(handle, (int) Math.Round(pxLeft), (int) Math.Round(pxTop), pxWidth, pxHeight, true);
        }

        public static Rect GetWindowRectInPixel(this Window window)
        {
            var handle = new WindowInteropHelper(window).EnsureHandle();

            User32.GetWindowRect(handle, out User32.RECT nRect);

            return new Rect(new Point(nRect.Left, nRect.Top), new Point(nRect.Right, nRect.Bottom));
        }

        private static void TransformToPixels(this Visual visual,
            double unitX,
            double unitY,
            out int pixelX,
            out int pixelY)
        {
            Matrix matrix;
            var source = PresentationSource.FromVisual(visual);
            if (source != null)
                matrix = source.CompositionTarget.TransformToDevice;
            else
                using (var src = new HwndSource(new HwndSourceParameters()))
                {
                    matrix = src.CompositionTarget.TransformToDevice;
                }

            pixelX = (int) Math.Round(matrix.M11 * unitX);
            pixelY = (int) Math.Round(matrix.M22 * unitY);
        }

        public static bool IsForegroundWindowBelongToSelf()
        {
            var hwnd = User32.GetForegroundWindow();
            if (hwnd == IntPtr.Zero)
                return false;

            User32.GetWindowThreadProcessId(hwnd, out var procId);
            return procId == Process.GetCurrentProcess().Id;
        }

        public static void SetNoactivate(this Window window)
        {
            var hwnd = new WindowInteropHelper(window);
            User32.SetWindowLong(hwnd.Handle, User32.GWL_EXSTYLE,
                User32.GetWindowLong(hwnd.Handle, User32.GWL_EXSTYLE) |
                User32.WS_EX_NOACTIVATE);
        }

        public static void EnableBlur(Window window)
        {
            var accent = new AccentPolicy();
            var accentStructSize = Marshal.SizeOf(accent);
            accent.AccentState = AccentState.AccentEnableBlurbehind;
            accent.AccentFlags = 2;
            accent.GradientColor = 0x99FFFFFF;

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData
            {
                Attribute = WindowCompositionAttribute.WcaAccentPolicy,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };

            User32.SetWindowCompositionAttribute(new WindowInteropHelper(window).Handle, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        private enum AccentState
        {
            AccentDisabled = 0,
            AccentEnableGradient = 1,
            AccentEnableTransparentgradient = 2,
            AccentEnableBlurbehind = 3,
            AccentInvalidState = 4
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct AccentPolicy
        {
            public AccentState AccentState;
            public int AccentFlags;
            public uint GradientColor;
            public readonly int AnimationId;
        }
    }
}