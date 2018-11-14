using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using LiteGui;
using LiteGuiDemo.Demos;

namespace LiteGuiDemo
{
    internal class App
    {
        private double DeltaTime_;
        private readonly AppForm Form_;
        public readonly Font TextFont_;
        private readonly Swapchain Swapchain_;
        private readonly DemoManager DemoMgr_;

        public App()
        {
            Form_ = new AppForm();
            /*{
                Text = "LiteGui Demo - 0.0.0.1",
                KeyPreview = true,
                MaximizeBox = false,
                ClientSize = new Size(960, 540),   
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedSingle,
            };*/
            
            Form_.KeyDown += (Sender, Args) => { Input.OnKeyDown(Args.KeyCode); };
            Form_.KeyUp += (Sender, Args) => { Input.OnKeyUp(Args.KeyCode); };
            Form_.Surface.MouseDown += (Sender, Args) => { Input.OnMouseDown(Args.Button, Args.X, Args.Y); };
            Form_.Surface.MouseUp += (Sender, Args) => { Input.OnMouseUp(Args.Button, Args.X, Args.Y); };
            Form_.Surface.MouseMove += (Sender, Args) => { Input.OnMouseMove(Args.Button, Args.X, Args.Y); };
            Form_.Surface.MouseWheel += (Sender, Args) => { Input.OnMouseWheel(Args.Delta); };

            TextFont_ = new Font("Arial", 10);
            Swapchain_ = new Swapchain(Form_.Handle, Form_.ClientSize.Width, Form_.ClientSize.Height);

            DemoMgr_ = new DemoManager(new LGuiVec2(960, 540));
        }
        
        public void Run()
        {
            Form_.Show();

            var PreviousFrameTicks = 0L;
            var Watch = new Stopwatch();
            Watch.Start();

            DemoMgr_.Startup();

            while (!Form_.IsDisposed)
            {
                var CurrentFrameTicks = Watch.ElapsedTicks;
                DeltaTime_ = (CurrentFrameTicks - PreviousFrameTicks) / (double)Stopwatch.Frequency;
                PreviousFrameTicks = CurrentFrameTicks;

                MainLoop();
                Application.DoEvents();
            }

            DemoMgr_.Shutdown();
        }

        private void MainLoop()
        {
            Tick((float)DeltaTime_);
            Render();

            Input.Update();
        }

        private void Tick(float Seconds)
        {
            DemoMgr_.Tick(Seconds);

            if (Input.IsKeyPressed(Keys.Escape))
            {
                Form_.Close();
            }
        }

        private void Render()
        {
            var G = Form_.GetCurrentDevice();
            //using (var G = Graphics.FromImage(Swapchain_.Background))
            {
                /*G.SmoothingMode = SmoothingMode.AntiAlias;
                G.TextRenderingHint = TextRenderingHint.AntiAlias;
                G.CompositingQuality = CompositingQuality.HighQuality;*/

                G.Clear(Color.Black);
                DemoMgr_.Render(G);
                G.DrawString($"Fps : {GetFps(DeltaTime_)}", TextFont_, Brushes.White, 900, 520);
            }

            //Swapchain_.SwapBuffers();
            Form_.SwapBuffers();
        }

        private double FpsDuration_ = 1.0d / 60.0d;
        private double FpsAlpha = 1.0d / 60.0d;
        private int FpsFrameCount_ = 0;

        private int GetFps(double Ms)
        {
            FpsFrameCount_++;
            FpsDuration_ = FpsDuration_ * (1 - FpsAlpha) + Ms * FpsAlpha;
            return (int)(1.0d / FpsDuration_);
        }
    }
}