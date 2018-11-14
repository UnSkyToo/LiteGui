using LiteGui.Graphics;

namespace LiteGui.Control
{
    internal static class LGuiColorButton
    {
        internal static bool OnProcess(string Title, LGuiColor Color, LGuiVec2 Size)
        {
            var Rect = LGuiLayout.DoLayout(Size);
            return OnProcess(Title, Color, Rect);
        }

        internal static bool OnProcess(string Title, LGuiColor Color, LGuiRect Rect)
        {
            var ID = LGuiHash.CalculateID(Title);
            LGuiContext.SetPreviousControlID(ID);

            if (!LGuiMisc.CheckVisible(ref Rect))
            {
                return false;
            }

            LGuiMisc.CheckAndSetContextID(ref Rect, ID);
            
            LGuiGraphics.DrawRect(Rect, Color, true);
            LGuiGraphics.DrawRect(Rect, LGuiStyleColorIndex.Border, false);

            if (LGuiMisc.CheckAndSetFocusID(ID))
            {
                return true;
            }

            return false;
        }
    }
}