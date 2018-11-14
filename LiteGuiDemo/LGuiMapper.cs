using System.Collections.Generic;
using System.Windows.Forms;
using LiteGui;

namespace LiteGuiDemo
{
    internal class LGuiMapper
    {
        private readonly Dictionary<Keys, LGuiKeys> KeyMapper_ = new Dictionary<Keys, LGuiKeys>();
        private readonly Dictionary<MouseButtons, LGuiMouseButtons> MouseBtnMapper_ = new Dictionary<MouseButtons, LGuiMouseButtons>();

        internal LGuiMapper()
        {
            KeyMapper_.Add(Keys.Back, LGuiKeys.Backspace);
            KeyMapper_.Add(Keys.Tab, LGuiKeys.Tab);
            KeyMapper_.Add(Keys.Enter, LGuiKeys.Enter);
            KeyMapper_.Add(Keys.ShiftKey, LGuiKeys.Shift);
            KeyMapper_.Add(Keys.ControlKey, LGuiKeys.Control);
            KeyMapper_.Add(Keys.Menu, LGuiKeys.Alt);
            KeyMapper_.Add(Keys.CapsLock, LGuiKeys.CapsLock);
            KeyMapper_.Add(Keys.Escape, LGuiKeys.Escape);
            KeyMapper_.Add(Keys.Space, LGuiKeys.Space);
            KeyMapper_.Add(Keys.PageUp, LGuiKeys.PageUp);
            KeyMapper_.Add(Keys.PageDown, LGuiKeys.PageDown);
            KeyMapper_.Add(Keys.End, LGuiKeys.End);
            KeyMapper_.Add(Keys.Home, LGuiKeys.Home);
            KeyMapper_.Add(Keys.Left, LGuiKeys.Left);
            KeyMapper_.Add(Keys.Up, LGuiKeys.Up);
            KeyMapper_.Add(Keys.Right, LGuiKeys.Right);
            KeyMapper_.Add(Keys.Down, LGuiKeys.Down);
            KeyMapper_.Add(Keys.Insert, LGuiKeys.Insert);
            KeyMapper_.Add(Keys.Delete, LGuiKeys.Delete);
            KeyMapper_.Add(Keys.D0, LGuiKeys.D0);
            KeyMapper_.Add(Keys.D1, LGuiKeys.D1);
            KeyMapper_.Add(Keys.D2, LGuiKeys.D2);
            KeyMapper_.Add(Keys.D3, LGuiKeys.D3);
            KeyMapper_.Add(Keys.D4, LGuiKeys.D4);
            KeyMapper_.Add(Keys.D5, LGuiKeys.D5);
            KeyMapper_.Add(Keys.D6, LGuiKeys.D6);
            KeyMapper_.Add(Keys.D7, LGuiKeys.D7);
            KeyMapper_.Add(Keys.D8, LGuiKeys.D8);
            KeyMapper_.Add(Keys.D9, LGuiKeys.D9);
            KeyMapper_.Add(Keys.A, LGuiKeys.A);
            KeyMapper_.Add(Keys.B, LGuiKeys.B);
            KeyMapper_.Add(Keys.C, LGuiKeys.C);
            KeyMapper_.Add(Keys.D, LGuiKeys.D);
            KeyMapper_.Add(Keys.E, LGuiKeys.E);
            KeyMapper_.Add(Keys.F, LGuiKeys.F);
            KeyMapper_.Add(Keys.G, LGuiKeys.G);
            KeyMapper_.Add(Keys.H, LGuiKeys.H);
            KeyMapper_.Add(Keys.I, LGuiKeys.I);
            KeyMapper_.Add(Keys.J, LGuiKeys.J);
            KeyMapper_.Add(Keys.K, LGuiKeys.K);
            KeyMapper_.Add(Keys.L, LGuiKeys.L);
            KeyMapper_.Add(Keys.M, LGuiKeys.M);
            KeyMapper_.Add(Keys.N, LGuiKeys.N);
            KeyMapper_.Add(Keys.O, LGuiKeys.O);
            KeyMapper_.Add(Keys.P, LGuiKeys.P);
            KeyMapper_.Add(Keys.Q, LGuiKeys.Q);
            KeyMapper_.Add(Keys.R, LGuiKeys.R);
            KeyMapper_.Add(Keys.S, LGuiKeys.S);
            KeyMapper_.Add(Keys.T, LGuiKeys.T);
            KeyMapper_.Add(Keys.U, LGuiKeys.U);
            KeyMapper_.Add(Keys.V, LGuiKeys.V);
            KeyMapper_.Add(Keys.W, LGuiKeys.W);
            KeyMapper_.Add(Keys.X, LGuiKeys.X);
            KeyMapper_.Add(Keys.Y, LGuiKeys.Y);
            KeyMapper_.Add(Keys.Z, LGuiKeys.Z);
            KeyMapper_.Add(Keys.NumPad0, LGuiKeys.NumPad0);
            KeyMapper_.Add(Keys.NumPad1, LGuiKeys.NumPad1);
            KeyMapper_.Add(Keys.NumPad2, LGuiKeys.NumPad2);
            KeyMapper_.Add(Keys.NumPad3, LGuiKeys.NumPad3);
            KeyMapper_.Add(Keys.NumPad4, LGuiKeys.NumPad4);
            KeyMapper_.Add(Keys.NumPad5, LGuiKeys.NumPad5);
            KeyMapper_.Add(Keys.NumPad6, LGuiKeys.NumPad6);
            KeyMapper_.Add(Keys.NumPad7, LGuiKeys.NumPad7);
            KeyMapper_.Add(Keys.NumPad8, LGuiKeys.NumPad8);
            KeyMapper_.Add(Keys.NumPad9, LGuiKeys.NumPad9);
            KeyMapper_.Add(Keys.Multiply, LGuiKeys.NumPadMultiply);
            KeyMapper_.Add(Keys.Add, LGuiKeys.NumPadAdd);
            KeyMapper_.Add(Keys.Separator, LGuiKeys.NumPadSeparator);
            KeyMapper_.Add(Keys.Subtract, LGuiKeys.NumPadSubtract);
            KeyMapper_.Add(Keys.Decimal, LGuiKeys.NumPadDecimal);
            KeyMapper_.Add(Keys.Divide, LGuiKeys.NumPadDivide);
            KeyMapper_.Add(Keys.F1, LGuiKeys.F1);
            KeyMapper_.Add(Keys.F2, LGuiKeys.F2);
            KeyMapper_.Add(Keys.F3, LGuiKeys.F3);
            KeyMapper_.Add(Keys.F4, LGuiKeys.F4);
            KeyMapper_.Add(Keys.F5, LGuiKeys.F5);
            KeyMapper_.Add(Keys.F6, LGuiKeys.F6);
            KeyMapper_.Add(Keys.F7, LGuiKeys.F7);
            KeyMapper_.Add(Keys.F8, LGuiKeys.F8);
            KeyMapper_.Add(Keys.F9, LGuiKeys.F9);
            KeyMapper_.Add(Keys.F10, LGuiKeys.F10);
            KeyMapper_.Add(Keys.F11, LGuiKeys.F11);
            KeyMapper_.Add(Keys.F12, LGuiKeys.F12);
            KeyMapper_.Add(Keys.OemSemicolon, LGuiKeys.DSemicolon);
            KeyMapper_.Add(Keys.Oemplus, LGuiKeys.DPlus);
            KeyMapper_.Add(Keys.Oemcomma, LGuiKeys.DComma);
            KeyMapper_.Add(Keys.OemMinus, LGuiKeys.DMinus);
            KeyMapper_.Add(Keys.OemPeriod, LGuiKeys.DPeriod);
            KeyMapper_.Add(Keys.OemQuestion, LGuiKeys.DQuestion);
            KeyMapper_.Add(Keys.Oemtilde, LGuiKeys.DTilde);
            KeyMapper_.Add(Keys.OemOpenBrackets, LGuiKeys.DOpenBrackets);
            KeyMapper_.Add(Keys.OemPipe, LGuiKeys.DPipe);
            KeyMapper_.Add(Keys.OemCloseBrackets, LGuiKeys.DCloseBrackets);
            KeyMapper_.Add(Keys.OemQuotes, LGuiKeys.DQuotes);

            MouseBtnMapper_.Add(MouseButtons.Left, LGuiMouseButtons.Left);
            MouseBtnMapper_.Add(MouseButtons.Right, LGuiMouseButtons.Right);
            MouseBtnMapper_.Add(MouseButtons.Middle, LGuiMouseButtons.Middle);
            MouseBtnMapper_.Add(MouseButtons.XButton1, LGuiMouseButtons.Other);
            MouseBtnMapper_.Add(MouseButtons.XButton2, LGuiMouseButtons.Other);
        }

        internal LGuiKeys ToLGuiKey(Keys Key)
        {
            if (KeyMapper_.ContainsKey(Key))
            {
                return KeyMapper_[Key];
            }

            return LGuiKeys.None;
        }

        internal LGuiMouseButtons ToLGuiMouseBtn(MouseButtons Btn)
        {
            if (MouseBtnMapper_.ContainsKey(Btn))
            {
                return MouseBtnMapper_[Btn];
            }

            return LGuiMouseButtons.None;
        }
    }
}