using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LiteGui;

namespace LiteGuiDemo.Demos
{
    internal class DemoManager
    {
        internal LGuiVec2 WinSize { get; private set; }

        private readonly GdiCommandExecutor Executor_;
        private readonly Font TextFont_;
        private readonly List<DemoBase> DemoList_ = new List<DemoBase>();
        private int DemoIndex_ = -1;

        internal DemoManager(LGuiVec2 WinSize)
        {
            this.WinSize = WinSize;

            TextFont_ = new Font("Arial", 10);

            WinConvert.Register();
            Executor_ = new GdiCommandExecutor();
            LGui.SetCommandExecutor(Executor_);

            var IO = LGui.GetIO();
            IO.DisplaySize = new LGuiVec2(960, 540);

            FileHelper.AddSearchPath($"{AppContext.BaseDirectory}..\\..\\Res\\");
            
            var Mapper = new LGuiMapper();
            Input.OnKeyEvent += (Key, IsKeyDown) => { IO.SetKeyState(Mapper.ToLGuiKey(Key), IsKeyDown); };
            Input.OnMouseEvent += (Btn, X, Y, IsMouseDown, IsMouseMove) => { IO.SetMouseState(Mapper.ToLGuiMouseBtn(Btn), X, Y, IsMouseDown, IsMouseMove); };
        }

        internal void Startup()
        {
            DemoList_.Add(new DemoWhole());
            DemoList_.Add(new DemoMemoryEditor());
            DemoList_.Add(new DemoColorPicker());
            DemoList_.Add(new DemoNewControl());

            ChangeToDemo(3);
        }

        internal void Shutdown()
        {
            if (DemoIndex_ != -1)
            {
                DemoList_[DemoIndex_].Shutdown();
                DemoIndex_ = -1;
            }
        }

        internal void ChangeToDemo(int Index)
        {
            if (Index < 0 || Index >= DemoList_.Count)
            {
                return;
            }

            if (DemoIndex_ != -1)
            {
                DemoList_[DemoIndex_].Shutdown();
                LGui.ClearContext();
            }
            DemoIndex_ = Index;

            if (!DemoList_[DemoIndex_].Startup())
            {
                DemoIndex_ = -1;
            }
        }
        
        internal void Tick(float Seconds)
        {
            var IO = LGui.GetIO();
            IO.MouseWheel = Input.GetMouseWheel();
            IO.MouseWheel = IO.MouseWheel > 0.0f ? 1.0f : (IO.MouseWheel < 0.0f ? -1.0f : 0.0f);

            IO.DeltaTime = Seconds;

            if (Input.IsKeyPressed(Keys.PageUp))
            {
                DemoIndex_ = DemoIndex_ > 0 ? DemoIndex_ - 1 : DemoList_.Count - 1;
                ChangeToDemo(DemoIndex_);
            }
            else if (Input.IsKeyPressed(Keys.PageDown))
            {
                DemoIndex_ = DemoIndex_ < DemoList_.Count - 1 ? DemoIndex_ + 1 : 0;
                ChangeToDemo(DemoIndex_);
            }

            if (DemoIndex_ != -1)
            {
                DemoList_[DemoIndex_].Tick(Seconds);
            }
        }

        internal void Render(Graphics Device)
        {
            Executor_.SetDevice(Device);
            WinConvert.SetDevice(Device);

            LGui.Begin();

            if (DemoIndex_ != -1)
            {
                DemoList_[DemoIndex_].Render();
            }
            else
            {
                Device.FillRectangle(BrushCache.GetOrCreate(LGuiColor.Red), 0, 0, WinSize.X, WinSize.Y);
            }

            LGui.End();

            Device.DrawString($"DrawCall : {LGui.GetDrawCall()}", TextFont_, Brushes.White, 800, 520);
        }
    }
}