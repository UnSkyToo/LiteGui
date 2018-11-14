using LiteGui.Graphics;

namespace LiteGui.Control
{
    internal static class LGuiRadioButton
    {
        internal static int OnProcess(string Title, int Index, int Value)
        {
            var Radius = LGuiStyle.GetValue(LGuiStyleValueIndex.RadioButtonRadius);
            var Size = LGuiConvert.GetTextSize(Title, LGuiContext.Font).Add(Radius * 2 + 3, 2);
            Size.Y = LGuiMisc.Max(Size.Y, Radius);
            var Rect = LGuiLayout.DoLayout(Size);
            return OnProcess(Title, Index, Value, Rect);
        }

        internal static int OnProcess(string Title, int Index, int Value, LGuiRect Rect)
        {
            var ID = LGuiHash.CalculateID(Title);
            LGuiContext.SetPreviousControlID(ID);

            if (!LGuiMisc.CheckVisible(ref Rect))
            {
                return Value;
            }
            
            LGuiMisc.CheckAndSetContextID(ref Rect, ID);

            var BgColorIndex = LGuiContext.ActiveID == ID ? LGuiStyleColorIndex.FrameActived :
                LGuiContext.HoveredID == ID ? LGuiStyleColorIndex.FrameHovered : LGuiStyleColorIndex.Frame;

            var Radius = LGuiStyle.GetValue(LGuiStyleValueIndex.RadioButtonRadius);
            var RadioCenter = new LGuiVec2(Rect.X + Radius, Rect.Y + Rect.Height / 2.0f);
            LGuiGraphics.DrawCircle(RadioCenter, Radius, BgColorIndex, true);
            LGuiGraphics.DrawCircle(RadioCenter, Radius, LGuiStyleColorIndex.Border, false);
            if (Index == Value)
            {
                LGuiGraphics.DrawCircle(RadioCenter, Radius - 3, LGuiStyleColorIndex.CheckMask, true);
            }
            LGuiGraphics.DrawText(Title, new LGuiVec2(Rect.X + Radius * 2 + 3, Rect.Y + 1), LGuiStyleColorIndex.Text);

            if (LGuiMisc.CheckAndSetFocusID(ID))
            {
                Value = Index;
            }

            return Value;
        }
    }
}