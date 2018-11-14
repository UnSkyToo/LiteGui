namespace LiteGui.Graphics
{
    internal class BeginCommand : ILGuiCommand
    {
    }

    internal class EndCommand : ILGuiCommand
    {
    }
    
    internal class SetClipRectCommand : ILGuiCommand
    {
        internal LGuiRect Rect { get; }

        internal SetClipRectCommand(LGuiRect Rect)
        {
            this.Rect = Rect;
        }
    }

    internal class DrawLineCommand : ILGuiCommand
    {
        internal LGuiVec2 BeginPos { get; }
        internal LGuiVec2 EndPos { get; }
        internal LGuiColor Color { get; }

        internal DrawLineCommand(LGuiVec2 BeginPos, LGuiVec2 EndPos, LGuiColor Color)
        {
            this.BeginPos = BeginPos;
            this.EndPos = EndPos;
            this.Color = Color;
        }
    }

    internal class DrawTriangleCommand : ILGuiCommand
    {
        internal LGuiVec2 Vert1 { get; }
        internal LGuiVec2 Vert2 { get; }
        internal LGuiVec2 Vert3 { get; }
        internal LGuiColor Color { get; }
        internal bool IsFill { get; }

        internal DrawTriangleCommand(LGuiVec2 Vert1, LGuiVec2 Vert2, LGuiVec2 Vert3, LGuiColor Color, bool IsFill)
        {
            this.Vert1 = Vert1;
            this.Vert2 = Vert2;
            this.Vert3 = Vert3;
            this.Color = Color;
            this.IsFill = IsFill;
        }
    }

    internal class DrawRectCommand : ILGuiCommand
    {
        internal LGuiRect Rect { get; }
        internal LGuiColor Color { get; }
        internal bool IsFill { get; }
        internal bool IsRound { get; }

        internal DrawRectCommand(LGuiRect Rect, LGuiColor Color, bool IsFill, bool IsRound)
        {
            this.Rect = Rect;
            this.Color = Color;
            this.IsFill = IsFill;
            this.IsRound = IsRound;
        }
    }

    internal class DrawCircleCommand : ILGuiCommand
    {
        internal LGuiVec2 Center { get; }
        internal float Radius { get; }
        internal LGuiColor Color { get; }
        internal bool IsFill { get; }

        internal DrawCircleCommand(LGuiVec2 Center, float Radius, LGuiColor Color, bool IsFill)
        {
            this.Center = Center;
            this.Radius = Radius;
            this.Color = Color;
            this.IsFill = IsFill;
        }
    }

    internal class DrawTextCommand : ILGuiCommand
    {
        internal string Text { get; }
        internal LGuiVec2 Pos { get; }
        internal LGuiColor Color { get; }
        internal LGuiFont Font { get; }

        internal DrawTextCommand(string Text, LGuiVec2 Pos, LGuiColor Color, LGuiFont Font)
        {
            this.Text = Text;
            this.Pos = Pos;
            this.Color = Color;
            this.Font = Font;
        }
    }

    internal class DrawTextureCommand : ILGuiCommand
    {
        internal int ID { get; }
        internal LGuiRect SrcRect { get; }
        internal LGuiRect DstRect { get; }

        internal DrawTextureCommand(int ID, LGuiRect SrcRect, LGuiRect DstRect)
        {
            this.ID = ID;
            this.SrcRect = SrcRect;
            this.DstRect = DstRect;
        }
    }

    internal class DrawPrimitiveCommand : ILGuiCommand
    {
        internal LGuiRect Rect { get; }
        internal LGuiVec2[] Vertices { get; }
        internal LGuiColor[] Colors { get; }
        internal int[] Indices { get; } // vertex,color,vertex,color...

        internal DrawPrimitiveCommand(LGuiRect Rect, LGuiVec2[] Vertices, LGuiColor[] Colors, int[] Indices)
        {
            this.Rect = Rect;
            this.Vertices = Vertices;
            this.Colors = Colors;
            this.Indices = Indices;
        }
    }
}