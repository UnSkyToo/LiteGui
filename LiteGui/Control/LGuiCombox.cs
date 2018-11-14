using LiteGui.Graphics;

namespace LiteGui.Control
{
    internal static class LGuiCombox
    {
        internal static bool OnProcess(string Title, ref int ItemIndex, string[] Items, LGuiVec2 Size)
        {
            var Rect = LGuiLayout.DoLayout(Size);
            return OnProcess(Title, ref ItemIndex, Items, Rect);
        }

        internal static bool OnProcess(string Title, ref int ItemIndex, string[] Items, LGuiRect Rect)
        {
            var ID = LGuiHash.CalculateID(Title);
            LGuiContext.SetPreviousControlID(ID);

            if (!LGuiMisc.CheckVisible(ref Rect))
            {
                return false;
            }

            
            var TitleRect = new LGuiRect(Rect.Pos, new LGuiVec2(Rect.Width, LGuiContext.Font.FontHeight));
            var TitleBgColorIndex = LGuiContext.ActiveID == ID ? LGuiStyleColorIndex.FrameActived :
                LGuiContext.HoveredID == ID ? LGuiStyleColorIndex.FrameHovered : LGuiStyleColorIndex.Frame;

            LGuiMisc.CheckAndSetContextID(ref TitleRect, ID);

            LGuiGraphics.DrawRect(TitleRect, TitleBgColorIndex, true);
            LGuiGraphics.DrawText(Items[ItemIndex], TitleRect.Pos + new LGuiVec2(LGuiStyle.GetValue(LGuiStyleValueIndex.ControlSpacingX), 0), LGuiStyleColorIndex.Text);

            var GrabRect = new LGuiRect(Rect.Right - LGuiContext.Font.FontHeight, Rect.Y, LGuiContext.Font.FontHeight, LGuiContext.Font.FontHeight);
            var GrabBgColorIndex = LGuiContext.ActiveID == ID ? LGuiStyleColorIndex.ButtonActived :
                LGuiContext.HoveredID == ID ? LGuiStyleColorIndex.ButtonHovered : LGuiStyleColorIndex.Button;
            LGuiGraphics.DrawRect(GrabRect, GrabBgColorIndex, true);
            LGuiGraphics.DrawTriangle(
                new LGuiVec2(GrabRect.Left + 4, GrabRect.Top + 4),
                new LGuiVec2(GrabRect.Right - 4, GrabRect.Top + 4),
                new LGuiVec2(GrabRect.CenterX, GrabRect.Bottom - 4),
                LGuiStyle.GetColor(LGuiStyleColorIndex.Text), true);

            LGuiGraphics.DrawRect(TitleRect, LGuiStyleColorIndex.Border, false);

            var PopupID = $"{Title}_ItemPopup";
            if (LGuiMisc.CheckAndSetFocusID(ID))
            {
                LGuiPopup.Open(PopupID);
            }

            var NewIndex = ItemIndex;
            if (LGuiPopup.Begin(PopupID, new LGuiRect(Rect.X, TitleRect.Bottom + 1, Rect.Width, Rect.Height)))
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