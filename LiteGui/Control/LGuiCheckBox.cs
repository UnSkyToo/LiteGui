using LiteGui.Graphics;

namespace LiteGui.Control
{
    internal static class LGuiCheckBox
    {
        internal static bool OnProcess(string Title, bool Value)
        {
            var Size = LGuiConvert.GetTextSize(Title, LGuiContext.Font).Add(LGuiStyle.GetValue(LGuiStyleValueIndex.CheckBoxSize) + 3, 2);
            Size.Y = LGuiMisc.Max(Size.Y, LGuiStyle.GetValue(LGuiStyleValueIndex.CheckBoxSize));
            var Rect = LGuiLayout.DoLayout(Size);
            return OnProcess(Title, Value, Rect);
        }

        internal static bool OnProcess(string Title, bool Value, LGuiRect Rect)
        {
            var ID = LGuiHash.CalculateID(Title);
            LGuiContext.SetPreviousControlID(ID);

            if (!LGuiMisc.CheckVisible(ref Rect))
            {
                return Value;
            }

            var BoxSize = LGuiStyle.GetValue(LGuiStyleValueIndex.CheckBoxSize);
            var BoxRect = new LGuiRect(new LGuiVec2(Rect.X, Rect.Y + (Rect.Height - BoxSize) / 2.0f), new LGuiVec2(BoxSize, BoxSize));

            LGuiMisc.CheckAndSetContextID(ref Rect, ID);

            var BgColorIndex = LGuiContext.ActiveID == ID ? LGuiStyleColorIndex.FrameActived :
                LGuiContext.HoveredID == ID ? LGuiStyleColorIndex.FrameHovered : LGuiStyleColorIndex.Frame;

            LGuiGraphics.DrawRect(BoxRect, BgColorIndex, true);
            LGuiGraphics.DrawRect(BoxRect, LGuiStyleColorIndex.Border, false);
            if (Value)
            {
                LGuiGraphics.DrawRect(new LGuiRect(BoxRect.X + 2, BoxRect.Y + 2, BoxRect.Width - 4, BoxRect.Height - 4), LGuiStyleColorIndex.CheckMask, true);
            }
            LGuiGraphics.DrawText(Title, new LGuiVec2(Rect.X + BoxSize + 3, Rect.Y + 1), LGuiStyleColorIndex.Text);

            if (LGuiMisc.CheckAndSetFocusID(ID))
            {
                Value = !Value;
            }

            return Value;
        }
    }
}