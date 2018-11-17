using System;

namespace LiteGui
{
    internal static class LGuiSettings
    {
        internal static string DefaultFrameTitle = "##System Frame##";
        internal static string DefaultToolTipsTitle = "##ToolTips##";
        internal static string DefaultTextureTitle = "##Texture##";
    }

    internal enum LGuiCommandLevel : byte
    {
        VeryLow,
        Low,
        Normal,
        High,
        VeryHigh,
        Window,
        FocusWindow,
        Popup,
        Tips,
    }
    
    public enum LGuiKeys : byte
    {
        None            = 0x00,
        Backspace       = 0x08,
        Tab             = 0x09,
        Enter           = 0x0D,
        Shift           = 0x10,
        Control         = 0x11,
        Alt             = 0x12,
        CapsLock        = 0x14,
        Escape          = 0x1B,
        Space           = 0x20,
        PageUp          = 0x21,
        PageDown        = 0x22,
        End             = 0x23,
        Home            = 0x24,
        Left            = 0x25,
        Up              = 0x26,
        Right           = 0x27,
        Down            = 0x28,
        Insert          = 0x2D,
        Delete          = 0x2E,
        D0              = 0x30,
        D1              = 0x31,
        D2              = 0x32,
        D3              = 0x33,
        D4              = 0x34,
        D5              = 0x35,
        D6              = 0x36,
        D7              = 0x37,
        D8              = 0x38,
        D9              = 0x39,
        A               = 0x41,
        B               = 0x42,
        C               = 0x43,
        D               = 0x44,
        E               = 0x45,
        F               = 0x46,
        G               = 0x47,
        H               = 0x48,
        I               = 0x49,
        J               = 0x4A,
        K               = 0x4B,
        L               = 0x4C,
        M               = 0x4D,
        N               = 0x4E,
        O               = 0x4F,
        P               = 0x50,
        Q               = 0x51,
        R               = 0x52,
        S               = 0x53,
        T               = 0x54,
        U               = 0x55,
        V               = 0x56,
        W               = 0x57,
        X               = 0x58,
        Y               = 0x59,
        Z               = 0x5A,
        NumPad0         = 0x60,
        NumPad1         = 0x61,
        NumPad2         = 0x62,
        NumPad3         = 0x63,
        NumPad4         = 0x64,
        NumPad5         = 0x65,
        NumPad6         = 0x66,
        NumPad7         = 0x67,
        NumPad8         = 0x68,
        NumPad9         = 0x69,
        NumPadMultiply  = 0x6A,
        NumPadAdd       = 0x6B,
        NumPadSeparator = 0x6C,
        NumPadSubtract  = 0x6D,
        NumPadDecimal   = 0x6E,
        NumPadDivide    = 0x6F,
        F1              = 0x70,
        F2              = 0x71,
        F3              = 0x72,
        F4              = 0x73,
        F5              = 0x74,
        F6              = 0x75,
        F7              = 0x76,
        F8              = 0x77,
        F9              = 0x78,
        F10             = 0x79,
        F11             = 0x7A,
        F12             = 0x7B,
        DSemicolon      = 0xBA, // ; :
        DPlus           = 0xBB, // - _
        DComma          = 0xBC, // , <
        DMinus          = 0xBD, // + =
        DPeriod         = 0xBE, // . >
        DQuestion       = 0xBF, // / ?
        DTilde          = 0xC0, // ` ~
        DOpenBrackets   = 0xDB, // [ {
        DPipe           = 0xDC, // \ |
        DCloseBrackets  = 0xDD, // ] }
        DQuotes         = 0xDE, // ' "
    }

    public enum LGuiMouseButtons : byte
    {
        None        = 0,
        Left        = 1,
        Right       = 2,
        Middle      = 3,
        Other       = 4,
    }
    
    [Flags]
    public enum LGuiButtonFlags : uint
    {
        None        = 0,
        Repeat      = 1 << 0,
        Invisible   = 1 << 1,
    }

    [Flags]
    public enum LGuiInputTextFlags : uint
    {
        None = 0,

        CharsDecimal = 1 << 0, // 0123456789.+-*/
        CharsScientific = 1 << 1, // 0123456789.+-*/eE
        CharsHexadecimal = 1 << 2, // 0123456789ABCDEFabcdef
        CharsUppercase = 1 << 3, // A..Z
        CharsNoBlank = 1 << 4, // no \n \t
        CharsCallback = 1 << 5, // filter with callback

        AutoSelectAll = 1 << 6, // first foucs select all
        Multiline = 1 << 7,
        Readonly = 1 << 8,
        InsertMode = 1 << 9,
        NoUndoRedo = 1 << 10,
        Password = 1 << 11,

        OnlyDisplay = 1 << 12,
        EnterReturnsTrue = 1 << 13,
    }

    [Flags]
    public enum LGuiWindowFlags : uint
    {
        None = 0,
        NoMove = 1 << 0,
        NoTitle = 1 << 1,
        NoFocus = 1 << 2,
    }

    public struct LGuiVec2
    {
        public float X;
        public float Y;

        public LGuiVec2(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public LGuiVec2 Add(float AddX, float AddY)
        {
            this.X += AddX;
            this.Y += AddY;
            return this;
        }
        
        public override int GetHashCode()
        {
            return LGuiHash.Combine(X.GetHashCode(), Y.GetHashCode());
        }

        public override string ToString()
        {
            return $"<{X}, {Y}>";
        }

        public static LGuiVec2 operator +(LGuiVec2 Left, LGuiVec2 Right)
        {
            return new LGuiVec2(Left.X + Right.X, Left.Y + Right.Y);
        }

        public static LGuiVec2 operator -(LGuiVec2 Left, LGuiVec2 Right)
        {
            return new LGuiVec2(Left.X - Right.X, Left.Y - Right.Y);
        }
        
        public static LGuiVec2 operator *(LGuiVec2 Left, float Right)
        {
            return new LGuiVec2(Left.X * Right, Left.Y * Right);
        }

        public static LGuiVec2 operator *(float Left, LGuiVec2 Right)
        {
            return new LGuiVec2(Left * Right.X, Left * Right.Y);
        }
        
        public static LGuiVec2 operator /(LGuiVec2 Left, float Right)
        {
            return new LGuiVec2(Left.X / Right, Left.Y / Right);
        }

        public static LGuiVec2 operator /(float Left, LGuiVec2 Right)
        {
            return new LGuiVec2(Left / Right.X, Left / Right.X);
        }

        public static readonly LGuiVec2 Zero    = new LGuiVec2(0, 0);
        public static readonly LGuiVec2 One     = new LGuiVec2(1, 1);
        public static readonly LGuiVec2 UnitX   = new LGuiVec2(1, 0);
        public static readonly LGuiVec2 UnitY   = new LGuiVec2(0, 1);
    }

    public struct LGuiRect
    {
        public LGuiVec2 Pos;
        public LGuiVec2 Size;
        public float Left => Pos.X;
        public float Right => Pos.X + Size.X;
        public float Top => Pos.Y;
        public float Bottom => Pos.Y + Size.Y;
        public float X => Pos.X;
        public float Y => Pos.Y;
        public float Width => Size.X;
        public float Height => Size.Y;
        public float CenterX => Pos.X + Size.X / 2.0f;
        public float CenterY => Pos.Y + Size.Y / 2.0f;
        public LGuiVec2 CenterPos => new LGuiVec2(CenterX, CenterY);

        public LGuiRect(LGuiVec2 Pos, LGuiVec2 Size)
        {
            this.Pos = Pos;
            this.Size = Size;
        }

        public LGuiRect(float X, float Y, float Width, float Height)
        {
            this.Pos = new LGuiVec2(X, Y);
            this.Size = new LGuiVec2(Width, Height);
        }

        public override int GetHashCode()
        {
            return LGuiHash.Combine(Pos.GetHashCode(), Size.GetHashCode());
        }

        public override string ToString()
        {
            return $"<{X}, {Y}, {Width}, {Height}>";
        }

        public static LGuiRect CreateWithCenter(LGuiVec2 CenterPos, LGuiVec2 Size)
        {
            return new LGuiRect(CenterPos.X - Size.X / 2.0f, CenterPos.Y - Size.Y / 2.0f, Size.X, Size.Y);
        }

        public static LGuiRect CreateWithCenter(float CenterX, float CenterY, float Width, float Height)
        {
            return new LGuiRect(CenterX - Width / 2.0f, CenterY - Height / 2.0f, Width, Height);
        }

        public static LGuiRect CreateWithBoundary(float Left, float Top, float Right, float Bottom)
        {
            return new LGuiRect(new LGuiVec2(Left, Top), new LGuiVec2(Right - Left, Bottom - Top));
        }

        public static readonly LGuiRect Zero = new LGuiRect(0, 0, 0, 0);
    }

    public struct LGuiColor
    {
        public float R;
        public float G;
        public float B;
        public float A;
        
        public LGuiColor(float R, float G, float B, float A = 1.0f)
        {
            this.R = R;
            this.G = G;
            this.B = B;
            this.A = A;
        }

        public LGuiColor(int R, int G, int B, int A = 255)
            : this(R / 255.0f, G / 255.0f, B / 255.0f, A / 255.0f)
        {
        }

        public override int GetHashCode()
        {
            return LGuiHash.Combine(R.GetHashCode(), G.GetHashCode(), B.GetHashCode(), A.GetHashCode());
        }

        public override string ToString()
        {
            return $"<{R}, {G}, {B}, {A}>";
        }

        /// <summary>
        /// h = r
        /// s = g
        /// v = b
        /// </summary>
        /// <returns></returns>
        public static LGuiColor Rgb2Hsv(LGuiColor Rgb)
        {
            var H = 0.0f;
            var S = 0.0f;
            var V = 0.0f;
            var R = Rgb.R;
            var G = Rgb.G;
            var B = Rgb.B;

            var Max = LGuiMisc.Max(LGuiMisc.Max(R, G), B);
            var Min = LGuiMisc.Min(LGuiMisc.Min(R, G), B);

            if (Max == Min)
            {
                H = 0;
            }
            else if (Max == R)
            {
                if (G >= B)
                {
                    H = (G - B) / (Max - Min) * 60.0f;
                }
                else
                {
                    H = (G - B) / (Max - Min) * 60.0f + 360.0f;
                }
            }
            else if (Max == G)
            {
                H = (B - R) / (Max - Min) * 60.0f + 120.0f;
            }
            else if (Max == B)
            {
                H = (R - G) / (Max - Min) * 60.0f + 240.0f;
            }

            if (Max == 0)
            {
                S = 0;
            }
            else
            {
                S = (Max - Min) / Max;
            }

            V = Max;

            return new LGuiColor(H / 360.0f, S, V, Rgb.A);
        }

        /// <summary>
        /// h = r
        /// s = g
        /// v = b
        /// </summary>
        /// <returns></returns>
        public static LGuiColor Hsv2Rgb(LGuiColor Hsv)
        {
            var H = Hsv.R * 360.0f;
            var S = Hsv.G;
            var V = Hsv.B;
            var A = Hsv.A;

            if (S == 0)
            {
                return new LGuiColor(V, V, V, A);
            }

            H /= 60.0f;
            var I = (int)H;
            var F = H - I;
            var P = V * (1 - S);
            var Q = V * (1 - F * S);
            var T = V * (1 - (1 - F) * S);
            
            switch (I)
            {
                case 0:
                    return new LGuiColor(V, T, P, A);
                case 1:
                    return new LGuiColor(Q, V, P, A);
                case 2:
                    return new LGuiColor(P, V, T, A);
                case 3:
                    return new LGuiColor(P, Q, V, A);
                case 4:
                    return new LGuiColor(T, P, V, A);
                case 5:
                    return new LGuiColor(V, P, Q, A);
                default:
                    return new LGuiColor(V, P, Q, A);
            }
        }
        
        public static readonly LGuiColor White  = new LGuiColor(1.0f, 1.0f, 1.0f);
        public static readonly LGuiColor Black  = new LGuiColor(0.0f, 0.0f, 0.0f);
        public static readonly LGuiColor Gray   = new LGuiColor(0.5f, 0.5f, 0.5f);
        public static readonly LGuiColor Red    = new LGuiColor(1.0f, 0.0f, 0.0f);
        public static readonly LGuiColor Green  = new LGuiColor(0.0f, 1.0f, 0.0f);
        public static readonly LGuiColor Blue   = new LGuiColor(0.0f, 0.0f, 1.0f);
    }

    public class LGuiFont
    {
        public string FontName { get; }
        public int FontSize { get; }
        public bool Bold { get; }

        private float FontWidth_;

        public float FontWidth
        {
            get
            {
                if (FontWidth_ == 0)
                {
                    FontWidth_ = LGuiConvert.GetTextSize(" ", this).X;
                }

                return FontWidth_;
            }
        }

        private float FontHeight_;
        public float FontHeight
        {
            get
            {
                if (FontHeight_ == 0)
                {
                    FontHeight_ = LGuiConvert.GetTextSize(" ", this).Y;
                }

                return FontHeight_;
            }
        }

        public LGuiFont(string FontName)
            : this(FontName, 10, false)
        {
        }

        public LGuiFont(string FontName, int FontSize)
            : this(FontName, FontSize, false)
        {
        }

        public LGuiFont(string FontName, int FontSize, bool Bold)
        {
            this.FontName = FontName;
            this.FontSize = FontSize;
            this.Bold = Bold;
        }

        public override int GetHashCode()
        {
            return LGuiHash.Combine(FontName.GetHashCode(), FontSize.GetHashCode(), Bold.GetHashCode());
        }

        public static readonly LGuiFont Default = new LGuiFont("Consolas", 12, false);
    }
}