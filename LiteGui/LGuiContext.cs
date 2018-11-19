using System.Collections.Generic;
using LiteGui.Graphics;

namespace LiteGui
{
    internal class LGuiGroupContext
    {
        internal string Title = string.Empty;
        internal float PreviousCursorX = 0.0f;
        internal float PreviousBeginCursorX = 0.0f;

        internal LGuiGroupContext(string Title, float PreviousCursorX, float PreviousBeginCursorX)
        {
            this.Title = Title;
            this.PreviousCursorX = PreviousCursorX;
            this.PreviousBeginCursorX = PreviousBeginCursorX;
        }
    }

    internal class LGuiFrameContext
    {
        internal string Title = string.Empty;
        internal LGuiRect Rect = LGuiRect.Zero;
        internal LGuiVec2 Size = LGuiVec2.Zero;
        internal bool Visibled = true;

        internal Stack<LGuiGroupContext> GroupStack = new Stack<LGuiGroupContext>();

        internal LGuiFrameContext(string Title, LGuiRect Rect)
        {
            this.Title = Title;
            this.Rect = Rect;
            this.Size = Rect.Size;
            this.Visibled = true;
            
            this.GroupStack = new Stack<LGuiGroupContext>();
        }

        internal void BeginGroup(string Title, float CursorX)
        {
            var LayoutContext = LGuiLayout.GetCurrentLayoutContext();
            GroupStack.Push(new LGuiGroupContext(Title, LayoutContext.CursorPos.X, LayoutContext.BeginCursorPos.X));
            LGuiContext.PushID(LGuiHash.CalculateID(Title));

            LayoutContext.BeginCursorPos.X = CursorX;
            LayoutContext.CursorPos.X = CursorX;
        }

        internal void EndGroup()
        {
            if (GroupStack.Count > 0)
            {
                LGuiContext.PopID();
                var Context = GroupStack.Pop();
                var LayoutContext = LGuiLayout.GetCurrentLayoutContext();

                LayoutContext.BeginCursorPos.X = Context.PreviousBeginCursorX;
                LayoutContext.CursorPos.X = Context.PreviousCursorX;
            }
        }
    }

    internal class LGuiWindowContext
    {
        internal string Title = string.Empty;
        internal int ID = 0;
        internal int Order = 0;
        internal LGuiRect Rect = LGuiRect.Zero;
        internal bool Moveable = true;
        internal bool Focusable = true;
        internal LGuiVec2 MoveOriginPos = LGuiVec2.Zero;
        internal LGuiCommandList DrawList = new LGuiCommandList();

        internal LGuiWindowContext(string Title, int ID, int Order, LGuiRect Rect, bool Moveable, bool Focusable)
        {
            this.Title = Title;
            this.ID = ID;
            this.Order = Order;
            this.Rect = Rect;
            this.Moveable = Moveable;
            this.Focusable = Focusable;
        }
    }

    internal static class LGuiContext
    {
        internal static LGuiIO IO = new LGuiIO();
        internal static LGuiFont Font = LGuiFont.Default;

        internal static Stack<int> IDStack = new Stack<int>();
        internal static Stack<LGuiFrameContext> FrameContextStack = new Stack<LGuiFrameContext>();
        internal static Stack<int> ControlWidthStack = new Stack<int>();

        internal static int FocusID = 0;
        internal static int ActiveID = 0;
        internal static int HoveredID = 0;
        internal static int FrameCount = 0;
        internal static int PreviousControlID = 0;
        internal static LGuiRect ActiveRect = LGuiRect.Zero;

        internal static void Clear()
        {
            LGuiContextCache.Clear();

            IO.Clear();
            Font = LGuiFont.Default;

            FocusID = 0;
            ActiveID = 0;
            HoveredID = 0;
            FrameCount = 0;
            PreviousControlID = 0;
            ActiveRect = LGuiRect.Zero;
        }
        
        internal static void Begin()
        {
            HoveredID = 0;
            PreviousControlID = 0;
            FrameCount++;

            IDStack.Clear();
            ControlWidthStack.Clear();
            FrameContextStack.Clear();

            IO.Begin();
            
            BeginFrame(new LGuiFrameContext(LGuiSettings.DefaultFrameTitle, new LGuiRect(LGuiVec2.Zero, IO.DisplaySize)), false);
        }

        internal static void End()
        {
            EndFrame();

            if (!IO.IsMouseDown(LGuiMouseButtons.Left))
            {
                ActiveID = 0;
            }
            else if (ActiveID == 0)
            {
                ActiveID = -1;
            }

            IO.End();
        }

        internal static void SetPreviousControlID(int ID)
        {
            PreviousControlID = ID;
        }
        
        internal static void PushID(int ID)
        {
            IDStack.Push(ID);
        }

        internal static void PopID()
        {
            if (IDStack.Count > 0)
            {
                IDStack.Pop();
            }
        }

        internal static int GetCurrentID()
        {
            if (IDStack.Count > 0)
            {
                return IDStack.Peek();
            }

            return 0;
        }
        
        internal static LGuiFrameContext GetCurrentFrame()
        {
            return FrameContextStack.Peek();
        }
        
        internal static void BeginFrame(LGuiFrameContext Context, bool IsChild)
        {
            PushID(LGuiHash.CalculateID(Context.Title));
            var ClipRect = FrameContextStack.Count > 0 && IsChild
                ? LGuiMisc.CombineRect(ref GetCurrentFrame().Rect, ref Context.Rect)
                : Context.Rect;
            FrameContextStack.Push(Context);
            LGuiGraphics.SetClipRect(ClipRect);
        }

        internal static void EndFrame()
        {
            if (FrameContextStack.Count > 0)
            {
                PopID();
                FrameContextStack.Pop();

                if (FrameContextStack.Count > 0)
                {
                    LGuiGraphics.SetClipRect(GetCurrentFrame().Rect);
                }
                else
                {
                    LGuiGraphics.SetClipRect(new LGuiRect(LGuiVec2.Zero, IO.DisplaySize));
                }
            }
        }
        
        internal static void BeginGroup(string Title, float CursorX)
        {
            GetCurrentFrame().BeginGroup(Title, CursorX);
        }

        internal static void EndGroup()
        {
            GetCurrentFrame().EndGroup();
        }

        internal static void PushControlWidth(int Width)
        {
            ControlWidthStack.Push(Width);
        }

        internal static void PopControlWidth()
        {
            ControlWidthStack.Pop();
        }
        
        internal static int GetCurrentControlWidth()
        {
            return ControlWidthStack.Count > 0 ? ControlWidthStack.Peek() : 0;
        }
    }

    internal static class LGuiContextCache
    {
        internal static Dictionary<string, LGuiVec2> FrameOffset = new Dictionary<string, LGuiVec2>();
        internal static Dictionary<string, LGuiVec2> FrameContextSize = new Dictionary<string, LGuiVec2>();
        internal static Dictionary<string, bool> GroupExpand = new Dictionary<string, bool>();
        internal static Dictionary<string, LGuiColor> ColorPickerHsv = new Dictionary<string, LGuiColor>();
        internal static Dictionary<string, bool> PopupOpen = new Dictionary<string, bool>();
        internal static Dictionary<string, LGuiVec2> PopupPos = new Dictionary<string, LGuiVec2>();
        internal static Dictionary<string, bool> WindowExpand = new Dictionary<string, bool>();

        internal static void Clear()
        {
            FrameOffset.Clear();
            FrameContextSize.Clear();
            GroupExpand.Clear();
            ColorPickerHsv.Clear();
            PopupOpen.Clear();
            PopupPos.Clear();
            WindowExpand.Clear();
        }

        internal static LGuiVec2 GetFrameOffset(string Title)
        {
            if (FrameOffset.ContainsKey(Title))
            {
                return FrameOffset[Title];
            }

            return LGuiVec2.Zero;
        }

        internal static void SetFrameOffset(string Title, LGuiVec2 Offset)
        {
            if (FrameOffset.ContainsKey(Title))
            {
                FrameOffset[Title] = Offset;
            }
            else
            {
                FrameOffset.Add(Title, Offset);
            }
        }

        internal static LGuiVec2 GetFrameContextSize(string Title)
        {
            if (FrameContextSize.ContainsKey(Title))
            {
                return FrameContextSize[Title];
            }

            return LGuiVec2.Zero;
        }

        internal static void SetFrameContextSize(string Title, LGuiVec2 Size)
        {
            if (FrameContextSize.ContainsKey(Title))
            {
                FrameContextSize[Title] = Size;
            }
            else
            {
                FrameContextSize.Add(Title, Size);
            }
        }

        internal static bool GetGroupExpand(string Title)
        {
            if (GroupExpand.ContainsKey(Title))
            {
                return GroupExpand[Title];
            }

            return false;
        }

        internal static void SetGroupExpand(string Title, bool Expand)
        {
            if (GroupExpand.ContainsKey(Title))
            {
                GroupExpand[Title] = Expand;
            }
            else
            {
                GroupExpand.Add(Title, Expand);
            }
        }

        internal static LGuiColor GetColorPickerHsv(string Title)
        {
            if (ColorPickerHsv.ContainsKey(Title))
            {
                return ColorPickerHsv[Title];
            }

            return LGuiColor.Black;
        }

        internal static void SetColorPickerHsv(string Title, LGuiColor Hsv)
        {
            if (ColorPickerHsv.ContainsKey(Title))
            {
                ColorPickerHsv[Title] = Hsv;
            }
            else
            {
                ColorPickerHsv.Add(Title, Hsv);
            }
        }

        internal static bool GetPopupOpen(string Title)
        {
            if (PopupOpen.ContainsKey(Title))
            {
                return PopupOpen[Title];
            }

            return false;
        }

        internal static void SetPopupOpen(string Title, bool IsOpen)
        {
            if (PopupOpen.ContainsKey(Title))
            {
                PopupOpen[Title] = IsOpen;
            }
            else
            {
                PopupOpen.Add(Title, IsOpen);
            }
        }

        internal static LGuiVec2 GetPopupPos(string Title)
        {
            if (PopupPos.ContainsKey(Title))
            {
                return PopupPos[Title];
            }

            return LGuiVec2.Zero;
        }

        internal static void SetPopupPos(string Title, LGuiVec2 Pos)
        {
            if (PopupPos.ContainsKey(Title))
            {
                PopupPos[Title] = Pos;
            }
            else
            {
                PopupPos.Add(Title, Pos);
            }
        }

        internal static bool GetWindowExpand(string Title)
        {
            if (WindowExpand.ContainsKey(Title))
            {
                return WindowExpand[Title];
            }

            return true;
        }

        internal static void SetWindowExpand(string Title, bool Expand)
        {
            if (WindowExpand.ContainsKey(Title))
            {
                WindowExpand[Title] = Expand;
            }
            else
            {
                WindowExpand.Add(Title, Expand);
            }
        }
    }
}