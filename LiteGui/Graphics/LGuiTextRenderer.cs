using System.Collections.Generic;

namespace LiteGui.Graphics
{
    internal class LGuiTextRenderInfo
    {
        internal string Text;
        internal LGuiColor TextColor;
        internal LGuiFont Font;
        internal bool OnlyShowText;

        internal uint Cursor;
        internal uint CursorWidth;
        internal LGuiColor CursorColor;

        internal uint SelectStart;
        internal uint SelectEnd;
        internal LGuiColor SelectColor;
        
        internal bool IsHidden;
        internal char HiddenChar;

        internal LGuiVec2 Spacing;
        internal uint OffsetX;
        internal uint OffsetY;
        internal uint CountX;
        internal uint CountY;
    }

    internal static class LGuiTextRenderer
    {
        internal static void Render(LGuiTextRenderInfo Info, LGuiVec2 RenderPos)
        {
            var FontWidth = Info.Font.FontWidth + Info.Spacing.X;
            var FontHeight = Info.Font.FontHeight + Info.Spacing.Y;
            var Padding = new LGuiVec2(Info.Spacing.X / 2.0f, Info.Spacing.Y / 2.0f);

            var SelectRects = new List<LGuiRect>();
            var HasSelect = Info.SelectStart != Info.SelectEnd && !Info.OnlyShowText;
            var SelectStart = LGuiMisc.Min(Info.SelectStart, Info.SelectEnd);
            var SelectEnd = LGuiMisc.Min((uint)Info.Text.Length, LGuiMisc.Max(Info.SelectStart, Info.SelectEnd));
            var InSelect = false;
            
            var BeginPos = RenderPos - new LGuiVec2(Info.OffsetX * FontWidth, Info.OffsetY * FontHeight);
            var Pos = BeginPos;
            var CursorPos = Pos;
            var SelectPos = Pos;
            
            var TextCmdList = LGuiGraphics.CreateCommandList();

            var CharX = 0u;
            var CharY = 0u;

            var Index = 0u;
            foreach (var Ch in Info.Text)
            {
                if (Index == Info.Cursor)
                {
                    CursorPos = Pos;
                }

                if (HasSelect)
                {
                    if (Index == SelectStart)
                    {
                        SelectPos = Pos;
                        InSelect = true;
                    }

                    if (Index == SelectEnd)
                    {
                        SelectRects.Add(new LGuiRect(SelectPos, new LGuiVec2(Pos.X - SelectPos.X, FontHeight)));
                        InSelect = false;
                        HasSelect = false;
                    }

                    if (InSelect && Ch == '\n')
                    {
                        SelectRects.Add(new LGuiRect(SelectPos, new LGuiVec2(Pos.X - SelectPos.X + FontWidth, FontHeight)));
                        SelectPos = new LGuiVec2(BeginPos.X, Pos.Y + FontHeight);
                    }
                }
                
                if (CharX >= Info.OffsetX  && CharY >= Info.OffsetY && CharX < (Info.OffsetX + Info.CountX) && CharY < (Info.OffsetY + Info.CountY))
                {
                    TextCmdList.DrawText((Info.IsHidden ? Info.HiddenChar : Ch).ToString(), Pos + Padding, Info.TextColor, Info.Font);
                }
                
                if (Ch == '\n')
                {
                    Pos.X = BeginPos.X;
                    Pos.Y = Pos.Y + FontHeight;
                    CharX = 0u;
                    CharY++;
                }
                else
                {
                    Pos.X = Pos.X + FontWidth;
                    CharX++;
                }

                Index++;
            }

            if (InSelect)
            {
                if (Info.Text[Info.Text.Length - 1] != '\n')
                {
                    SelectRects.Add(new LGuiRect(SelectPos, new LGuiVec2(Pos.X - SelectPos.X, FontHeight)));
                }
            }

            var ViewRect = new LGuiRect(RenderPos, new LGuiVec2(Info.CountX * FontWidth + 1.0f, Info.CountY * FontHeight));
            var SelectResult = LGuiRect.Zero;
            foreach (var Rect in SelectRects)
            {
                if (!LGuiMisc.IntersectRect(Rect, ViewRect, ref SelectResult))
                {
                    continue;
                }

                LGuiGraphics.DrawRect(SelectResult, Info.SelectColor, true, false);
            }
            
            LGuiGraphics.AddCommandList(TextCmdList);

            if (!Info.OnlyShowText && ((LGuiContext.FrameCount >> 4) & 1) == 1)
            {
                if (Info.Cursor == Info.Text.Length)
                {
                    CursorPos = Pos;
                }

                if (Info.CursorWidth > 0)
                {
                    var CursorRect = new LGuiRect(CursorPos, new LGuiVec2(Info.CursorWidth * FontWidth, FontHeight));
                    if (LGuiMisc.Overlaps(ref ViewRect, ref CursorRect))
                    {
                        LGuiGraphics.DrawRect(CursorRect, Info.CursorColor, true, false);
                    }
                }
                else if (LGuiMisc.Contains(ref ViewRect, ref CursorPos))
                {
                    LGuiGraphics.DrawLine(CursorPos, CursorPos + new LGuiVec2(0, FontHeight), Info.CursorColor);
                }
            }
        }
    }
}