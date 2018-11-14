using LiteGui;

namespace LiteGuiDemo.Demos
{
    internal class DemoColorPicker : DemoBase
    {
        private LGuiColor Color_ = LGuiColor.Black;
        
        internal DemoColorPicker()
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
            LGui.Text(Color_, "Picker Color");

            if (LGui.Button("ColorPicker"))
            {
                LGui.OpenPopup("Color Picker Popup 1");
            }
            
            if (LGui.BeginPopup("Color Picker Popup 1", /*new LGuiRect(100, 100, 260, 310)*/new LGuiVec2(260, 310)))
            {
                LGui.ColorPicker("ColorPicker1", ref Color_);
                LGui.EndPopup();
            }
        }
    }
}