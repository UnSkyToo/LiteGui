using LiteGui.Graphics;

namespace LiteGui.Control
{
    internal static class LGuiColorPicker
    {
        internal static bool OnProcess(string Title, LGuiVec2 Size, ref LGuiColor Color)
        {
            var Rect = LGuiLayout.DoLayout(Size);
            return OnProcess(Title, Rect, ref Color);
        }

        internal static bool OnProcess(string Title, LGuiRect Rect, ref LGuiColor Color)
        {
            LGuiFrame.Begin(Title, Rect);

            var ID = LGuiHash.CalculateID(Title);
            LGuiContext.SetPreviousControlID(ID);

            if (!LGuiMisc.CheckVisible(ref Rect))
            {
                return false;
            }

            var Hsv = LGuiContextCache.GetColorPickerHsv(Title);
            var FrameSpacing = LGuiStyle.GetFrameChildSpacing();
            var SpacingY = LGuiStyle.GetValue(LGuiStyleValueIndex.ControlSpacingY);
            var SliderSize = LGuiStyle.GetValue(LGuiStyleValueIndex.SliderSize);
            
            var ColorRect = new LGuiRect(Rect.Pos + FrameSpacing, new LGuiVec2(Rect.Width - FrameSpacing.X * 2.0f, Rect.Height - (SliderSize + SpacingY) * 4.0f - SpacingY * 3.0f - 20));
            RenderColorRect(ColorRect, Hsv.R);
            RenderColorCross(ColorRect, Hsv.G, Hsv.B);

            var IDColor = LGuiHash.CalculateID($"{Title}_ColorSelector");
            LGuiMisc.CheckAndSetContextID(ref ColorRect, IDColor);

            var HsvValueChanged = false;
            if (LGuiContext.ActiveID == IDColor)
            {
                var X = LGuiMisc.Lerp(0.0f, 1.0f, (LGuiContext.IO.MousePos.X - ColorRect.X) / ColorRect.Width);
                var Y = LGuiMisc.Lerp(0.0f, 1.0f, (ColorRect.Height - (LGuiContext.IO.MousePos.Y - ColorRect.Y)) / ColorRect.Height);

                if (X != Hsv.G || Y != Hsv.B)
                {
                    HsvValueChanged = true;
                    Hsv.G = X;
                    Hsv.B = Y;
                }
            }

            var HueRect = new LGuiRect(Rect.X + FrameSpacing.X, ColorRect.Bottom + FrameSpacing.Y, ColorRect.Width, 20);
            RenderHueRect(HueRect);
            RenderHueArrow(HueRect, Hsv.R);

            var IDHue = LGuiHash.CalculateID($"{Title}_HueSelector");
            LGuiMisc.CheckAndSetContextID(ref HueRect, IDHue);

            if (LGuiContext.ActiveID == IDHue)
            {
                var Hue = LGuiMisc.Lerp(0.0f, 1.0f, (LGuiContext.IO.MousePos.X - HueRect.X) / HueRect.Width);

                if (Hue != Hsv.R)
                {
                    HsvValueChanged = true;
                    Hsv.R = Hue;
                }
            }

            HsvValueChanged |= HandleHsvSlider(Title, ref Hsv, new LGuiVec2(HueRect.X, HueRect.Bottom + SpacingY), HueRect.Width / 2.0f);

            if (HsvValueChanged)
            {
                Color = LGuiColor.Hsv2Rgb(Hsv);
                LGuiContextCache.SetColorPickerHsv(Title, Hsv);
            }
            
            var RgbValueChanged = false;
            if (HandleRgbSlider(Title, ref Color, new LGuiVec2(HueRect.X + HueRect.Width / 2.0f + 2.5f, HueRect.Bottom + SpacingY), HueRect.Width / 2.0f))
            {
                RgbValueChanged = true;
                LGuiContextCache.SetColorPickerHsv(Title, LGuiColor.Rgb2Hsv(Color));
            }

            var CurrentTitleWidth = LGuiContext.Font.FontWidth * 8.5f;
            LGuiGraphics.DrawText("Current:", new LGuiVec2(HueRect.X, HueRect.Bottom + SpacingY + (SliderSize + SpacingY) * 3.0f), LGuiStyleColorIndex.Text);
            LGuiGraphics.DrawRect(new LGuiRect(HueRect.X + CurrentTitleWidth, HueRect.Bottom + SpacingY + (SliderSize + SpacingY) * 3.0f, HueRect.Width / 2.0f - CurrentTitleWidth, SliderSize), Color, true, false);

            LGuiFrame.End();
            
            return HsvValueChanged | RgbValueChanged;
        }

        internal static bool HandleRgbSlider(string Title, ref LGuiColor Color, LGuiVec2 Pos, float Length)
        {
            var A = (int)(Color.A * 255.0f);
            var R = (int)(Color.R * 255.0f);
            var G = (int)(Color.G * 255.0f);
            var B = (int)(Color.B * 255.0f);
            var TitleWidth = LGuiContext.Font.FontWidth * 2.5f;
            var SpacingY = LGuiStyle.GetValue(LGuiStyleValueIndex.ControlSpacingY);
            var SliderSize = LGuiStyle.GetValue(LGuiStyleValueIndex.SliderSize);
            var RgbValueChanged = false;

            LGuiGraphics.DrawText("R:", new LGuiVec2(Pos.X, Pos.Y), LGuiStyleColorIndex.Text);
            if (LGuiSlider.OnProcess($"{Title}_RedSlider", ref R, 0, 255, 1, true, true,
                new LGuiRect(Pos.X + TitleWidth, Pos.Y, Length - TitleWidth, SliderSize)))
            {
                RgbValueChanged = true;
            }

            LGuiGraphics.DrawText("G:", new LGuiVec2(Pos.X, Pos.Y + SliderSize + SpacingY), LGuiStyleColorIndex.Text);
            if (LGuiSlider.OnProcess($"{Title}_GreenSlider", ref G, 0, 255, 1, true, true,
                new LGuiRect(Pos.X + TitleWidth, Pos.Y + SliderSize + SpacingY, Length - TitleWidth, SliderSize)))
            {
                RgbValueChanged = true;
            }

            LGuiGraphics.DrawText("B:", new LGuiVec2(Pos.X, Pos.Y + (SliderSize + SpacingY) * 2.0f), LGuiStyleColorIndex.Text);
            if (LGuiSlider.OnProcess($"{Title}_BlueSlider", ref B, 0, 255, 1, true, true,
                new LGuiRect(Pos.X + TitleWidth, Pos.Y + (SliderSize + SpacingY) * 2.0f, Length - TitleWidth, SliderSize)))
            {
                RgbValueChanged = true;
            }

            LGuiGraphics.DrawText("A:", new LGuiVec2(Pos.X, Pos.Y + (SliderSize + SpacingY) * 3.0f), LGuiStyleColorIndex.Text);
            if (LGuiSlider.OnProcess($"{Title}_AlphaSlider", ref A, 0, 255, 1, true, true,
                new LGuiRect(Pos.X + TitleWidth, Pos.Y + (SliderSize + SpacingY) * 3.0f, Length - TitleWidth, SliderSize)))
            {
                RgbValueChanged = true;
            }

            if (RgbValueChanged)
            {
                Color = new LGuiColor(R, G, B, A);
            }

            return RgbValueChanged;
        }

        internal static bool HandleHsvSlider(string Title, ref LGuiColor Hsv, LGuiVec2 Pos, float Length)
        {
            var TitleWidth = LGuiContext.Font.FontWidth * 2.5f;
            var SpacingY = LGuiStyle.GetValue(LGuiStyleValueIndex.ControlSpacingY);
            var SliderSize = LGuiStyle.GetValue(LGuiStyleValueIndex.SliderSize);
            var HsvValueChanged = false;

            LGuiGraphics.DrawText("H:", new LGuiVec2(Pos.X, Pos.Y), LGuiStyleColorIndex.Text);
            if (LGuiSlider.OnProcess($"{Title}_HueSlider", ref Hsv.R, 0.0f, 1.0f, 0.005f, true, true,
                new LGuiRect(Pos.X + TitleWidth, Pos.Y, Length - TitleWidth, SliderSize)))
            {
                HsvValueChanged = true;
            }

            LGuiGraphics.DrawText("S:", new LGuiVec2(Pos.X, Pos.Y + SliderSize + SpacingY), LGuiStyleColorIndex.Text);
            if (LGuiSlider.OnProcess($"{Title}_SaturationSlider", ref Hsv.G, 0.0f, 1.0f, 0.005f, true, true,
                new LGuiRect(Pos.X + TitleWidth, Pos.Y + SliderSize + SpacingY, Length - TitleWidth, SliderSize)))
            {
                HsvValueChanged = true;
            }

            LGuiGraphics.DrawText("V:", new LGuiVec2(Pos.X, Pos.Y + (SliderSize + SpacingY) * 2.0f), LGuiStyleColorIndex.Text);
            if (LGuiSlider.OnProcess($"{Title}_ValueSlider", ref Hsv.B, 0.0f, 1.0f, 0.005f, true, true,
                new LGuiRect(Pos.X + TitleWidth, Pos.Y + (SliderSize + SpacingY) * 2.0f, Length - TitleWidth, SliderSize)))
            {
                HsvValueChanged = true;
            }
            
            return HsvValueChanged;
        }

        internal static void RenderColorRect(LGuiRect Rect, float Hue)
        {
            var Width = (int) Rect.Width;
            var Height = (int) Rect.Height;

            var Vertices = new LGuiVec2[Width * Height];
            var Colors = new LGuiColor[Width * Height];
            var Indices = new int[Width * Height * 2];
            var Index = 0;
            
            for (var Y = 0; Y < Height; ++Y)
            {
                for (var X = 0; X < Width; ++X)
                {
                    Colors[Index] = LGuiColor.Hsv2Rgb(new LGuiColor(Hue, X / (float)(Width - 1), Y / (float)(Height - 1)));
                    Vertices[Index] = new LGuiVec2(X, Height - Y - 1);

                    Indices[Index * 2 + 0] = Index;
                    Indices[Index * 2 + 1] = Index;
                    Index++;
                }
            }

            LGuiGraphics.DrawPrimitive(Rect, Vertices, Colors, Indices);
        }

        internal static void RenderColorCross(LGuiRect ColorRect, float Saturation, float Value)
        {
            var CrossX = ColorRect.X + ColorRect.Width * Saturation;
            var CrossY = ColorRect.Y + ColorRect.Height * (1 - Value);
            LGuiGraphics.DrawLine(new LGuiVec2(CrossX - 5, CrossY), new LGuiVec2(CrossX + 5, CrossY), LGuiColor.White);
            LGuiGraphics.DrawLine(new LGuiVec2(CrossX, CrossY - 5), new LGuiVec2(CrossX, CrossY + 5), LGuiColor.White);
        }

        internal static void RenderHueRect(LGuiRect Rect)
        {
            var Width = (int)Rect.Width;
            var Height = (int)Rect.Height;

            var Vertices = new LGuiVec2[Width * Height];
            var Colors = new LGuiColor[Width * Height];
            var Indices = new int[Width * Height * 2];
            var Index = 0;

            for (var Y = 0; Y < Height; ++Y)
            {
                for (var X = 0; X < Width; ++X)
                {
                    Colors[Index] = LGuiColor.Hsv2Rgb(new LGuiColor(X / (float)(Width - 1), 1, 1));
                    Vertices[Index] = new LGuiVec2(X, Y);

                    Indices[Index * 2 + 0] = Index;
                    Indices[Index * 2 + 1] = Index;
                    Index++;
                }
            }

            LGuiGraphics.DrawPrimitive(Rect, Vertices, Colors, Indices);
        }

        internal static void RenderHueArrow(LGuiRect HueRect, float Hue)
        {
            var ArrowX = HueRect.X + HueRect.Width * Hue;
            var ArrowY1 = HueRect.Y;
            var ArrowY2 = HueRect.Bottom;

            LGuiGraphics.DrawTriangle(
                new LGuiVec2(ArrowX, ArrowY1 + 5),
                new LGuiVec2(ArrowX - 5, ArrowY1),
                new LGuiVec2(ArrowX + 5, ArrowY1),
                LGuiColor.White, true);
            LGuiGraphics.DrawTriangle(
                new LGuiVec2(ArrowX, ArrowY2 - 5),
                new LGuiVec2(ArrowX - 5, ArrowY2),
                new LGuiVec2(ArrowX + 5, ArrowY2),
                LGuiColor.White, true);
        }
    }
}