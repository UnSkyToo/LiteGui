using System.Collections.Generic;
using LiteGui.Graphics;

namespace LiteGui.Control
{
    internal static class LGuiWindow
    {
        private static readonly Dictionary<int, LGuiWindowContext> WindowList_ = new Dictionary<int, LGuiWindowContext>();
        private static readonly List<int> SortedWindowID_ = new List<int>();

        private static int CurrentWindow_ = 0;
        private static int FocusWindow_ = 0;
        private static int FocusOrder = 1000;

        internal static void Clear()
        {
            WindowList_.Clear();
            SortedWindowID_.Clear();

            CurrentWindow_ = 0;
            FocusWindow_ = 0;
            FocusOrder = 1000;
        }

        internal static void Begin()
        {
            foreach (var Window in WindowList_)
            {
                Window.Value.DrawList.Clear();
            }
        }

        internal static void End()
        {
            LGuiGraphics.SetTargetCommandList(null);

            SortWindowList();
            if (SortedWindowID_.Count > 0)
            {
                SortedWindowID_.Sort((Left, Right) =>
                {
                    if (WindowList_[Left].Order < WindowList_[Right].Order)
                    {
                        return -1;
                    }

                    if (WindowList_[Left].Order > WindowList_[Right].Order)
                    {
                        return 1;
                    }

                    return 0;
                });

                LGuiGraphics.SetCurrentLevel(LGuiCommandLevel.Window);

                foreach (var ID in SortedWindowID_)
                {
                    LGuiGraphics.AddCommandList(WindowList_[ID].DrawList);
                }

                LGuiGraphics.RestoreCurrentLevel();
            }
        }

        internal static void SortWindowList()
        {
            SortedWindowID_.Clear();
            foreach (var Window in WindowList_)
            {
                if (Window.Value.DrawList.GetCommandCount() > 0)
                {
                    SortedWindowID_.Add(Window.Key);
                }
            }
        }
        
        internal static bool CurrentWindowCanHandleMouseMsg(bool NeedContainerMousePos)
        {
            if (CurrentWindow_ == 0)
            {
                return true;
            }

            for (var Index = SortedWindowID_.Count - 1; Index >= 0; --Index)
            {
                var CurrentID = SortedWindowID_[Index];

                if (CurrentID == CurrentWindow_)
                {
                    return !NeedContainerMousePos || LGuiMisc.Contains(ref WindowList_[CurrentID].Rect, ref LGuiContext.IO.MousePos);
                }

                if (LGuiMisc.Contains(ref WindowList_[CurrentID].Rect, ref LGuiContext.IO.MousePos))
                {
                    return false;
                }
            }

            return true;
        }

        internal static bool BeginWindow(string Title, LGuiVec2 Size, LGuiWindowFlags Flags)
        {
            return BeginWindow(Title, new LGuiRect(LGuiLayout.GetCurrentLayoutContext().CursorPos, Size), Flags);
        }

        internal static bool BeginWindow(string Title, LGuiRect InitRect, LGuiWindowFlags Flags)
        {
            if (CurrentWindow_ != 0)
            {
                return false;
            }

            var ID = LGuiHash.Calculate(Title);
            if (!WindowList_.ContainsKey(ID))
            {
                WindowList_.Add(ID, new LGuiWindowContext(Title, ID, WindowList_.Count, InitRect,
                    (Flags & LGuiWindowFlags.NoMove) != LGuiWindowFlags.NoMove,
                    (Flags & LGuiWindowFlags.NoFocus) != LGuiWindowFlags.NoFocus));

                FocusOrder++;
            }

            var Rect = WindowList_[ID].Rect;
            if (!LGuiMisc.CheckVisible(ref Rect))
            {
                return false;
            }

            CurrentWindow_ = ID;
            if (FocusWindow_ == 0)
            {
                FocusWindow_ = CurrentWindow_;
                WindowList_[FocusWindow_].Order = FocusOrder;
            }

            LGuiGraphics.SetTargetCommandList(WindowList_[ID].DrawList);

            if ((Flags & LGuiWindowFlags.NoTitle) != LGuiWindowFlags.NoTitle)
            {
                var TitleRect = new LGuiRect(Rect.Pos, new LGuiVec2(Rect.Width, LGuiContext.Font.FontHeight));
                var NoCollapse = ((Flags & LGuiWindowFlags.NoCollapse) == LGuiWindowFlags.NoCollapse);
                var Expand = LGuiContextCache.GetWindowExpand(Title);

                var TitleTextOffset = NoCollapse ? 5.0f : 20.0f;

                if (Expand)
                {
                    LGuiGraphics.DrawRect(Rect, LGuiStyleColorIndex.Frame, true);
                }

                LGuiGraphics.DrawRect(TitleRect, LGuiStyleColorIndex.HeaderActive, true);
                LGuiGraphics.DrawText(Title, new LGuiVec2(TitleRect.X + TitleTextOffset, TitleRect.Y), LGuiStyleColorIndex.Text);

                if (!NoCollapse)
                {

                    if (Expand)
                    {
                        LGuiGraphics.DrawTriangle(
                            new LGuiVec2(TitleRect.X + 5, TitleRect.Top + 5),
                            new LGuiVec2(TitleRect.X + 15, TitleRect.Top + 5),
                            new LGuiVec2(TitleRect.X + 10, TitleRect.Bottom - 5),
                            LGuiStyle.GetColor(LGuiStyleColorIndex.Text), true);
                    }
                    else
                    {
                        LGuiGraphics.DrawTriangle(
                            new LGuiVec2(TitleRect.X + 5, TitleRect.Top + 5),
                            new LGuiVec2(TitleRect.X + 15, TitleRect.CenterY),
                            new LGuiVec2(TitleRect.X + 5, TitleRect.Bottom - 5),
                            LGuiStyle.GetColor(LGuiStyleColorIndex.Text), true);
                    }

                    var ArrowRect = new LGuiRect(TitleRect.X, TitleRect.Y, 20.0f, 20.0f);

                    LGuiMisc.CheckAndSetContextID(ref ArrowRect, ID);

                    if (LGuiMisc.CheckAndSetFocusID(ID))
                    {
                        Expand = !Expand;
                        LGuiContextCache.SetWindowExpand(Title, Expand);

                    }

                    if (!Expand)
                    {
                        HandleMouseMsg(ref TitleRect);
                        CurrentWindow_ = 0;
                        LGuiGraphics.SetTargetCommandList(null);
                        return false;
                    }
                }
                
                LGuiGraphics.DrawRect(TitleRect, LGuiStyleColorIndex.Border, false);
                var ContextRect = new LGuiRect(Rect.X, TitleRect.Bottom, Rect.Width, Rect.Height - TitleRect.Height);
                LGuiFrame.Begin(Title, ContextRect, false);
            }
            else
            {
                LGuiGraphics.DrawRect(Rect, LGuiStyleColorIndex.Frame, true);
                LGuiFrame.Begin(Title, Rect, false);
            }


            return true;
        }

        internal static void EndWindow()
        {
            LGuiFrame.End();
            LGuiGraphics.SetTargetCommandList(null);
            
            if (!CurrentWindowCanHandleMouseMsg(false))
            {
                CurrentWindow_ = 0;
                return;
            }
            
            HandleMouseMsg(ref WindowList_[CurrentWindow_].Rect);

            CurrentWindow_ = 0;
        }

        private static void HandleMouseMsg(ref LGuiRect Rect)
        {
            if (LGuiContext.IO.IsMouseClick(LGuiMouseButtons.Left) && LGuiMisc.Contains(ref Rect, ref LGuiContext.IO.MousePos))
            {
                if (CurrentWindow_ != FocusWindow_ && WindowList_[CurrentWindow_].Focusable)
                {
                    FocusWindow_ = CurrentWindow_;
                    WindowList_[FocusWindow_].Order = FocusOrder++;
                    SortWindowList();
                }

                WindowList_[CurrentWindow_].MoveOriginPos = Rect.Pos;
            }

            if (WindowList_[FocusWindow_].Moveable)
            {
                LGuiMisc.CheckAndSetContextID(ref Rect, CurrentWindow_);
                if (LGuiContext.ActiveID == CurrentWindow_ && LGuiContext.IO.IsMouseDown(LGuiMouseButtons.Left))
                {
                    var MoveOriginPos = WindowList_[CurrentWindow_].MoveOriginPos;
                    var MovePos = LGuiContext.IO.GetMouseMovePos();
                    WindowList_[CurrentWindow_].Rect.Pos = MoveOriginPos + MovePos;
                }
            }
        }
    }
}