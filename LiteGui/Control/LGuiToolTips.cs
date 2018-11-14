using LiteGui.Graphics;

namespace LiteGui.Control
{
    internal static class LGuiToolTips
    {
        internal static bool Begin(LGuiVec2 Size)
        {
            if (!LGuiMisc.PreviousControlIsHovered())
            {
                return false;
            }
            
            var Pos = LGuiContext.IO.MousePos + new LGuiVec2(20, 5);
            var Rect = new LGuiRect(Pos, Size);

            LGuiGraphics.SetCurrentLevel(LGuiCommandLevel.VeryHigh);
            LGuiGraphics.DrawRect(Rect, LGuiStyleColorIndex.Frame, true);
            LGuiFrame.Begin(LGuiSettings.DefaultToolTipsTitle, Rect, false);
            return true;
        }

        internal static void End()
        {
            LGuiFrame.End();
            LGuiGraphics.RestoreCurrentLevel();
        }
    }
}