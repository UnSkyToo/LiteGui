﻿using LiteGui;

namespace LiteGuiDemo.Demos
{
    internal class DemoNewControl : DemoBase
    {
        internal DemoNewControl()
        {
        }

        internal override bool Startup()
        {
            return true;
        }

        internal override void Shutdown()
        {
        }

        internal override void Tick(float Seconds)
        {
        }
        
        internal override void Render()
        {
            if (LGui.BeginWindow("First Window", new LGuiVec2(300, 300)))
            {
                if (LGui.Button("Click Me"))
                {
                    System.Console.WriteLine("Window1");
                }
                LGui.SetToolTips("tips");
                LGui.EndWindow();
            }

            if (LGui.BeginWindow("Second Window", new LGuiVec2(300, 300)))
            {
                if (LGui.Button("Click Me"))
                {
                    System.Console.WriteLine("Window2");
                }
                LGui.EndWindow();
            }

            LGui.Text(LGuiColor.Red, "aaa");
        }
    }
}