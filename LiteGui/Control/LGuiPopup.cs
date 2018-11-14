using LiteGui.Graphics;

namespace LiteGui.Control
{
    internal static class LGuiPopup
    {
        internal static void Open(string Title)
        {
            Open(Title, LGuiContext.IO.MousePos + LGuiStyle.GetFrameChildSpacing());
        }

        internal static void Close(string Title)
        {
            LGuiContextCache.SetPopupOpen(Title, false);
        }

        internal static void Open(string Title, LGuiVec2 Pos)
        {
            LGuiContextCache.SetPopupOpen(Title, true);
            LGuiContextCache.SetPopupPos(Title, Pos);
        }

        internal static bool Begin(string Title, LGuiVec2 Size)
        {
            var Rect = new LGuiRect(LGuiContextCache.GetPopupPos(Title), Size);
            return Begin(Title, Rect);
        }

        internal static bool Begin(string Title, LGuiRect Rect)
        {
            var ID = LGuiHash.CalculateID(Title);
            LGuiContext.SetPreviousControlID(ID);
            
            if (!LGuiMisc.CheckVisible(ref Rect))
            {
                return false;
            }

            var IsOpen = LGuiContextCache.GetPopupOpen(Title);
            if (IsOpen)
            {
                if (LGuiContext.IO.IsMouseClick(LGuiMouseButtons.Left) && !LGuiMisc.Contains(ref Rect, ref LGuiContext.IO.MousePos))
                {
                    LGuiContextCache.SetPopupOpen(Title, false);
                    return false;
                }

                LGuiGraphics.SetCurrentLevel(LGuiCommandLevel.High);
                LGuiGraphics.DrawRect(Rect, LGuiStyleColorIndex.Frame, true);
                LGuiFrame.Begin(Title, Rect, false);
            }

            return IsOpen;
        }

        internal static void End()
        {
            LGuiFrame.End();
            LGuiGraphics.RestoreCurrentLevel();
        }
    }
}