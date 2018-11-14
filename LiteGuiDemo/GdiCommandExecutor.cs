using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using LiteGui;
using LiteGui.Graphics;

namespace LiteGuiDemo
{
    public class GdiCommandExecutor : LGuiCommandExecutor
    {
        public static StringFormat FontStringFormat;
        private static readonly Stack<Region> ClipRegionStack_ = new Stack<Region>();
        private Graphics Device_;

        public GdiCommandExecutor()
        {
            FontStringFormat = StringFormat.GenericTypographic;
            FontStringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
        }
        
        public void SetDevice(Graphics Device)
        {
            Device_ = Device;
        }

        private GraphicsPath GenerateRectPath(int X, int Y, int Width, int Height, int RadiusX, int RadiusY)
        {
            var Path = new GraphicsPath();
            Path.AddArc(X, Y, RadiusX, RadiusY, 180, 90);
            Path.AddArc(X + Width - RadiusX, Y, RadiusX, RadiusY, 270, 90);
            Path.AddArc(X + Width - RadiusX, Y + Height - RadiusY, RadiusX, RadiusY, 0, 90);
            Path.AddArc(X, Y + Height - RadiusY, RadiusX, RadiusY, 90, 90);
            Path.CloseAllFigures();
            return Path;
        }

        private GraphicsPath GenerateTrianglePath(int X1, int Y1, int X2, int Y2, int X3, int Y3)
        {
            var Path = new GraphicsPath();
            Path.AddLine(X1, Y1, X2, Y2);
            Path.AddLine(X2, Y2, X3, Y3);
            Path.AddLine(X3, Y3, X1, Y1);
            Path.CloseAllFigures();
            return Path;
        }

        public override void Begin()
        {
            ClipRegionStack_.Clear();
        }

        public override void End()
        {
            SetClipRect(new LGuiRect(0, 0, 960, 540));
        }
        
        public override void SetClipRect(LGuiRect Rect)
        {
            Device_.Clip = new Region(new Rectangle((int)Rect.X, (int)Rect.Y, (int)Rect.Width, (int)Rect.Height));
        }

        public override void DrawLine(LGuiVec2 BeginPos, LGuiVec2 EndPos, LGuiColor Color)
        {
            Device_.DrawLine(PenCache.GetOrCreate(Color), BeginPos.X, BeginPos.Y, EndPos.X, EndPos.Y);
        }

        public override void DrawTriangle(LGuiVec2 Vert1, LGuiVec2 Vert2, LGuiVec2 Vert3, LGuiColor Color, bool IsFill)
        {
            if (IsFill)
            {
                Device_.FillPath(BrushCache.GetOrCreate(Color), GenerateTrianglePath((int)Vert1.X, (int)Vert1.Y, (int)Vert2.X, (int)Vert2.Y, (int)Vert3.X, (int)Vert3.Y));
            }
            else
            {
                Device_.DrawPath(PenCache.GetOrCreate(Color), GenerateTrianglePath((int)Vert1.X, (int)Vert1.Y, (int)Vert2.X, (int)Vert2.Y, (int)Vert3.X, (int)Vert3.Y));
            }
        }

        public override void DrawRect(LGuiRect Rect, LGuiColor Color, bool IsFill, bool IsRound)
        {
            if (IsRound)
            {
                if (IsFill)
                {
                    Device_.FillPath(BrushCache.GetOrCreate(Color),
                        GenerateRectPath((int) Rect.X, (int) Rect.Y, (int) Rect.Width, (int) Rect.Height, 10, 10));
                }
                else
                {
                    Device_.DrawPath(PenCache.GetOrCreate(Color),
                        GenerateRectPath((int) Rect.X, (int) Rect.Y, (int) Rect.Width, (int) Rect.Height, 10, 10));
                }
            }
            else
            {
                if (IsFill)
                {
                    Device_.FillRectangle(BrushCache.GetOrCreate(Color),
                        new Rectangle((int) Rect.X, (int) Rect.Y, (int) Rect.Width, (int) Rect.Height));
                }
                else
                {
                    Device_.DrawRectangle(PenCache.GetOrCreate(Color),
                        new Rectangle((int) Rect.X, (int) Rect.Y, (int) Rect.Width, (int) Rect.Height));
                }
            }
        }

        public override void DrawCircle(LGuiVec2 Center, float Radius, LGuiColor Color, bool IsFill)
        {
            if (IsFill)
            {
                Device_.FillEllipse(BrushCache.GetOrCreate(Color),
                    new Rectangle((int)(Center.X - Radius), (int)(Center.Y - Radius), (int)(Radius * 2), (int)(Radius * 2)));
            }
            else
            {
                Device_.DrawEllipse(PenCache.GetOrCreate(Color),
                    new Rectangle((int)(Center.X - Radius), (int)(Center.Y - Radius), (int)(Radius * 2), (int)(Radius * 2)));
            }
        }

        public override void DrawText(string Text, LGuiVec2 Pos, LGuiColor Color, LGuiFont Font)
        {
            Device_.DrawString(Text, FontCache.GetOrCreate(Font), BrushCache.GetOrCreate(Color), Pos.X, Pos.Y, FontStringFormat);
            
        }

        public override void DrawTexture(int ID, LGuiRect SrcRect, LGuiRect DstRect)
        {
            var Texture = TextureCache.Get(ID);
            if (Texture != null)
            {
                Device_.DrawImage(Texture,
                    new RectangleF((int)DstRect.X, (int)DstRect.Y, (int)DstRect.Width, (int)DstRect.Height),
                    new RectangleF((int)SrcRect.X, (int)SrcRect.Y,
                        (int)SrcRect.Width == 0 ? Texture.Width : (int)SrcRect.Width,
                        (int)SrcRect.Height == 0 ? Texture.Height : (int)SrcRect.Height),
                    GraphicsUnit.Pixel);
            }
        }

        public override void DrawPrimitive(LGuiRect Rect, LGuiVec2[] Vertices, LGuiColor[] Colors, int[] Indices)
        {
            unsafe
            {
                var Texture = new Bitmap((int)Rect.Width, (int)Rect.Height, PixelFormat.Format32bppArgb);
                var Buffer = Texture.LockBits(new Rectangle(0, 0, Texture.Width, Texture.Height), ImageLockMode.ReadWrite, Texture.PixelFormat);
                var Pointer = (byte*)Buffer.Scan0;

                for (var Index = 0; Index < Indices.Length; Index += 2)
                {
                    var X = (int) Vertices[Indices[Index]].X;
                    var Y = (int) Vertices[Indices[Index]].Y;
                    var Color = ColorCache.GetOrCreate(Colors[Indices[Index + 1]]);
                    
                    var BufferIndex = Y * Buffer.Stride + X * 4;
                    Pointer[BufferIndex + 0] = Color.B;
                    Pointer[BufferIndex + 1] = Color.G;
                    Pointer[BufferIndex + 2] = Color.R;
                    Pointer[BufferIndex + 3] = Color.A;
                }

                Texture.UnlockBits(Buffer);

                Device_.DrawImage(Texture,
                    new RectangleF(Rect.X, Rect.Y, Rect.Width, Rect.Height),
                    new Rectangle(0, 0, Texture.Width, Texture.Height),
                    GraphicsUnit.Pixel);
            }
        }
    }
}