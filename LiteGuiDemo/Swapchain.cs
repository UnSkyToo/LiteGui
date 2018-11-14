using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LiteGuiDemo
{
    internal class Swapchain
    {
        public Bitmap Current => Surface_[CurrentIndex_];
        public Bitmap Background => Surface_[1 - CurrentIndex_];

        private readonly Graphics Device_;
        private readonly Bitmap[] Surface_;
        private int CurrentIndex_;

        public Swapchain(IntPtr Handle, int Width, int Height)
        {
            Device_ = Graphics.FromHwnd(Handle);
            //Device_.SmoothingMode = SmoothingMode.AntiAlias;
            //Device_.PixelOffsetMode = PixelOffsetMode.HighQuality;
            //Device_.CompositingQuality = CompositingQuality.HighQuality;

            Surface_ = new Bitmap[2];
            Surface_[0] = new Bitmap(Width, Height);
            Surface_[1] = new Bitmap(Width, Height);
            CurrentIndex_ = 0;
        }

        public void SwapBuffers()
        {
            CurrentIndex_ = 1 - CurrentIndex_;
            Device_.DrawImage(Current, Point.Empty);
        }
    }
}