using System.Collections.Generic;
using System.Drawing;
using LiteGui;

namespace LiteGuiDemo
{
    internal static class FontCache
    {
        private static readonly Dictionary<int, Font> FontCache_ = new Dictionary<int, Font>();

        internal static Font GetOrCreate(LGuiFont GuiFont)
        {
            var Hash = GuiFont.GetHashCode();

            if (!FontCache_.ContainsKey(Hash))
            {
                FontCache_.Add(Hash, new Font(GuiFont.FontName, GuiFont.FontSize, GuiFont.Bold ? FontStyle.Bold : FontStyle.Regular));
            }

            return FontCache_[Hash];
        }
    }

    internal static class ColorCache
    {
        private static readonly Dictionary<int, Color> ColorCache_ = new Dictionary<int, Color>();

        internal static Color GetOrCreate(LGuiColor GuiColor)
        {
            return Color.FromArgb(
                (int) (GuiColor.A * 255),
                (int) (GuiColor.R * 255),
                (int) (GuiColor.G * 255),
                (int) (GuiColor.B * 255));

            // Hash Conflict
            var Hash = GuiColor.GetHashCode();

            if (!ColorCache_.ContainsKey(Hash))
            {
                ColorCache_.Add(Hash,
                    Color.FromArgb(
                        (int)(GuiColor.A * 255),
                        (int)(GuiColor.R * 255),
                        (int)(GuiColor.G * 255),
                        (int)(GuiColor.B * 255)));
            }

            return ColorCache_[Hash];
        }
    }

    internal static class PenCache
    {
        private static readonly Dictionary<int, Pen> PenCache_ = new Dictionary<int, Pen>();

        internal static Pen GetOrCreate(LGuiColor GuiColor)
        {
            var Hash = GuiColor.GetHashCode();

            if (!PenCache_.ContainsKey(Hash))
            {
                PenCache_.Add(Hash, new Pen(ColorCache.GetOrCreate(GuiColor)));
            }

            return PenCache_[Hash];
        }
    }

    internal static class BrushCache
    {
        private static readonly Dictionary<int, Brush> BrushCache_ = new Dictionary<int, Brush>();

        internal static Brush GetOrCreate(LGuiColor GuiColor)
        {
            var Hash = GuiColor.GetHashCode();

            if (!BrushCache_.ContainsKey(Hash))
            {
                BrushCache_.Add(Hash, new SolidBrush(ColorCache.GetOrCreate(GuiColor)));
            }

            return BrushCache_[Hash];
        }
    }

    internal static class TextureCache
    {
        private static int ID_ = 0;
        private static readonly Dictionary<int, Bitmap> TextureCache_ = new Dictionary<int, Bitmap>();

        internal static Bitmap Get(int ID)
        {
            if (TextureCache_.ContainsKey(ID))
            {
                return TextureCache_[ID];
            }

            return null;
        }

        internal static Bitmap Get(string FilePath)
        {
            var FullPath = FileHelper.GetFileFullPath(FilePath);
            var ID = FullPath.GetHashCode();
            if (!TextureCache_.ContainsKey(ID))
            {
                var Texture = new Bitmap(FullPath);
                TextureCache_.Add(ID, Texture);
            }
            return TextureCache_[ID];
        }

        internal static int Add(string FilePath)
        {
            var FullPath = FileHelper.GetFileFullPath(FilePath);
            var Texture = new Bitmap(FullPath);
            TextureCache_.Add(++ID_, Texture);
            return ID_;
        }
        
        internal static void Remove(int ID)
        {
            TextureCache_.Remove(ID);
        }

        internal static LGuiVec2 GetSize(int ID)
        {
            var Texture = Get(ID);

            if (Texture != null)
            {
                return new LGuiVec2(Texture.Width, Texture.Height);
            }

            return LGuiVec2.Zero;
        }
    }
}