namespace LiteGui
{
    internal static class LGuiMisc
    {
        internal static bool Contains(ref LGuiRect Rect, ref LGuiVec2 Value)
        {
            if (Value.X < Rect.Left || Value.X > Rect.Right || Value.Y < Rect.Top || Value.Y > Rect.Bottom)
            {
                return false;
            }

            return true;
        }

        internal static bool Overlaps(ref LGuiRect Left, ref LGuiRect Right)
        {
            if (Left.Left >= Right.Right || Left.Right <= Right.Left || Left.Top >= Right.Bottom || Left.Bottom <= Right.Top)
            {
                return false;
            }

            return true;
        }

        internal static bool IntersectRect(LGuiRect Left, LGuiRect Right, ref LGuiRect Result)
        {
            if (!Overlaps(ref Left, ref Right))
            {
                return false;
            }

            Result = LGuiRect.CreateWithBoundary(
                Max(Left.Left, Right.Left),
                Max(Left.Top, Right.Top),
                Min(Left.Right, Right.Right),
                Min(Left.Bottom, Right.Bottom));
            return true;
        }

        internal static LGuiRect CombineRect(ref LGuiRect Parent, ref LGuiRect Child)
        {
            var Result = LGuiRect.Zero;
            if (!IntersectRect(Parent, Child, ref Result))
            {
                return Parent;
            }

            return Result;
        }

        internal static float Clamp(float Min, float Max, float Value)
        {
            if (Value < Min)
            {
                return Min;
            }

            if (Value > Max)
            {
                return Max;
            }

            return Value;
        }

        internal static float Clamp01(float Value)
        {
            if (Value < 0.0f)
            {
                return 0.0f;
            }

            if (Value > 1.0f)
            {
                return 1.0f;
            }

            return Value;
        }

        internal static int Clamp(int Min, int Max, int Value)
        {
            if (Value < Min)
            {
                return Min;
            }

            if (Value > Max)
            {
                return Max;
            }

            return Value;
        }

        internal static float Lerp(float Min, float Max, float T)
        {
            T = Clamp01(T);
            return Min + (Max - Min) * T;
        }

        internal static int Lerp(int Min, int Max, float T)
        {
            T = Clamp01(T);
            return (int)((float)Min + (float)(Max - Min) * T);
        }

        internal static float LerpUnclamped(float Min, float Max, float T)
        {
            return Min + (Max - Min) * T;
        }

        internal static int LerpUnclamped(int Min, int Max, float T)
        {
            return (int)((float)Min + (float)(Max - Min) * T);
        }

        internal static float Min(float Left, float Right)
        {
            return Left < Right ? Left : Right;
        }

        internal static double Min(double Left, double Right)
        {
            return Left < Right ? Left : Right;
        }

        internal static int Min(int Left, int Right)
        {
            return Left < Right ? Left : Right;
        }

        internal static uint Min(uint Left, uint Right)
        {
            return Left < Right ? Left : Right;
        }
        
        internal static float Max(float Left, float Right)
        {
            return Left > Right ? Left : Right;
        }

        internal static double Max(double Left, double Right)
        {
            return Left > Right ? Left : Right;
        }

        internal static int Max(int Left, int Right)
        {
            return Left > Right ? Left : Right;
        }

        internal static uint Max(uint Left, uint Right)
        {
            return Left > Right ? Left : Right;
        }
        
        internal static float Abs(float Value)
        {
            return System.Math.Abs(Value);
        }

        internal static int Abs(int Value)
        {
            return System.Math.Abs(Value);
        }
        
        internal static float Ceiling(float Value)
        {
            return (float)System.Math.Ceiling(Value);
        }

        internal static float Floor(float Value)
        {
            return (float)System.Math.Floor(Value);
        }

        internal static bool CheckVisible(ref LGuiRect Rect)
        {
            var FrameContext = LGuiContext.GetCurrentFrame();
            if (!FrameContext.Visibled)
            {
                return false;
            }

            return Overlaps(ref FrameContext.Rect, ref Rect);
        }

        internal static void CheckAndSetContextID(ref LGuiRect Rect, int ID, bool OnlyHovered = false)
        {
            var FrameContext = LGuiContext.GetCurrentFrame();
            if (!Contains(ref FrameContext.Rect, ref LGuiContext.IO.MousePos))
            {
                return;
            }

            if (Contains(ref Rect, ref LGuiContext.IO.MousePos))
            {
                LGuiContext.HoveredID = ID;

                if (LGuiContext.ActiveID == 0 && LGuiContext.IO.IsMouseDown(LGuiMouseButtons.Left) && !OnlyHovered)
                {
                    LGuiContext.ActiveID = ID;
                    LGuiContext.ActiveRect = Rect;
                }
            }
        }

        internal static bool CheckAndSetFocusID(int ID)
        {
            // when click other control to clear current focus control (like InputText)
            if (LGuiContext.FocusID == ID && LGuiContext.HoveredID != ID && LGuiContext.IO.IsMouseDown(LGuiMouseButtons.Left))
            {
                LGuiContext.FocusID = 0;
            }

            if (LGuiContext.HoveredID == ID && LGuiContext.ActiveID == ID && !LGuiContext.IO.IsMouseDown(LGuiMouseButtons.Left))
            {
                LGuiContext.FocusID = ID;
                return true;
            }

            return false;
        }

        internal static bool PreviousControlIsHovered()
        {
            return LGuiContext.HoveredID == LGuiContext.PreviousControlID;
        }

        internal static bool PreviousControlIsActive()
        {
            return LGuiContext.ActiveID == LGuiContext.PreviousControlID;
        }
        
        internal static void PreviousControlFocus()
        {
            LGuiContext.FocusID = LGuiContext.PreviousControlID;
        }
        
        internal static float GetStepValue(float Value, float Step)
        {
            return (float)System.Math.Floor(Value / Step) * Step;
        }

        internal static int GetStepValue(int Value, int Step)
        {
            return (Value / Step) * Step;
        }

        internal static float DoMouseWheelEvent(int ID, float Min, float Max, float Value)
        {
            if (LGuiContext.HoveredID == ID)
            {
                var MouseWheel = LGuiContext.IO.MouseWheel;
                if (MouseWheel < 0)
                {
                    Value = LGuiMisc.Min(Value + LGuiStyle.GetValue(LGuiStyleValueIndex.ControlSpacingY) * 5, Max);
                }
                else if (MouseWheel > 0)
                {
                    Value = LGuiMisc.Max(Value - LGuiStyle.GetValue(LGuiStyleValueIndex.ControlSpacingY) * 5, Min);
                }

                LGuiContext.IO.MouseWheel = 0;
            }

            return Value;
        }

        internal static int DoMouseWheelEvent(int ID, int Min, int Max, int Value)
        {
            if (LGuiContext.HoveredID == ID)
            {
                var MouseWheel = LGuiContext.IO.MouseWheel;
                if (MouseWheel < 0)
                {
                    Value = LGuiMisc.Min(Value + (int)(LGuiStyle.GetValue(LGuiStyleValueIndex.ControlSpacingY) * 5), Max);
                }
                else if (MouseWheel > 0)
                {
                    Value = LGuiMisc.Max(Value - (int)(LGuiStyle.GetValue(LGuiStyleValueIndex.ControlSpacingY) * 5), Min);
                }

                LGuiContext.IO.MouseWheel = 0;
            }

            return Value;
        }
    }
}