using LiteGui.Graphics;

namespace LiteGui.Control
{
    internal static class LGuiCombox
    {
        internal static bool OnProcess(string Title, ref int ItemIndex, string[] Items, float Width, float PopupHeight)
        {
            var Rect = LGuiLayout.DoLayout(new LGuiVec2(Width, LGuiContext.Font.FontHeight));
            return OnProcess(Title, ref ItemIndex, Items, Rect, PopupHeight);
        }

        internal static bool OnProcess(string Title, ref int ItemIndex, string[] Items, LGuiRect Rect, float PopupHeight)
        {
            var ID = LGuiHash.CalculateID(Title);
            LGuiContext.SetPreviousControlID(ID);

            if (!LGuiMisc.CheckVisible(ref Rect))
            {
                return false;
            }
            
            var TitleBgColorIndex = LGuiContext.ActiveID == ID ? LGuiStyleColorIndex.FrameActived :
                LGuiContext.HoveredID == ID ? LGuiStyleColorIndex.FrameHovered : LGuiStyleColorIndex.Frame;

            LGuiMisc.CheckAndSetContextID(ref Rect, ID);

            LGuiGraphics.DrawRect(Rect, TitleBgColorIndex, true);
            LGuiGraphics.DrawText(Items[ItemIndex], Rect.Pos + new LGuiVec2(LGuiStyle.GetValue(LGuiStyleValueIndex.ControlSpacingX), 0), LGuiStyleColorIndex.Text);

            var GrabRect = new LGuiRect(Rect.Right - LGuiContext.Font.FontHeight, Rect.Y, LGuiContext.Font.FontHeight, LGuiContext.Font.FontHeight);
            var GrabBgColorIndex = LGuiContext.ActiveID == ID ? LGuiStyleColorIndex.ButtonActived :
                LGuiContext.HoveredID == ID ? LGuiStyleColorIndex.ButtonHovered : LGuiStyleColorIndex.Button;
            LGuiGraphics.DrawRect(GrabRect, GrabBgColorIndex, true);
            LGuiGraphics.DrawTriangle(
                new LGuiVec2(GrabRect.Left + 4, GrabRect.Top + 4),
                new LGuiVec2(GrabRect.Right - 4, GrabRect.Top + 4),
                new LGuiVec2(GrabRect.CenterX, GrabRect.Bottom - 4),
                LGuiStyle.GetColor(LGuiStyleColorIndex.Text), true);

            LGuiGraphics.DrawRect(Rect, LGuiStyleColorIndex.Border, false);

            var PopupID = $"{Title}_ItemPopup";
            if (LGuiMisc.CheckAndSetFocusID(ID))
            {
                LGuiPopup.Open(PopupID);
            }

            var NewIndex = ItemIndex;
            if (LGuiPopup.Begin(PopupID, new LGuiRect(Rect.X, Rect.Bottom + 1, Rect.Width, PopupHeight)))
            {
                var ItemWidth = Rect.Width - LGuiStyle.GetValue(LGuiStyleValueIndex.FrameChildSpacingX) * 2.0f - LGuiStyle.GetValue(LGuiStyleValueIndex.SliderSize);
                for (var Index = 0; Index < Items.Length; ++Index)
                {
                    if (LGuiSelectable.OnProcess(Items[Index], ItemIndex == Index, ItemWidth))
                    {
                        NewIndex = Index;
                        LGuiPopup.Close(PopupID);
                    }
                }
                LGuiPopup.End();
            }

            if (ItemIndex != NewIndex)
            {
                ItemIndex = NewIndex;
                return true;
            }

            return false;
        }
    }
}