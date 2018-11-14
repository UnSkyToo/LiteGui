using LiteGui;

namespace LiteGuiDemo.Demos
{
    internal class DemoNewControl : DemoBase
    {
        private int ItemIndex = 0;
        private string[] Items = new string[] {"item1", "item2", "item3", "item4", "item5", "item6", "item7", "item8", "item9", "item10"};
        private bool Selected = false;
        private LGuiColor Color = LGuiColor.White;

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
            if (LGui.ColorButton("ColorButton1", Color, new LGuiVec2(30, 30)))
            {
                LGui.OpenPopup("Popup 1");
            }

            if (LGui.BeginPopup("Popup 1", new LGuiVec2(260, 310)))
            {
                LGui.ColorPicker("Color Picker 1", ref Color);
                LGui.EndPopup();
            }

            LGui.ListBox("ListBox", ref ItemIndex, Items, new LGuiVec2(100, 150));
            LGui.Text("Current Item : {0}", Items[ItemIndex]);
        }
    }
}