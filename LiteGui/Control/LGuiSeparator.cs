using LiteGui.Graphics;

namespace LiteGui.Control
{
    internal static class LGuiSeparator
    {
        internal static void OnProcess(LGuiColor Color)
        {
            var FrameContext = LGuiContext.GetCurrentFrame();
            var Size = new LGuiVec2(FrameContext.Size.X, 1);
            var Rect = LGuiLayout.DoLayout(Size);
            OnProcess(Color, new LGuiVec2(Rect.Left, Rect.CenterY), new LGuiVec2(Rect.Right, Rect.CenterY));
        }

        internal static void OnProcess(LGuiColor Color, LGuiVec2 BeginPos, LGuiVec2 EndPos)
        {
            LGuiGraphics.DrawLine(BeginPos, EndPos, Color);
        }
    }
}