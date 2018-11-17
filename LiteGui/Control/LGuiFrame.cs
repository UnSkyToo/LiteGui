using LiteGui.Graphics;

namespace LiteGui.Control
{
    internal static class LGuiFrame
    {
        internal static void Begin(string Title, LGuiVec2 Size)
        {
            var Rect = LGuiLayout.DoLayout(Size);
            Begin(Title, Rect, true);
        }
        
        internal static void Begin(string Title, LGuiRect Rect, bool IsChild = true)
        {
            var FullTitle = $"{LGuiContext.GetCurrentFrame().Title}/{Title}";
            var ID = LGuiHash.CalculateID(FullTitle);
            LGuiContext.SetPreviousControlID(ID);

            var Offset = LGuiContextCache.GetFrameOffset(FullTitle);
            var Context = new LGuiFrameContext(FullTitle, Rect);
            Context.Size = LGuiContextCache.GetFrameContextSize(FullTitle);
            
            if (!LGuiMisc.CheckVisible(ref Context.Rect))
            {
                Context.Visibled = false;
            }
            else
            {
                LGuiGraphics.DrawRect(Rect, LGuiStyleColorIndex.Border, false);
            }

            LGuiMisc.CheckAndSetContextID(ref Rect, ID, true);

            LGuiContext.BeginFrame(Context, IsChild);
            LGuiLayout.BeginLayout(LGuiLayout.GetCurrentLayoutMode(),
                new LGuiVec2(
                    Rect.X + LGuiStyle.GetValue(LGuiStyleValueIndex.FrameChildSpacingX) - Offset.X,
                    Rect.Y + LGuiStyle.GetValue(LGuiStyleValueIndex.FrameChildSpacingY) - Offset.Y),
                false);
        }

        internal static void End()
        {
            var FrameContext = LGuiContext.GetCurrentFrame();
            FrameContext.Size = FrameContext.Rect.Size - LGuiStyle.GetFrameChildSpacing() * 2.0f;

            if (FrameContext.Visibled)
            {
                var LayoutContext = LGuiLayout.GetCurrentLayoutContext();
                var Width = LayoutContext.ChildSize.X - FrameContext.Rect.Width + LGuiStyle.GetValue(LGuiStyleValueIndex.FrameChildSpacingX);
                var Height = LayoutContext.ChildSize.Y - FrameContext.Rect.Height + LGuiStyle.GetValue(LGuiStyleValueIndex.FrameChildSpacingY);
                var SliderSize = LGuiStyle.GetValue(LGuiStyleValueIndex.SliderSize);

                var Offset = LGuiContextCache.GetFrameOffset(FrameContext.Title);

                if (Width > 0)
                {
                    LGuiSlider.OnProcess(
                        $"{FrameContext.Title}_hsbar",
                        ref Offset.X,
                        0.0f,
                        Width,
                        LGuiStyle.GetValue(LGuiStyleValueIndex.ControlSpacingX),
                        true,
                        false,
                        new LGuiRect(FrameContext.Rect.X, FrameContext.Rect.Bottom - SliderSize,
                            FrameContext.Rect.Width - SliderSize, SliderSize));
                    FrameContext.Size.Y -= SliderSize;
                }
                else
                {
                    Offset.X = 0;
                }

                if (Height > 0)
                {
                    LGuiSlider.OnProcess(
                        $"{FrameContext.Title}_vsbar",
                        ref Offset.Y,
                        0.0f,
                        Height,
                        LGuiStyle.GetValue(LGuiStyleValueIndex.ControlSpacingY),
                        false,
                        false,
                        new LGuiRect(FrameContext.Rect.Right - SliderSize,
                            FrameContext.Rect.Y, SliderSize, FrameContext.Rect.Height));
                    FrameContext.Size.X -= SliderSize;

                    var ID = LGuiHash.CalculateID(FrameContext.Title);
                    LGuiMisc.CheckAndSetContextID(ref FrameContext.Rect, ID, true);
                    Offset.Y = LGuiMisc.DoMouseWheelEvent(ID, 0.0f, Height, Offset.Y);
                }
                else
                {
                    Offset.Y = 0;
                }

                LGuiContextCache.SetFrameOffset(FrameContext.Title, Offset);
            }

            LGuiContextCache.SetFrameContextSize(FrameContext.Title, FrameContext.Size);
            LGuiLayout.EndLayout();
            LGuiContext.EndFrame();
        }
    }
}