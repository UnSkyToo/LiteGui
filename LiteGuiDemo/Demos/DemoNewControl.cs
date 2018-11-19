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

            LGui.Texture("1.jpg");

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

            if (LGui.BeginWindow("Second Window", new LGuiRect(400, 100, 300, 300), LGuiWindowFlags.NoCollapse))
            {
                if (LGui.Button("Click Me"))
                {
                    System.Console.WriteLine("Window2");
                }
                LGui.EndWindow();
            }

            if (LGui.BeginWindow("Third Window", new LGuiRect(200, 300, 300, 300), LGuiWindowFlags.NoTitle))
            {
                if (LGui.Button("Click Me"))
                {
                    System.Console.WriteLine("Window3");
                }
                LGui.EndWindow();
            }

            LGui.Text(LGuiColor.Red, "aaa");

            LGui.ProgressBar("bar1", Color.R);
        }
    }
}