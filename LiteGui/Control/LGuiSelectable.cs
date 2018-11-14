using LiteGui.Graphics;

namespace LiteGui.Control
{
    internal static class LGuiSelectable
    {
        internal static bool OnProcess(string Text, bool Selected)
        {
            var Size = LGuiConvert.GetTextSize(Text, LGuiContext.Font);
            return OnProcess(Text, Selected, Size);
        }

        internal static bool OnProcess(string Text, bool Selected, float Width)
        {
            return OnProcess(Text, Selected, new LGuiVec2(Width, LGuiContext.Font.FontHeight));
        }

        internal static bool OnProcess(string Text, bool Selected, LGuiVec2 Size)
        {
            var Rect = LGuiLayout.DoLayout(Size);
            return OnProcess(Text, Selected, Rect);
        }

        internal static bool OnProcess(string Text, bool Selected, LGuiRect Rect)
        {
            var ID = LGuiHash.CalculateID(Text);
            LGuiContext.SetPreviousControlID(ID);

            if (!LGuiMisc.CheckVisible(ref Rect))
            {
                return false;
            }

            LGuiMisc.CheckAndSetContextID(ref Rect, ID);
            
            var BgColorIndex = LGuiContext.ActiveID == ID ? LGuiStyleColorIndex.HeaderActive :
                LGuiContext.HoveredID == ID ? LGuiStyleColorIndex.HeaderHovered :
                Selected ? LGuiStyleColorIndex.Header : LGuiStyleColorIndex.Frame;
            LGuiGraphics.DrawRect(Rect, BgColorIndex, true, false);
            
            LGuiGraphics.DrawText(Text, new LGuiVec2(Rect.X, Rect.Y), LGuiStyleColorIndex.Text);

            if (LGuiMisc.CheckAndSetFocusID(ID))
            {
                return true;
            }

            return false;
        }
    }
}