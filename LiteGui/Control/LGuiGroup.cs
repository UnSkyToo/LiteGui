using LiteGui.Graphics;

namespace LiteGui.Control
{
    internal static class LGuiGroup
    {
        internal static bool Begin(string Title)
        {
            var FullTitle = $"{LGuiContext.GetCurrentFrame().Title}/{Title}";
            var ID = LGuiHash.CalculateID(FullTitle);
            LGuiContext.SetPreviousControlID(ID);

            var Rect = LGuiLayout.DoLayout(new LGuiVec2(LGuiStyle.GetValue(LGuiStyleValueIndex.LargeControlLength), LGuiStyle.GetValue(LGuiStyleValueIndex.HeaderSize)));
            if (!LGuiMisc.CheckVisible(ref Rect))
            {
                return false;
            }

            LGuiMisc.CheckAndSetContextID(ref Rect, ID);

            var BgColorIndex = LGuiContext.ActiveID == ID ? LGuiStyleColorIndex.GroupActived :
                LGuiContext.HoveredID == ID ? LGuiStyleColorIndex.GroupHovered : LGuiStyleColorIndex.Group;

            LGuiGraphics.DrawRect(Rect, BgColorIndex, true);
            LGuiGraphics.DrawRect(Rect, LGuiStyleColorIndex.Border, false);
            LGuiGraphics.DrawText(Title, new LGuiVec2(Rect.X + LGuiStyle.GetValue(LGuiStyleValueIndex.GroupChildSpacing), Rect.Y), LGuiStyleColorIndex.Text);

            var Expand = LGuiContextCache.GetGroupExpand(FullTitle);
            if (Expand)
            {
                LGuiGraphics.DrawTriangle(
                    new LGuiVec2(Rect.X + 5, Rect.Y + 3),
                    new LGuiVec2(Rect.X + 15, Rect.Y + 3),
                    new LGuiVec2(Rect.X + 10, Rect.Bottom - 3),
                    LGuiStyle.GetColor(LGuiStyleColorIndex.Text),
                    true);
            }
            else
            {
                LGuiGraphics.DrawTriangle(
                    new LGuiVec2(Rect.X + 5, Rect.Y + 3),
                    new LGuiVec2(Rect.X + 15, Rect.CenterY),
                    new LGuiVec2(Rect.X + 5, Rect.Bottom - 3),
                    LGuiStyle.GetColor(LGuiStyleColorIndex.Text),
                    true);
            }

            if (LGuiMisc.CheckAndSetFocusID(ID))
            {
                Expand = !Expand;
                LGuiContextCache.SetGroupExpand(FullTitle, Expand);
            }

            if (Expand)
            {
                LGuiContext.BeginGroup(FullTitle, Rect.X + LGuiStyle.GetValue(LGuiStyleValueIndex.GroupChildSpacing));
            }
            return Expand;
        }

        internal static void End()
        {
            LGuiContext.EndGroup();
        }
    }
}