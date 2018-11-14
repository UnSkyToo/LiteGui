using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace LiteGuiDemo
{
    internal class AppForm : Form
    {
        private readonly Graphics[] Devices_;
        private readonly Bitmap[] Buffers_;
        private readonly PictureBox Surface_;
        private int CurrentIndex_;

        internal PictureBox Surface => Surface_;

        internal AppForm()
            : base()
        {
            /*base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.UpdateStyles();*/

            base.Text = "LiteGui Demo - 0.0.0.1";
            base.KeyPreview = true;
            base.MaximizeBox = false;
            base.ClientSize = new Size(960, 540);
            base.StartPosition = FormStartPosition.CenterScreen;
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            
            this.Surface_ = new PictureBox
            {
                Location = Point.Empty,
                ClientSize = ClientSize,
            };
            base.Controls.Add(this.Surface_);
            
            this.Buffers_ = new Bitmap[2];
            this.Buffers_[0] = new Bitmap(ClientSize.Width, ClientSize.Height);
            this.Buffers_[1] = new Bitmap(ClientSize.Width, ClientSize.Height);

            this.Devices_ = new Graphics[2];
            this.Devices_[0] = Graphics.FromImage(this.Buffers_[0]);
            this.Devices_[0].TextRenderingHint = TextRenderingHint.AntiAlias;
            this.Devices_[0].SmoothingMode = SmoothingMode.AntiAlias;
            this.Devices_[0].PixelOffsetMode = PixelOffsetMode.HighQuality;
            //this.Devices_[0].CompositingQuality = CompositingQuality.HighQuality;
            this.Devices_[1] = Graphics.FromImage(this.Buffers_[1]);
            this.Devices_[1].TextRenderingHint = TextRenderingHint.AntiAlias;
            this.Devices_[1].SmoothingMode = SmoothingMode.AntiAlias;
            this.Devices_[1].PixelOffsetMode = PixelOffsetMode.HighQuality;
            //this.Devices_[1].CompositingQuality = CompositingQuality.HighQuality;

            this.CurrentIndex_ = 0;
        }

        internal Graphics GetCurrentDevice()
        {
            return this.Devices_[this.CurrentIndex_];
        }
        
        internal void SwapBuffers()
        {
            this.Surface_.Image = Buffers_[this.CurrentIndex_];
            this.CurrentIndex_ = 1 - this.CurrentIndex_;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // AppForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "AppForm";
            this.ResumeLayout(false);

        }
    }
}