using System.Collections.Generic;

namespace LiteGui
{
    public enum LGuiLayoutMode : byte
    {
        None,
        Vertical,
        Horizontal,
    }

    internal class LGuiLayoutContext
    {
        internal LGuiLayoutMode LayoutMode = LGuiLayoutMode.None;
        internal LGuiVec2 BeginCursorPos = LGuiVec2.Zero;
        internal LGuiVec2 CursorPos = LGuiVec2.Zero;
        internal LGuiVec2 ChildSize = LGuiVec2.Zero;
        internal LGuiVec2 PreviousPos = LGuiVec2.Zero;
        internal bool AutoMerge = true;

        internal LGuiLayoutContext(LGuiLayoutMode LayoutMode, LGuiVec2 CursorPos, bool AutoMerge)
        {
            this.LayoutMode = LayoutMode;
            this.BeginCursorPos = CursorPos;
            this.CursorPos = CursorPos;
            this.ChildSize = LGuiVec2.Zero;
            this.PreviousPos = CursorPos;
            this.AutoMerge = AutoMerge;
        }
    }
    
    internal static class LGuiLayout
    {
        internal static Stack<LGuiLayoutContext> LayoutContextStack = new Stack<LGuiLayoutContext>();

        internal static void Begin()
        {
            BeginLayout(LGuiLayoutMode.Vertical,
                new LGuiVec2(
                    LGuiStyle.GetValue(LGuiStyleValueIndex.FrameChildSpacingX),
                    LGuiStyle.GetValue(LGuiStyleValueIndex.FrameChildSpacingY)),
                true);
        }

        internal static void End()
        {
            LayoutContextStack.Clear();
        }

        internal static void BeginLayout(LGuiLayoutMode LayoutMode, bool AutoMerge)
        {
            BeginLayout(LayoutMode, GetCurrentLayoutContext().CursorPos, AutoMerge);
        }
        
        internal static void BeginLayout(LGuiLayoutMode LayoutMode, LGuiVec2 CursorPos, bool AutoMerge)
        {
           LayoutContextStack.Push(new LGuiLayoutContext(LayoutMode, CursorPos, AutoMerge));
        }

        internal static void EndLayout()
        {
            if (LayoutContextStack.Count > 1)
            {
                var LayoutContext = LayoutContextStack.Pop();
                if (LayoutContext.AutoMerge)
                {
                    DoLayout(LayoutContext.ChildSize);
                }
            }
        }

        internal static LGuiLayoutContext GetCurrentLayoutContext()
        {
            return LayoutContextStack.Peek();
        }

        internal static LGuiLayoutMode GetCurrentLayoutMode()
        {
            return GetCurrentLayoutContext().LayoutMode;
        }
        
        internal static LGuiRect DoLayout(LGuiVec2 Size)
        {
            var Context = GetCurrentLayoutContext();
            var Rect = new LGuiRect(Context.CursorPos, Size);

            switch (Context.LayoutMode)
            {
                case LGuiLayoutMode.None:
                    break;
                case LGuiLayoutMode.Horizontal:
                    Context.PreviousPos.X = Context.CursorPos.X;
                    Context.PreviousPos.Y = Context.CursorPos.Y + Size.Y;
                    Context.CursorPos.X += (Size.X + LGuiStyle.GetValue(LGuiStyleValueIndex.ControlSpacingX));
                    Context.CursorPos.Y = Context.BeginCursorPos.Y;
                    Context.ChildSize.X = LGuiMisc.Max(Context.ChildSize.X, Context.CursorPos.X - Context.BeginCursorPos.X);
                    Context.ChildSize.Y = LGuiMisc.Max(Context.ChildSize.Y, Context.PreviousPos.Y - Context.BeginCursorPos.Y);
                    break;
                case LGuiLayoutMode.Vertical:
                    Context.PreviousPos.X = Context.CursorPos.X + Size.X;
                    Context.PreviousPos.Y = Context.CursorPos.Y;
                    Context.CursorPos.X = Context.BeginCursorPos.X;
                    Context.CursorPos.Y += (Size.Y + LGuiStyle.GetValue(LGuiStyleValueIndex.ControlSpacingY));
                    Context.ChildSize.X = LGuiMisc.Max(Context.ChildSize.X, Context.PreviousPos.X - Context.BeginCursorPos.X);
                    Context.ChildSize.Y = LGuiMisc.Max(Context.ChildSize.Y, Context.CursorPos.Y - Context.BeginCursorPos.Y);
                    break;
                default:
                    break;
            }
            
            return Rect;
        }

        internal static void SameLine()
        {
            var Context = GetCurrentLayoutContext();
            if (Context.LayoutMode == LGuiLayoutMode.Vertical)
            {
                Context.CursorPos.X = Context.PreviousPos.X + LGuiStyle.GetValue(LGuiStyleValueIndex.ControlSpacingX);
                Context.CursorPos.Y = Context.PreviousPos.Y;
            }
        }

        internal static void SameLine(float CursorX)
        {
            var Context = GetCurrentLayoutContext();
            if (Context.LayoutMode == LGuiLayoutMode.Vertical)
            {
                Context.CursorPos.X = CursorX;
                Context.CursorPos.Y = Context.PreviousPos.Y;
            }
        }

        internal static void NextLine()
        {
            var Context = GetCurrentLayoutContext();
            if (Context.LayoutMode == LGuiLayoutMode.Horizontal)
            {
                Context.CursorPos.X = Context.PreviousPos.X;
                Context.CursorPos.Y = Context.PreviousPos.Y + LGuiStyle.GetValue(LGuiStyleValueIndex.ControlSpacingY);
            }
        }

        internal static void NextLine(float CursorY)
        {
            var Context = GetCurrentLayoutContext();
            if (Context.LayoutMode == LGuiLayoutMode.Horizontal)
            {
                Context.CursorPos.X = Context.PreviousPos.X;
                Context.CursorPos.Y = CursorY;
            }
        }
    }
}