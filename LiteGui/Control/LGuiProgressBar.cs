using LiteGui.Graphics;

namespace LiteGui.Control
{
    internal static class LGuiProgressBar
    {
        internal static void OnProcess(string Title, float Value)
        {
            var Size = new LGuiVec2(LGuiStyle.GetValue(LGuiStyleValueIndex.LargeControlLength), LGuiStyle.GetValue(LGuiStyleValueIndex.SliderSize));
            OnProcess(Title, Value, Size);
        }

        internal static void OnProcess(string Title, float Value, LGuiVec2 Size)
        {
            var Rect = LGuiLayout.DoLayout(Size);
            OnProcess(Title, Value, Rect);
        }
        
        internal static void OnProcess(string Title, float Value, LGuiRect Rect)
        {
            var ID = LGuiHash.CalculateID(Title);
            LGuiContext.SetPreviousControlID(ID);

            if (!LGuiMisc.CheckVisible(ref Rect))
            {
                return;
            }

            LGuiMisc.CheckAndSetContextID(ref Rect, ID, true);

            Value = LGuiMisc.Clamp01(Value);
            var MaskRect = new LGuiRect(Rect.X + 1, Rect.Y + 1, (Rect.Width - 2) * Value, Rect.Height - 2);

            LGuiGraphics.DrawRect(Rect, LGuiStyleColorIndex.Frame, true, false);
            LGuiGraphics.DrawRect(MaskRect, LGuiStyleColorIndex.CheckMask, true, false);
            LGuiGraphics.DrawRect(Rect, LGuiStyleColorIndex.Border, false, false);

            var Text = $"{(Value * 100):00}%";
            var TextSize = LGuiConvert.GetTextSize(Text, LGuiContext.Font);
            LGuiGraphics.DrawText(Text, new LGuiVec2(Rect.CenterX - TextSize.X / 2.0f, Rect.CenterY - TextSize.Y / 2.0f), LGuiStyleColorIndex.Text);
        }
    }
}