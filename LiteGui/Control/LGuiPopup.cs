using LiteGui.Graphics;

namespace LiteGui.Control
{
    internal static class LGuiPopup
    {
        internal static void Open(string Title)
        {
            Open(Title, LGuiContext.IO.MousePos + LGuiStyle.GetFrameChildSpacing());
        }

        internal static void Open(string Title, LGuiVec2 Pos)
        {
            var FullTitle = $"{LGuiContext.GetCurrentFrame().Title}/{Title}";
            LGuiContextCache.SetPopupOpen(FullTitle, true);
            LGuiContextCache.SetPopupPos(FullTitle, Pos);
        }

        internal static bool Begin(string Title, LGuiVec2 Size)
        {
            var FullTitle = $"{LGuiContext.GetCurrentFrame().Title}/{Title}";
            var Rect = new LGuiRect(LGuiContextCache.GetPopupPos(FullTitle), Size);
            return Begin(Title, Rect);
        }

        internal static bool Begin(string Title, LGuiRect Rect)
        {
            var FullTitle = $"{LGuiContext.GetCurrentFrame().Title}/{Title}";
            var ID = LGuiHash.CalculateID(FullTitle);
            LGuiContext.SetPreviousControlID(ID);
            
            if (!LGuiMisc.CheckVisible(ref Rect))
            {
                return false;
            }
            
            var IsOpen = LGuiContextCache.GetPopupOpen(FullTitle);
            if (IsOpen)
            {
                if (LGuiContext.IO.IsMouseClick(LGuiMouseButtons.Left) && !LGuiMisc.Contains(ref Rect, ref LGuiContext.IO.MousePos))
                {
                    LGuiContextCache.SetPopupOpen(FullTitle, false);
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