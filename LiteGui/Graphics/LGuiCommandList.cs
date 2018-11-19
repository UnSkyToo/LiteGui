using System.Collections.Generic;

namespace LiteGui.Graphics
{
    internal class LGuiCommandList : ILGuiCommandList
    {
        private readonly List<ILGuiCommand> Commands_;

        internal LGuiCommandList()
        {
            Commands_ = new List<ILGuiCommand>();
        }

        internal int GetCommandCount()
        {
            return Commands_.Count;
        }
        
        internal void Clear()
        {
            Commands_.Clear();
        }
        
        internal void AddCommand<T>(T Command) where T : ILGuiCommand
        {
            Commands_.Add(Command);
        }

        internal void AddCommandList(LGuiCommandList List)
        {
            Commands_.AddRange(List.Commands_);
        }

        public void Begin()
        {
            var Cmd = new BeginCommand();
            AddCommand(Cmd);
        }

        public void End()
        {
            var Cmd = new EndCommand();
            AddCommand(Cmd);
        }

        public void SetClipRect(LGuiRect Rect)
        {
            var Cmd = new SetClipRectCommand(Rect);
            AddCommand(Cmd);
        }

        public void DrawLine(LGuiVec2 BeginPos, LGuiVec2 EndPos, LGuiColor Color)
        {
            var Cmd = new DrawLineCommand(BeginPos, EndPos, Color);
            AddCommand(Cmd);
        }

        public void DrawTriangle(LGuiVec2 Vert1, LGuiVec2 Vert2, LGuiVec2 Vert3, LGuiColor Color, bool IsFill)
        {
            var Cmd = new DrawTriangleCommand(Vert1, Vert2, Vert3, Color, IsFill);
            AddCommand(Cmd);
        }

        public void DrawRect(LGuiRect Rect, LGuiColor Color, bool IsFill, bool IsRound)
        {
            var Cmd = new DrawRectCommand(Rect, Color, IsFill, IsRound);
            AddCommand(Cmd);
        }

        public void DrawCircle(LGuiVec2 Center, float Radius, LGuiColor Color, bool IsFill)
        {
            var Cmd = new DrawCircleCommand(Center, Radius, Color, IsFill);
            AddCommand(Cmd);
        }

        public void DrawText(string Text, LGuiVec2 Pos, LGuiColor Color, LGuiFont Font)
        {
            var Cmd = new DrawTextCommand(Text, Pos, Color, Font);
            AddCommand(Cmd);
        }

        public void DrawTexture(int ID, LGuiRect SrcRect, LGuiRect DstRect)
        {
            var Cmd = new DrawTextureCommand(ID, SrcRect, DstRect);
            AddCommand(Cmd);
        }

        public void DrawPrimitive(LGuiRect Rect, LGuiVec2[] Vertices, LGuiColor[] Colors, int[] Indices)
        {
            var Cmd = new DrawPrimitiveCommand(Rect, Vertices, Colors, Indices);
            AddCommand(Cmd);
        }

        internal void ExecuteAll(LGuiCommandExecutor Executor)
        {
            if (Executor == null)
            {
                return;
            }

            foreach (var Cmd in Commands_)
            {
                switch (Cmd)
                {
                    case BeginCommand Entity:
                        Executor.Begin();
                        break;
                    case EndCommand Entity:
                        Executor.End();
                        break;
                    case SetClipRectCommand Entity:
                        Executor.SetClipRect(Entity.Rect);
                        break;
                    case DrawLineCommand Entity:
                        Executor.DrawLine(Entity.BeginPos, Entity.EndPos, Entity.Color);
                        break;
                    case DrawTriangleCommand Entity:
                        Executor.DrawTriangle(Entity.Vert1, Entity.Vert2, Entity.Vert3, Entity.Color, Entity.IsFill);
                        break;
                    case DrawRectCommand Entity:
                        Executor.DrawRect(Entity.Rect, Entity.Color, Entity.IsFill, Entity.IsRound);
                        break;
                    case DrawCircleCommand Entity:
                        Executor.DrawCircle(Entity.Center, Entity.Radius, Entity.Color, Entity.IsFill);
                        break;
                    case DrawTextCommand Entity:
                        Executor.DrawText(Entity.Text, Entity.Pos, Entity.Color, Entity.Font);
                        break;
                    case DrawTextureCommand Entity:
                        Executor.DrawTexture(Entity.ID, Entity.SrcRect, Entity.DstRect);
                        break;
                    case DrawPrimitiveCommand Entity:
                        Executor.DrawPrimitive(Entity.Rect, Entity.Vertices, Entity.Colors, Entity.Indices);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}