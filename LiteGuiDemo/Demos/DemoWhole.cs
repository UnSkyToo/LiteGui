using System;
using LiteGui;

namespace LiteGuiDemo.Demos
{
    internal class DemoWhole : DemoBase
    {
        private bool Value_;
        private int RValue_ = 1;
        private float SliderValue_ = 0;
        private int SliderValueInt_ = 0;
        private string Str1_ = "test";//"a\ninput text..\naasdasdad\n12312312312\nasdas\n";
        private string Str2_ = "123";
        private int TextureID_ = 0;
        private LGuiColor Color_ = LGuiColor.White;

        internal DemoWhole()
        {
        }

        internal override bool Startup()
        {
            TextureID_ = TextureCache.Add("1.jpg");
            return true;
        }

        internal override void Shutdown()
        {
            TextureCache.Remove(TextureID_);
        }

        internal override void Tick(float Seconds)
        {
        }
        
        internal override void Render()
        {
            LGui.BeginFrame("Main Window", new LGuiVec2(950, 530));
            
            LGui.Text("LiteGui Demo");
            
            LGui.Separator();

            LGui.BeginLayout(LGuiLayoutMode.Horizontal);
            if (LGui.Button("Save"))
            {
                Console.WriteLine(Value_.ToString());
            }
            LGui.BeginLayout(LGuiLayoutMode.Vertical);
            if (LGui.Button("End1"))
            {
                Console.WriteLine(Value_.ToString());
            }
            if (LGui.Button("End2"))
            {
                Console.WriteLine(Value_.ToString());
            }
            LGui.EndLayout();
            LGui.EndLayout();

            LGui.Separator(LGuiColor.Red);

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

            Value_ = LGui.CheckBox("IsShow", Value_);

            //LGui.BeginLayout(LGuiLayoutMode.Horizontal);
            RValue_ = LGui.RadioButton("Radio 1", 1, RValue_);
            LGui.SameLine();
            RValue_ = LGui.RadioButton("Radio 2", 2, RValue_);
            LGui.SameLine();
            if (LGui.BeginToolTips(new LGuiVec2(200, 60)))
            {
                LGui.Text("This is tips");
                LGui.Button("Tips Btn");
                LGui.EndToolTips();
            }
            
            RValue_ = LGui.RadioButton("Radio 3", 3, RValue_);
            LGui.SetToolTips("this is other tips");
            //LGui.EndLayout();

            LGui.InputText("Input1", ref Str1_);
            LGui.InputText("Input2", ref Str2_);

            LGui.BeginFrame("List1", new LGuiVec2(300, 300));

            LGui.Text(new LGuiColor(1.0f, 0.0f, 0.0f), "Color Text");

            LGui.BeginFrame("List2", new LGuiVec2(100, 200));

            LGui.Text("AA");
            LGui.Text("AA");
            LGui.Text("AA");
            LGui.Text("AA");
            LGui.Text("AA");
            LGui.Text("AA");
            LGui.Text("AA");
            LGui.Text("AA");
            LGui.Text("AA");
            LGui.Text("AA");

            LGui.EndFrame();
            LGui.Separator();

            if (Value_)
            {
                if (LGui.BeginGroup("Test Group 0"))
                {
                    LGui.Text("Group 0 Text");
                    LGui.Text("Group 0 Text");
                    LGui.Text("Group 0 Text");
                    LGui.EndGroup();
                }

                for (var Index = 0; Index < 50; ++Index)
                {
                    LGui.Text("{0} Text", Index);
                    LGui.Slider("Slider" + Index, ref SliderValue_, 0, 1, 0.1f, true, Value_, 200);
                }

                if (LGui.BeginGroup("Test Group 1"))
                {
                    LGui.Text("Group 1 Text");
                    LGui.Text("Group 1 Text");
                    LGui.Text("Group 1 Text");
                    LGui.EndGroup();
                }

                var TPos = LGui.GetCursorPos();
                LGui.Texture(1, new LGuiVec2(50, 50));
                if (LGui.BeginToolTips(new LGuiVec2(110, 110)))
                {
                    var MousePos = LGui.GetIO().MousePos;
                    var BeginPos = MousePos - TPos;
                    var TexSize = TextureCache.GetSize(1);
                    BeginPos.X = BeginPos.X / 50.0f * TexSize.X;
                    BeginPos.Y = BeginPos.Y / 50.0f * TexSize.Y;
                    var SrcRect = new LGuiRect(BeginPos, new LGuiVec2(90, 70));
                    SrcRect.Size = new LGuiVec2(Math.Min(SrcRect.Size.X, TexSize.X), Math.Min(SrcRect.Size.Y, TexSize.Y));
                    LGui.Text("X:{0} Y:{1}", (int)BeginPos.X, (int)BeginPos.Y);
                    LGui.Texture(1, SrcRect, new LGuiVec2(90, 70));
                    LGui.EndToolTips();
                }
            }

            LGui.EndFrame();

            //LGui.Separator();
            LGui.Button("Push Text");
            //LGui.Separator();
            
            if (LGui.BeginGroup("Test Group 2"))
            {
                LGui.Text("Group 2 Text");
                LGui.Text("Group 2 Text");
                LGui.Text("Group 2 Text");
                
                if (LGui.BeginGroup("Test Group 22"))
                {
                    LGui.BeginFrame("FFFF", new LGuiVec2(100, 100));
                    {
                        LGui.Text("Group 222 Text");
                        LGui.Text("Group 222 Text");
                    }
                    LGui.EndFrame();
                    LGui.EndGroup();
                }
                LGui.EndGroup();
            }
            
            if (LGui.BeginGroup("Test Group 3"))
            {
                LGui.Text("Group 3 Text");
                LGui.EndGroup();
            }

            LGui.EndFrame();
        }
    }
}