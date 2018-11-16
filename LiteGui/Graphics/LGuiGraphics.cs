using System.Collections.Generic;

namespace LiteGui.Graphics
{
    internal static class LGuiGraphics
    {
        private static LGuiCommandExecutor CommandExecutor_ = null;
        private static LGuiCommandList CombineCommandList_ = null;
        private static LGuiCommandList[] CommandList_ = null;
        private static Stack<LGuiCommandLevel> CommandLevelStack_ = null;
        private static LGuiCommandLevel CurrentLevel_ = LGuiCommandLevel.Normal;
        
        internal static void SetExecutor(LGuiCommandExecutor Executor)
        {
            CommandExecutor_ = Executor;
        }

        internal static int GetDrawCall()
        {
            return CombineCommandList_?.GetCommandCount() ?? 0;
        }
        
        internal static void SetCurrentLevel(LGuiCommandLevel Level)
        {
            CommandLevelStack_.Push(CurrentLevel_);
            CurrentLevel_ = Level;
        }

        internal static void RestoreCurrentLevel()
        {
            if (CommandLevelStack_.Count > 0)
            {
                CurrentLevel_ = CommandLevelStack_.Pop();
            }
        }

        internal static LGuiCommandList GetCurrentList()
        {
            return CommandList_[(int)CurrentLevel_];
        }
        
        internal static void Begin()
        {
            CommandList_ = new LGuiCommandList[9];
            CommandList_[(int)LGuiCommandLevel.VeryLow] = new LGuiCommandList();
            CommandList_[(int)LGuiCommandLevel.Low] = new LGuiCommandList();
            CommandList_[(int)LGuiCommandLevel.Normal] = new LGuiCommandList();
            CommandList_[(int)LGuiCommandLevel.High] = new LGuiCommandList();
            CommandList_[(int)LGuiCommandLevel.VeryHigh] = new LGuiCommandList();
            CommandList_[(int)LGuiCommandLevel.Window] = new LGuiCommandList();
            CommandList_[(int)LGuiCommandLevel.FocusWindow] = new LGuiCommandList();
            CommandList_[(int)LGuiCommandLevel.Popup] = new LGuiCommandList();
            CommandList_[(int)LGuiCommandLevel.Tips] = new LGuiCommandList();

            CommandLevelStack_ = new Stack<LGuiCommandLevel>();
            CurrentLevel_ = LGuiCommandLevel.Normal;
        }

        internal static void End()
        {
            CombineCommandList_ = new LGuiCommandList();
            CombineCommandList_.Begin();

            CombineCommandList_.AddCommandList(CommandList_[(int)LGuiCommandLevel.VeryLow]);
            CombineCommandList_.AddCommandList(CommandList_[(int)LGuiCommandLevel.Low]);
            CombineCommandList_.AddCommandList(CommandList_[(int)LGuiCommandLevel.Normal]);
            CombineCommandList_.AddCommandList(CommandList_[(int)LGuiCommandLevel.High]);
            CombineCommandList_.AddCommandList(CommandList_[(int)LGuiCommandLevel.VeryHigh]);
            CombineCommandList_.AddCommandList(CommandList_[(int)LGuiCommandLevel.Window]);
            CombineCommandList_.AddCommandList(CommandList_[(int)LGuiCommandLevel.FocusWindow]);
            CombineCommandList_.AddCommandList(CommandList_[(int)LGuiCommandLevel.Popup]);
            CombineCommandList_.AddCommandList(CommandList_[(int)LGuiCommandLevel.Tips]);

            CombineCommandList_.End();
            CombineCommandList_.ExecuteAll(CommandExecutor_);
        }

        internal static ILGuiCommandList CreateCommandList()
        {
            return new LGuiCommandList();
        }

        internal static void AddCommandList(ILGuiCommandList CommandList)
        {
            GetCurrentList().AddCommandList(CommandList as LGuiCommandList);
        }

        internal static void SetClipRect(LGuiRect Rect)
        {
            GetCurrentList().SetClipRect(new LGuiRect(Rect.X + 1, Rect.Y + 1, Rect.Width - 2, Rect.Height - 2));
        }

        internal static void DrawLine(LGuiVec2 BeginPos, LGuiVec2 EndPos, LGuiColor Color)
        {
            GetCurrentList().DrawLine(BeginPos, EndPos, Color);
        }

        internal static void DrawTriangle(LGuiVec2 Vert1, LGuiVec2 Vert2, LGuiVec2 Vert3, LGuiColor Color, bool IsFill)
        {
            GetCurrentList().DrawTriangle(Vert1, Vert2, Vert3, Color, IsFill);
        }

        internal static void DrawRect(LGuiRect Rect, LGuiColor Color, bool IsFill, bool IsRound = true)
        {
            GetCurrentList().DrawRect(Rect, Color, IsFill, IsRound);
        }

        internal static void DrawRect(LGuiRect Rect, LGuiStyleColorIndex ColorIndex, bool IsFill, bool IsRound = true)
        {
            GetCurrentList().DrawRect(Rect, LGuiStyle.GetColor(ColorIndex), IsFill, IsRound);
        }

        internal static void DrawCircle(LGuiVec2 Center, float Radius, LGuiColor Color, bool IsFill)
        {
            GetCurrentList().DrawCircle(Center, Radius, Color, IsFill);
        }

        internal static void DrawCircle(LGuiVec2 Center, float Radius, LGuiStyleColorIndex ColorIndex, bool IsFill)
        {
            GetCurrentList().DrawCircle(Center, Radius, LGuiStyle.GetColor(ColorIndex), IsFill);
        }

        internal static void DrawText(string Text, LGuiVec2 Pos, LGuiColor Color, LGuiFont Font)
        {
            GetCurrentList().DrawText(Text, Pos, Color, Font);
        }

        internal static void DrawText(string Text, LGuiVec2 Pos, LGuiStyleColorIndex ColorIndex)
        {
            GetCurrentList().DrawText(Text, Pos, LGuiStyle.GetColor(ColorIndex), LGuiContext.Font);
        }

        internal static void DrawTexture(int ID, LGuiRect SrcRect, LGuiRect DstRect)
        {
            GetCurrentList().DrawTexture(ID, SrcRect, DstRect);
        }

        internal static void DrawPrimitive(LGuiRect Rect, LGuiVec2[] Vertices, LGuiColor[] Colors, int[] Indices)
        {
            GetCurrentList().DrawPrimitive(Rect, Vertices, Colors, Indices);
        }
    }
}