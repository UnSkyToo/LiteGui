using LiteGui;

namespace LiteGuiDemo.Demos
{
    internal class DemoNewControl : DemoBase
    {
        private int ItemIndex = 0;
        private string[] Items = new string[] {"item1", "item2", "item3", "item4", "item5", "item6", "item7", "item8", "item9", "item10"};

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
            LGui.Combox("Combox", ref ItemIndex, Items, 100, 150);
        }
    }
}