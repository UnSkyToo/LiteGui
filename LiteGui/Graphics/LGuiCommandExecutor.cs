namespace LiteGui.Graphics
{
    public abstract class LGuiCommandExecutor
    {
        public abstract void Begin();
        public abstract void End();
        public abstract void SetClipRect(LGuiRect Rect);
        public abstract void DrawLine(LGuiVec2 BeginPos, LGuiVec2 EndPos, LGuiColor Color);
        public abstract void DrawTriangle(LGuiVec2 Vert1, LGuiVec2 Vert2, LGuiVec2 Vert3, LGuiColor Color, bool IsFill);
        public abstract void DrawRect(LGuiRect Rect, LGuiColor Color, bool IsFill, bool IsRound);
        public abstract void DrawCircle(LGuiVec2 Center, float Radius, LGuiColor Color, bool IsFill);
        public abstract void DrawText(string Text, LGuiVec2 Pos, LGuiColor Color, LGuiFont Font);
        public abstract void DrawTexture(int ID, LGuiRect SrcRect, LGuiRect DstRect);
        public abstract void DrawPrimitive(LGuiRect Rect, LGuiVec2[] Vertices, LGuiColor[] Colors, int[] Indices);
    }
}