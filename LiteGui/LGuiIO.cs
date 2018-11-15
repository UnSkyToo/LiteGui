using System.Drawing;

namespace LiteGui
{
    public class LGuiIO
    {
        public LGuiVec2 DisplaySize = new LGuiVec2(400, 400);
        public float DeltaTime = 0.0f;
        
        internal bool[] KeyCurState = new bool[256];
        internal bool[] KeyPreState = new bool[256];
        internal float[] KeyPressedTime = new float[256];
        internal LGuiKeys KeyCode = LGuiKeys.None;
        
        internal bool[] MouseCurState = new bool[5];
        internal bool[] MousePreState = new bool[5];
        internal float[] MousePressedTime = new float[5];
        internal LGuiVec2[] MouseClickPos = new LGuiVec2[5];
        internal LGuiVec2 MouseCurDeltaPos = LGuiVec2.Zero;
        internal LGuiVec2 MousePreDeltaPos = LGuiVec2.Zero;
        internal LGuiMouseButtons MouseButton = LGuiMouseButtons.None;
        public LGuiVec2 MousePos = LGuiVec2.Zero;
        public float MouseWheel = 0;
        
        internal LGuiIO()
        {
            Clear();
        }

        internal void Begin()
        {
            for (var Index = 0; Index < KeyPressedTime.Length; ++Index)
            {
                KeyPressedTime[Index] += DeltaTime;
            }

            for (var Index = 0; Index < MousePressedTime.Length; ++Index)
            {
                MousePressedTime[Index] += DeltaTime;
            }
        }

        internal void End()
        {
            for (var Index = 0; Index < KeyCurState.Length; ++Index)
            {
                KeyPreState[Index] = KeyCurState[Index];

                if (!KeyCurState[Index])
                {
                    KeyPressedTime[Index] = 0.0f;
                }
            }

            for (var Index = 0; Index < MouseCurState.Length; ++Index)
            {
                MousePreState[Index] = MouseCurState[Index];

                if (!MouseCurState[Index])
                {
                    MousePressedTime[Index] = 0.0f;
                }
            }

            MousePreDeltaPos = MouseCurDeltaPos;
            MouseWheel = 0;
        }

        internal void Clear()
        {
            for (var Index = 0; Index < KeyCurState.Length; ++Index)
            {
                KeyCurState[Index] = false;
                KeyPreState[Index] = false;
                KeyPressedTime[Index] = 0;
            }

            KeyCode = LGuiKeys.None;

            for (var Index = 0; Index < MouseCurState.Length; ++Index)
            {
                MouseCurState[Index] = false;
                MousePreState[Index] = false;
                MousePressedTime[Index] = 0;
                MouseClickPos[Index] = LGuiVec2.Zero;
            }

            MouseCurDeltaPos = LGuiVec2.Zero;
            MousePreDeltaPos = LGuiVec2.Zero;
            MouseButton = LGuiMouseButtons.None;
            MousePos = LGuiVec2.Zero;
            MouseWheel = 0;
        }

        public void SetKeyState(LGuiKeys Key, bool IsKeyDown)
        {
            var Index = (int)Key;
            KeyPreState[Index] = KeyCurState[Index];
            KeyCurState[Index] = IsKeyDown;
            KeyCode = IsKeyDown ? Key : LGuiKeys.None;
        }

        public bool IsKeyDown(LGuiKeys Key)
        {
            return KeyCurState[(int)Key];
        }

        public bool IsKeyUp(LGuiKeys Key)
        {
            return !KeyCurState[(int)Key];
        }

        public bool IsKeyClick(LGuiKeys Key)
        {
            var Index = (int)Key;
            return !KeyPreState[Index] && KeyCurState[Index];
        }

        public bool IsKeyPressed(LGuiKeys Key)
        {
            var Index = (int)Key;
            return IsKeyClick(Key) || (IsKeyDown(Key) && KeyPressedTime[Index] >= 0.5f);
        }

        public void SetMouseState(LGuiMouseButtons Btn, int X, int Y, bool IsMouseBtnDown, bool IsMouseMove)
        {
            var Index = (int)Btn;
            MouseButton = Btn;

            if (!IsMouseMove)
            {
                MousePreState[Index] = MouseCurState[Index];
                MouseCurState[Index] = IsMouseBtnDown;
                MouseClickPos[Index] = new LGuiVec2(X, Y);

                MousePreDeltaPos = MouseCurDeltaPos;
                MouseCurDeltaPos = LGuiVec2.Zero;
            }
            else
            {
                if (MouseCurState[Index])
                {
                    MousePreDeltaPos = MouseCurDeltaPos;
                    MouseCurDeltaPos.X = X - MouseClickPos[Index].X;
                    MouseCurDeltaPos.Y = Y - MouseClickPos[Index].Y;
                }

                MousePos.X = X;
                MousePos.Y = Y;
            }
        }
        
        public bool IsMouseDown(LGuiMouseButtons Btn)
        {
            return MouseCurState[(int)Btn];
        }

        public bool IsMouseUp(LGuiMouseButtons Btn)
        {
            return !MouseCurState[(int)Btn];
        }
        
        public bool IsMouseClick(LGuiMouseButtons Btn)
        {
            var Index = (int)Btn;
            return !MousePreState[Index] && MouseCurState[Index];
        }

        public bool IsMousePressed(LGuiMouseButtons Btn)
        {
            var Index = (int)Btn;
            return IsMouseClick(Btn) || (IsMouseDown(Btn) && MousePressedTime[Index] >= 0.5f);
        }

        public bool IsMouseMove()
        {
            return LGuiMisc.Abs((MouseCurDeltaPos.X - MousePreDeltaPos.X) + (MouseCurDeltaPos.Y - MousePreDeltaPos.Y)) > 0;
        }

        public LGuiVec2 GetMouseClickPos(LGuiMouseButtons Btn)
        {
            return MouseClickPos[(int)Btn];
        }

        public LGuiVec2 GetMouseMovePos()
        {
            return MouseCurDeltaPos;
        }

        public LGuiVec2 GetMouseDelta()
        {
            return new LGuiVec2(MouseCurDeltaPos.X - MousePreDeltaPos.X, MouseCurDeltaPos.Y - MousePreDeltaPos.Y);
        }
    }
}