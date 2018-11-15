using LiteGui.Graphics;

namespace LiteGui.Control
{
    internal static class LGuiWindow
    {
        internal static bool Begin(string Title, LGuiVec2 Size)
        {
            var FullTitle = $"{LGuiContext.GetCurrentFrame().Title}/{Title}";
            var Rect = LGuiContextCache.GetWindowRect(FullTitle);
            if (Rect.Width == 0 || Rect.Height == 0)
            {
                Rect.Pos = LGuiStyle.GetFrameChildSpacing();
                Rect.Size = Size;
                LGuiContextCache.SetWindowRect(FullTitle, Rect);
            }

            return Begin(Title, Rect);
        }

        internal static bool Begin(string Title, LGuiRect Rect)
        {
            if (!LGuiMisc.CheckVisible(ref Rect))
            {
                return false;
            }

            var FullTitle = $"{LGuiContext.GetCurrentFrame().Title}/{Title}";
            var ID = LGuiHash.Calculate(FullTitle);

            LGuiGraphics.SetCurrentLevel(LGuiContext.WindowID == ID ? LGuiCommandLevel.VeryHigh : LGuiCommandLevel.High);

            var TitleRect = new LGuiRect(Rect.Pos, new LGuiVec2(Rect.Width, LGuiContext.Font.FontHeight));
            var ContextRect = new LGuiRect(Rect.X, TitleRect.Bottom, Rect.Width, Rect.Height - TitleRect.Height);
            
            LGuiGraphics.DrawRect(Rect, LGuiStyleColorIndex.Frame, true);
            LGuiGraphics.DrawRect(TitleRect, LGuiStyleColorIndex.HeaderActive, true);
            LGuiGraphics.DrawText(Title, TitleRect.Pos + new LGuiVec2(20, 0), LGuiStyleColorIndex.Text);
            LGuiGraphics.DrawTriangle(
                new LGuiVec2(TitleRect.X + 5, TitleRect.Top + 5), 
                new LGuiVec2(TitleRect.X + 15, TitleRect.Top + 5),
                new LGuiVec2(TitleRect.X + 10, TitleRect.Bottom - 5),
                LGuiStyle.GetColor(LGuiStyleColorIndex.Text), true);
            LGuiGraphics.DrawRect(TitleRect, LGuiStyleColorIndex.Border, false);

            LGuiFrame.Begin(Title, ContextRect, false);
            return true;
        }

        internal static void End()
        {
            var Title = LGuiContext.GetCurrentFrame().Title;
            LGuiFrame.End();
            LGuiGraphics.RestoreCurrentLevel();

            var ID = LGuiHash.Calculate(Title);
            var Rect = LGuiContextCache.GetWindowRect(Title);
            LGuiMisc.CheckAndSetContextID(ref Rect, ID);
            
            if (LGuiContext.IO.IsMouseClick(LGuiMouseButtons.Left))
            {
                LGuiContextCache.SetWindowOrginPos(Title, Rect.Pos);
            }

            if (LGuiContext.ActiveID == ID && LGuiContext.IO.IsMouseDown(LGuiMouseButtons.Left))
            {
                var WindowOriginPos = LGuiContextCache.GetWindowOrginPos(Title);
                var MovePos = LGuiContext.IO.GetMouseMovePos();
                Rect.Pos = WindowOriginPos + MovePos;
                LGuiContextCache.SetWindowRect(Title, Rect);

                LGuiContext.WindowID = ID;
            }
        }
    }
}