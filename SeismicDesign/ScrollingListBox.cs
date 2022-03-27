using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Scroll
{
    public partial class ScrollingListBox : ListView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        [Category("Action")]
        public event ScrollEventHandler Scrolled = null;
        //public event ScrollEventHandler Wheeled = null;
        private const int WM_VSCROLL = 0x115;
        private const int SB_ENDSCROLL = 0x8;
        private const int SIF_RANGE = 0x1;
        private const int SIF_PAGE = 0x2;
        private const int SIF_POS = 0x4;
        private const int SIF_TRACKPOS = 0x10;
        private const int SIF_ALL = SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS;
        private struct ScrollInfoStruct
        {
            public int cbSize;
            public int fMask;
            public int nMin;
            public int nMax;
            public int nPage;
            public int nPos;
            public int nTrackPos;
        }
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetScrollInfo(IntPtr hWnd, int n, ref ScrollInfoStruct lpScrollInfo);
        protected override void WndProc(ref System.Windows.Forms.Message msg)
        {
            if (msg.Msg == WM_VSCROLL)
            {
                if (Scrolled != null)
                {
                    ScrollInfoStruct si = new ScrollInfoStruct();
                    si.fMask = SIF_ALL;
                    si.cbSize = Marshal.SizeOf(si);
                    GetScrollInfo(msg.HWnd, 0, ref si);
                    if (msg.WParam.ToInt32() == SB_ENDSCROLL)
                    {
                        ScrollEventArgs sargs = new ScrollEventArgs(ScrollEventType.EndScroll, si.nPos);
                        Scrolled(this, sargs);
                    }
                }
            }
            base.WndProc(ref msg);
        }
        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }
    }
}