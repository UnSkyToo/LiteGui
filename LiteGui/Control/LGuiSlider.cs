using LiteGui.Graphics;

namespace LiteGui.Control
{
    internal static class LGuiSlider
    {
        internal static bool OnProcess(string Title, ref float Value, float Min, float Max, float Step, bool IsHorizontal, bool ShowValue, float Length)
        {
            var Size = IsHorizontal
                ? new LGuiVec2(Length, LGuiStyle.GetValue(LGuiStyleValueIndex.SliderSize))
                : new LGuiVec2(LGuiStyle.GetValue(LGuiStyleValueIndex.SliderSize), Length);
            var Rect = LGuiLayout.DoLayout(Size);
            return OnProcess(Title, ref Value, Min, Max, Step, IsHorizontal, ShowValue, Rect);
        }

        internal static bool OnProcess(string Title, ref float Value, float Min, float Max, float Step, bool IsHorizontal, bool ShowValue, LGuiRect Rect)
        {
            var ID = LGuiHash.CalculateID(Title);
            LGuiContext.SetPreviousControlID(ID);

            if (!LGuiMisc.CheckVisible(ref Rect))
            {
                return false;
            }

            var NewValue = LGuiMisc.Clamp(Min, Max, Value);
            Step = LGuiMisc.Min(Max - Min, Step);

            var GrabRect = CalculateGrabRect(ref Rect,
                new LGuiVec2(Step / (Max - Min) * Rect.Width, Step / (Max - Min) * Rect.Height),
                LGuiStyle.GetGrabMinSize(),
                (NewValue - Min) / (Max - Min),
                IsHorizontal);

            LGuiMisc.CheckAndSetContextID(ref GrabRect, ID, true);

            var GrabColorIndex = LGuiContext.ActiveID == ID ? LGuiStyleColorIndex.SliderGrabActived :
                LGuiContext.HoveredID == ID ? LGuiStyleColorIndex.SliderGrabHovered : LGuiStyleColorIndex.SliderGrab;

            LGuiGraphics.DrawRect(Rect, LGuiStyleColorIndex.Frame, true);
            LGuiGraphics.DrawRect(Rect, LGuiStyleColorIndex.Border, false);
            LGuiGraphics.DrawRect(GrabRect, GrabColorIndex, true);
            if (ShowValue)
            {
                var Text = $"{NewValue:0.00}";
                var TextSize = LGuiConvert.GetTextSize(Text, LGuiContext.Font);
                LGuiGraphics.DrawText(Text, new LGuiVec2(Rect.X + (Rect.Width - TextSize.X) / 2.0f, Rect.Y + (Rect.Height - TextSize.Y) / 2.0f), LGuiStyleColorIndex.Text);
            }

            LGuiMisc.CheckAndSetContextID(ref Rect, ID);

            if (LGuiContext.ActiveID == ID)
            {
                NewValue = CalculateCurrentValue(ref Rect, ref GrabRect, Min, Max, Step, IsHorizontal);
            }
            else if (!IsHorizontal)
            {
                NewValue = LGuiMisc.DoMouseWheelEvent(ID, Min, Max, NewValue);
            }
            
            if (LGuiMisc.CheckAndSetFocusID(ID))
            {
            }

            var ValueChanged = false;
            if (NewValue != Value)
            {
                ValueChanged = true;
                Value = NewValue;
            }

            return ValueChanged;
        }

        internal static bool OnProcess(string Title, ref int Value, int Min, int Max, int Step, bool IsHorizontal, bool ShowValue, float Length)
        {
            var Size = IsHorizontal
                ? new LGuiVec2(Length, LGuiStyle.GetValue(LGuiStyleValueIndex.SliderSize))
                : new LGuiVec2(LGuiStyle.GetValue(LGuiStyleValueIndex.SliderSize), Length);
            var Rect = LGuiLayout.DoLayout(Size);
            return OnProcess(Title, ref Value, Min, Max, Step, IsHorizontal, ShowValue, Rect);
        }

        internal static bool OnProcess(string Title, ref int Value, int Min, int Max, int Step, bool IsHorizontal, bool ShowValue, LGuiRect Rect)
        {
            var ID = LGuiHash.CalculateID(Title);
            LGuiContext.SetPreviousControlID(ID);

            if (!LGuiMisc.CheckVisible(ref Rect))
            {
                return false;
            }

            var NewValue = LGuiMisc.Clamp(Min, Max, Value);
            Step = LGuiMisc.Min(Max - Min, Step);

            var GrabRect = CalculateGrabRect(ref Rect,
                new LGuiVec2((float)Step / (float)(Max - Min) * Rect.Width, (float)Step / (float)(Max - Min) * Rect.Height),
                LGuiStyle.GetGrabMinSize(),
                (float)(NewValue - Min) / (float)(Max - Min),
                IsHorizontal);

            LGuiMisc.CheckAndSetContextID(ref GrabRect, ID, true);

            var GrabColorIndex = LGuiContext.ActiveID == ID ? LGuiStyleColorIndex.SliderGrabActived :
                LGuiContext.HoveredID == ID ? LGuiStyleColorIndex.SliderGrabHovered : LGuiStyleColorIndex.SliderGrab;

            LGuiGraphics.DrawRect(Rect, LGuiStyleColorIndex.Frame, true);
            LGuiGraphics.DrawRect(Rect, LGuiStyleColorIndex.Border, false);
            LGuiGraphics.DrawRect(GrabRect, GrabColorIndex, true);
            if (ShowValue)
            {
                var Text = NewValue.ToString();
                var TextSize = LGuiConvert.GetTextSize(Text, LGuiContext.Font);
                LGuiGraphics.DrawText(Text, new LGuiVec2(Rect.X + (Rect.Width - TextSize.X) / 2.0f, Rect.Y + (Rect.Height - TextSize.Y) / 2.0f), LGuiStyleColorIndex.Text);
            }

            LGuiMisc.CheckAndSetContextID(ref Rect, ID);

            if (LGuiContext.ActiveID == ID)
            {
                NewValue = CalculateCurrentValue(ref Rect, ref GrabRect, Min, Max, Step, IsHorizontal);
            }
            else if (!IsHorizontal)
            {
                NewValue = LGuiMisc.DoMouseWheelEvent(ID, Min, Max, NewValue);
            }

            if (LGuiMisc.CheckAndSetFocusID(ID))
            {
            }

            var ValueChanged = false;
            if (NewValue != Value)
            {
                ValueChanged = true;
                Value = NewValue;
            }

            return ValueChanged;
        }

        private static LGuiRect CalculateGrabRect(ref LGuiRect Rect, LGuiVec2 GrabSize, LGuiVec2 MinSize, float Percent, bool IsHorizontal)
        {
            var GrabWidth = IsHorizontal ? LGuiMisc.Max(MinSize.X, GrabSize.X) : Rect.Width - 4;
            var GrabHeight = IsHorizontal ? Rect.Height - 4 : LGuiMisc.Max(MinSize.Y, GrabSize.Y);
            var GrabCenterX = IsHorizontal ? LGuiMisc.Lerp(Rect.Left + GrabWidth / 2.0f + 2, Rect.Right - GrabWidth / 2.0f - 2, Percent) : Rect.CenterX;
            var GrabCenterY = IsHorizontal ? Rect.CenterY : LGuiMisc.Lerp(Rect.Top + GrabHeight / 2.0f + 2, Rect.Bottom - GrabHeight / 2.0f - 2, Percent);
            var GrabRect = LGuiRect.CreateWithCenter(GrabCenterX, GrabCenterY, GrabWidth, GrabHeight);
            return GrabRect;
        }

        private static float CalculateCurrentValue(ref LGuiRect Rect, ref LGuiRect GrabRect, float Min, float Max, float Step, bool IsHorizontal)
        {
            var NewValue = LGuiMisc.Lerp(Min, Max, IsHorizontal ?
                (LGuiContext.IO.MousePos.X - (Rect.X + GrabRect.Width / 2.0f + 2)) / (Rect.Width - GrabRect.Width - 4) :
                (LGuiContext.IO.MousePos.Y - (Rect.Y + GrabRect.Height / 2.0f + 2)) / (Rect.Height - GrabRect.Height - 4));

            if (NewValue > Min && NewValue < Max)
            {
                return LGuiMisc.GetStepValue(NewValue, Step);
            }

            return NewValue;
        }

        private static int CalculateCurrentValue(ref LGuiRect Rect, ref LGuiRect GrabRect, int Min, int Max, int Step, bool IsHorizontal)
        {
            var NewValue = LGuiMisc.Lerp(Min, Max, IsHorizontal ?
                (LGuiContext.IO.MousePos.X - (Rect.X + GrabRect.Width / 2.0f + 2)) / (Rect.Width - GrabRect.Width - 4) :
                (LGuiContext.IO.MousePos.Y - (Rect.Y + GrabRect.Height / 2.0f + 2)) / (Rect.Height - GrabRect.Height - 4));

            if (NewValue > Min && NewValue < Max)
            {
                return LGuiMisc.GetStepValue(NewValue, Step);
            }

            return NewValue;
        }
    }
}