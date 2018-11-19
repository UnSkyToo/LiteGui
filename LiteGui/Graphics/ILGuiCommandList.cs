namespace LiteGui.Graphics
{
    public interface ILGuiCommandList
    {
        void Begin();
        void End();
        void SetClipRect(LGuiRect Rect);
        void DrawLine(LGuiVec2 BeginPos, LGuiVec2 EndPos, LGuiColor Color);
        void DrawTriangle(LGuiVec2 Vert1, LGuiVec2 Vert2, LGuiVec2 Vert3, LGuiColor Color, bool IsFill);
        void DrawRect(LGuiRect Rect, LGuiColor Color, bool IsFill, bool IsRound);
        void DrawCircle(LGuiVec2 Center, float Radius, LGuiColor Color, bool IsFill);
        void DrawText(string Text, LGuiVec2 Pos, LGuiColor Color, LGuiFont Font);
        void DrawTexture(int ID, LGuiRect SrcRect, LGuiRect DstRect);
        void DrawTexture(string FilePath, LGuiRect SrcRect, LGuiRect DstRect);
        void DrawPrimitive(LGuiRect Rect, LGuiVec2[] Vertices, LGuiColor[] Colors, int[] Indices);
    }
}