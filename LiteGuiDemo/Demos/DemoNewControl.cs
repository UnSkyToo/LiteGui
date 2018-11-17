using LiteGui;

namespace LiteGuiDemo.Demos
{
    internal class DemoNewControl : DemoBase
    {
        private LGuiColor Color;
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
            LGui.BeginFrame("Test", new LGuiRect(500, 5, 400, 400));

            LGui.Button("asd");

            LGui.EndFrame();

            if (LGui.BeginWindow("First Window", new LGuiVec2(300, 300)))
            {
                if (LGui.Button("Click Me"))
                {
                    System.Console.WriteLine("Window1");
                }

                LGui.ColorPicker("asd", ref Color);

                LGui.EndWindow();
            }

            if (LGui.BeginWindow("Second Window", new LGuiRect(400, 100, 300, 300)))
            {
                if (LGui.Button("Click Me"))
                {
                    System.Console.WriteLine("Window2");
                }
                LGui.EndWindow();
            }

            LGui.Text(LGuiColor.Red, "aaa");

            LGui.ProgressBar("bar1", Color.R);
        }
    }
}