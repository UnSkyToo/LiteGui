using LiteGui.Graphics;

namespace LiteGui.Control
{
    internal static class LGuiText
    {
        internal static void OnProcess(LGuiColor Color, string Format, params object[] Args)
        {
            var Text = string.Format(Format, Args);
            var Size = LGuiConvert.GetTextSize(Text, LGuiContext.Font);
            var Rect = LGuiLayout.DoLayout(Size);
            OnProcess(Color, Text, Rect);
        }
        
        internal static void OnProcess(LGuiColor Color, string Text, LGuiRect Rect)
        {
            var ID = LGuiHash.CalculateID(Text);
            LGuiContext.SetPreviousControlID(ID);

            if (!LGuiMisc.CheckVisible(ref Rect))
            {
                return;
            }

            LGuiMisc.CheckAndSetContextID(ref Rect, ID);
            LGuiGraphics.DrawText(Text, Rect.Pos, Color, LGuiContext.Font);
        }
    }
}