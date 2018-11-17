using LiteGui.Graphics;

namespace LiteGui.Control
{
    internal static class LGuiWindow
    {
        internal static bool Begin(string Title, LGuiVec2 Size, LGuiWindowFlags Flags)
        {
            return Begin(Title, new LGuiRect(LGuiLayout.GetCurrentLayoutContext().CursorPos, Size), Flags);
        }

        internal static bool Begin(string Title, LGuiRect InitRect, LGuiWindowFlags Flags)
        {
            if (LGuiContext.CurrentWindow != null)
            {
                return false;
            }

            var FullTitle = $"{LGuiContext.GetCurrentFrame().Title}/{Title}";
            var Rect = LGuiContextCache.GetWindowRect(FullTitle);
            if (Rect.Width == 0 || Rect.Height == 0)
            {
                Rect = InitRect;
                LGuiContextCache.SetWindowRect(FullTitle, Rect);
            }

            if (!LGuiMisc.CheckVisible(ref Rect))
            {
                return false;
            }

            var ID = LGuiHash.Calculate(FullTitle);
            
            LGuiContext.CurrentWindow = new LGuiWindowContext(FullTitle, ID, Rect,
                (Flags & LGuiWindowFlags.NoMove) != LGuiWindowFlags.NoMove,
                (Flags & LGuiWindowFlags.NoFocus) != LGuiWindowFlags.NoFocus);

            if (LGuiContext.FocusWindow == null)
            {
                LGuiContext.FocusWindow = LGuiContext.CurrentWindow;
            }

            LGuiGraphics.SetCurrentLevel(LGuiContext.FocusWindow.ID == ID ? LGuiCommandLevel.FocusWindow : LGuiCommandLevel.Window);

            if ((Flags & LGuiWindowFlags.NoTitle) != LGuiWindowFlags.NoTitle)
            {
                var TitleRect = new LGuiRect(Rect.Pos, new LGuiVec2(Rect.Width, LGuiContext.Font.FontHeight));
                LGuiGraphics.DrawRect(Rect, LGuiStyleColorIndex.Frame, true);
                LGuiGraphics.DrawRect(TitleRect, LGuiStyleColorIndex.HeaderActive, true);
                LGuiGraphics.DrawText(Title, TitleRect.Pos + new LGuiVec2(LGuiStyle.GetValue(LGuiStyleValueIndex.HeaderSize), 0), LGuiStyleColorIndex.Text);
                LGuiGraphics.DrawTriangle(
                    new LGuiVec2(TitleRect.X + 5, TitleRect.Top + 5),
                    new LGuiVec2(TitleRect.X + 15, TitleRect.Top + 5),
                    new LGuiVec2(TitleRect.X + 10, TitleRect.Bottom - 5),
                    LGuiStyle.GetColor(LGuiStyleColorIndex.Text), true);
                LGuiGraphics.DrawRect(TitleRect, LGuiStyleColorIndex.Border, false);

                var ContextRect = new LGuiRect(Rect.X, TitleRect.Bottom, Rect.Width, Rect.Height - TitleRect.Height);
                LGuiFrame.Begin(Title, ContextRect, false);
            }
            else
            {
                LGuiFrame.Begin(Title, Rect, false);
            }

            return true;
        }

        internal static void End()
        {
            var Title = LGuiContext.GetCurrentFrame().Title;
            var ID = LGuiHash.Calculate(Title);

            LGuiFrame.End();
            LGuiGraphics.RestoreCurrentLevel();

            // Ignore other window when mousepos in FocusWindow
            if (LGuiContext.FocusWindow.ID != ID && LGuiMisc.Contains(ref LGuiContext.FocusWindow.Rect, ref LGuiContext.IO.MousePos))
            {
                LGuiContext.CurrentWindow = null;
                return;
            }

            var Rect = LGuiContextCache.GetWindowRect(Title);
            if (LGuiMisc.Contains(ref Rect, ref LGuiContext.IO.MousePos))
            {
                if (LGuiContext.IO.IsMouseClick(LGuiMouseButtons.Left))
                {
                    if (LGuiContext.FocusWindow.ID != ID && LGuiContext.CurrentWindow.Focusable)
                    {
                        LGuiContext.FocusWindow = LGuiContext.CurrentWindow;
                    }

                    LGuiContextCache.SetWindowOrginPos(Title, Rect.Pos);
                }

                if (LGuiContext.CurrentWindow.Moveable)
                {
                    LGuiMisc.CheckAndSetContextID(ref Rect, ID);
                    if (LGuiContext.ActiveID == ID && LGuiContext.IO.IsMouseDown(LGuiMouseButtons.Left))
                    {
                        var WindowOriginPos = LGuiContextCache.GetWindowOrginPos(Title);
                        var MovePos = LGuiContext.IO.GetMouseMovePos();
                        Rect.Pos = WindowOriginPos + MovePos;
                        LGuiContextCache.SetWindowRect(Title, Rect);
                        LGuiContext.CurrentWindow.Rect = Rect;
                    }
                }
            }

            LGuiContext.CurrentWindow = null;
        }
    }
}