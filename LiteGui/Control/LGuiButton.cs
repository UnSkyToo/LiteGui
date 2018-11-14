using LiteGui.Graphics;

namespace LiteGui.Control
{
    internal static class LGuiButton
    {
        internal static bool OnProcess(string Title, LGuiButtonFlags Flags)
        {
            var Size = LGuiConvert.GetTextSize(Title, LGuiContext.Font).Add(6, 4);
            return OnProcess(Title, Size, Flags);
        }

        internal static bool OnProcess(string Title, LGuiVec2 Size, LGuiButtonFlags Flags)
        {
            var Rect = LGuiLayout.DoLayout(Size);
            return OnProcess(Title, Rect, Flags);
        }

        internal static bool OnProcess(string Title, LGuiRect Rect, LGuiButtonFlags Flags)
        {
            var ID = LGuiHash.CalculateID(Title);
            LGuiContext.SetPreviousControlID(ID);

            if (!LGuiMisc.CheckVisible(ref Rect))
            {
                return false;
            }

            LGuiMisc.CheckAndSetContextID(ref Rect, ID);

            if ((Flags & LGuiButtonFlags.Invisible) == LGuiButtonFlags.Invisible)
            {
            }
            else
            {
                var BgColorIndex = LGuiContext.ActiveID == ID ? LGuiStyleColorIndex.ButtonActived :
                    LGuiContext.HoveredID == ID ? LGuiStyleColorIndex.ButtonHovered : LGuiStyleColorIndex.Button;

                LGuiGraphics.DrawRect(Rect, BgColorIndex, true);
                LGuiGraphics.DrawRect(Rect, LGuiStyleColorIndex.Border, false);
                LGuiGraphics.DrawText(Title, new LGuiVec2(Rect.X + 3, Rect.Y + 2), LGuiStyleColorIndex.Text);
            }

            if ((Flags & LGuiButtonFlags.Repeat) == LGuiButtonFlags.Repeat)
            {
                if (LGuiContext.ActiveID == ID)
                {
                    return true;
                }
            }
            else if (LGuiMisc.CheckAndSetFocusID(ID))
            {
                return true;
            }

            return false;
        }
    }
}