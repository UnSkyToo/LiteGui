using System;
using System.Drawing;
using System.Windows.Forms;

namespace LiteGuiDemo
{
    public static class Input
    {
        private static readonly bool[] KeyCurState_ = new bool[256];
        private static readonly bool[] KeyPreState_ = new bool[256];
        private static readonly bool[] MouseCurState_ = new bool[4];
        private static readonly bool[] MousePreState_ = new bool[4];
        private static readonly Point[] MouseClickPos_ = new Point[4];
        private static Point MouseCurDeltaPos_ = Point.Empty;
        private static Point MousePreDeltaPos_ = Point.Empty;
        private static Point MousePos_ = Point.Empty;
        private static int MouseWheel_ = 0;

        public static event Action<Keys, bool> OnKeyEvent;
        public static event Action<MouseButtons, int, int, bool, bool> OnMouseEvent; 

        static Input()
        {
            for (var Index = 0; Index < KeyCurState_.Length; ++Index)
            {
                KeyCurState_[Index] = false;
                KeyPreState_[Index] = false;
            }

            for (var Index = 0; Index < MouseCurState_.Length; ++Index)
            {
                MouseCurState_[Index] = false;
                MousePreState_[Index] = false;
                MouseClickPos_[Index] = Point.Empty;
            }
        }

        public static void Update()
        {
            for (var Index = 0; Index < KeyCurState_.Length; ++Index)
            {
                KeyPreState_[Index] = KeyCurState_[Index];
            }

            for (var Index = 0; Index < MouseCurState_.Length; ++Index)
            {
                MousePreState_[Index] = MouseCurState_[Index];
            }

            MousePreDeltaPos_ = MouseCurDeltaPos_;
            MouseWheel_ = 0;
        }

        private static int KeyToIndex(Keys Key)
        {
            return (int)Key;
        }

        public static void OnKeyDown(Keys Key)
        {
            var Index = KeyToIndex(Key);
            KeyPreState_[Index] = KeyCurState_[Index];
            KeyCurState_[Index] = true;

            OnKeyEvent?.Invoke(Key, true);
        }

        public static void OnKeyUp(Keys Key)
        {
            var Index = KeyToIndex(Key);
            KeyPreState_[Index] = KeyCurState_[Index];
            KeyCurState_[Index] = false;

            OnKeyEvent?.Invoke(Key, false);
        }

        public static bool IsKeyDown(Keys Key)
        {
            var Index = KeyToIndex(Key);
            return KeyCurState_[Index];
        }

        public static bool IsKeyUp(Keys Key)
        {
            var Index = KeyToIndex(Key);
            return !KeyCurState_[Index];
        }

        public static bool IsKeyPressed(Keys Key)
        {
            var Index = KeyToIndex(Key);
            return KeyPreState_[Index] == false && KeyCurState_[Index] == true;
        }

        private static int MouseToIndex(MouseButtons Mouse)
        {
            switch (Mouse)
            {
                case MouseButtons.Left:
                    return 0;
                case MouseButtons.Right:
                    return 1;
                case MouseButtons.Middle:
                    return 2;
                default:
                    return 3;
            }
        }

        public static void OnMouseDown(MouseButtons Btn, int X, int Y)
        {
            var Index = MouseToIndex(Btn);
            MousePreState_[Index] = MouseCurState_[Index];
            MouseCurState_[Index] = true;
            MouseClickPos_[Index] = new Point(X, Y);

            MousePreDeltaPos_ = MouseCurDeltaPos_;
            MouseCurDeltaPos_ = Point.Empty;

            OnMouseEvent?.Invoke(Btn, X, Y, true, false);
        }

        public static void OnMouseUp(MouseButtons Btn, int X, int Y)
        {
            var Index = MouseToIndex(Btn);
            MousePreState_[Index] = MouseCurState_[Index];
            MouseCurState_[Index] = false;
            MouseClickPos_[Index] = Point.Empty;

            MousePreDeltaPos_ = MouseCurDeltaPos_;
            MouseCurDeltaPos_ = Point.Empty;

            OnMouseEvent?.Invoke(Btn, X, Y, false, false);
        }

        public static void OnMouseMove(MouseButtons Btn, int X, int Y)
        {
            var Index = MouseToIndex(Btn);
            if (MouseCurState_[Index])
            {
                MousePreDeltaPos_ = MouseCurDeltaPos_;
                MouseCurDeltaPos_.X = MouseClickPos_[Index].X - X;
                MouseCurDeltaPos_.Y = MouseClickPos_[Index].Y - Y;
            }

            MousePos_.X = X;
            MousePos_.Y = Y;

            OnMouseEvent?.Invoke(Btn, X, Y, MouseCurState_[Index], true);
        }

        public static void OnMouseWheel(int Delta)
        {
            MouseWheel_ = Delta;
        }

        public static bool IsMouseDown(MouseButtons Btn)
        {
            var Index = MouseToIndex(Btn);
            return MouseCurState_[Index];
        }

        public static bool IsMouseUp(MouseButtons Btn)
        {
            var Index = MouseToIndex(Btn);
            return !MouseCurState_[Index];
        }

        public static bool IsMousePressed(MouseButtons Btn)
        {
            var Index = MouseToIndex(Btn);
            return MousePreState_[Index] == false && MouseCurState_[Index] == true;
        }

        public static bool IsMouseMove()
        {
            return Math.Abs((MouseCurDeltaPos_.X - MousePreDeltaPos_.X) + (MouseCurDeltaPos_.Y - MousePreDeltaPos_.Y)) > 0;
        }

        public static Point GetMouseClickPos(MouseButtons Btn)
        {
            var Index = MouseToIndex(Btn);
            return MouseClickPos_[Index];
        }

        public static Point GetMouseMovePos()
        {
            return MouseCurDeltaPos_;
        }

        public static Point GetMouseDelta()
        {
            return new Point(MouseCurDeltaPos_.X - MousePreDeltaPos_.X, MouseCurDeltaPos_.Y - MousePreDeltaPos_.Y);
        }

        public static Point GetMousePos()
        {
            return MousePos_;
        }

        public static int GetMouseWheel()
        {
            return MouseWheel_;
        }
    }
}