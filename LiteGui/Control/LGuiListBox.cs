namespace LiteGui.Control
{
    internal static class LGuiListBox
    {
        internal static bool OnProcess(string Title, ref int ItemIndex, string[] Items, LGuiVec2 Size)
        {
            var Rect = LGuiLayout.DoLayout(Size);
            return OnProcess(Title, ref ItemIndex, Items, Rect);
        }

        internal static bool OnProcess(string Title, ref int ItemIndex, string[] Items, LGuiRect Rect)
        {
            LGuiFrame.Begin(Title, Rect);
            
            var ItemWidth = Rect.Width - LGuiStyle.GetValue(LGuiStyleValueIndex.FrameChildSpacingX) * 2.0f - LGuiStyle.GetValue(LGuiStyleValueIndex.SliderSize);
            for (var Index = 0; Index < Items.Length; ++Index)
            {
                if (LGuiSelectable.OnProcess(Items[Index], ItemIndex == Index, ItemWidth))
                {
                    ItemIndex = Index;
                }
            }

            LGuiFrame.End();

            return false;
        }
    }
}