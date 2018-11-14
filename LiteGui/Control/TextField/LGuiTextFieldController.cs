using System;
using LiteGui.Graphics;

namespace LiteGui.Control.TextField
{
    internal static class LGuiTextFieldController
    {
        internal static LGuiTextFieldState CurrentState = null;

        internal static bool OnProcess(int ID, ref string Text, LGuiRect Rect, uint MaxLength, LGuiInputTextFlags Flags, Func<char, bool> Callback)
        {
            var RenderPos = new LGuiVec2(Rect.X + 3, Rect.Y + 1);

            var Info = new LGuiTextRenderInfo();
            Info.TextColor = LGuiStyle.GetColor(LGuiStyleColorIndex.Text);
            Info.Font = LGuiContext.Font;
            Info.IsHidden = (Flags & LGuiInputTextFlags.Password) == LGuiInputTextFlags.Password;
            Info.HiddenChar = '*';
            Info.CountX = (uint)LGuiMisc.Floor((Rect.Width - 6) / Info.Font.FontWidth);
            Info.CountY = (Flags & LGuiInputTextFlags.Multiline) == LGuiInputTextFlags.Multiline
                ? (uint)LGuiMisc.Floor((Rect.Height - 2) / Info.Font.FontHeight)
                : 1u;

            Info.OnlyShowText = false;
            if ((Flags & LGuiInputTextFlags.OnlyDisplay) == LGuiInputTextFlags.OnlyDisplay)
            {
                Info.Text = Text;
                Info.OnlyShowText = true;

                LGuiTextRenderer.Render(Info, RenderPos);
                return false;
            }

            if (CurrentState == null || CurrentState.ID != ID)
            {
                CurrentState = new LGuiTextFieldState(ID, Text, (Flags & LGuiInputTextFlags.Multiline) != LGuiInputTextFlags.Multiline);
                if ((Flags & LGuiInputTextFlags.AutoSelectAll) == LGuiInputTextFlags.AutoSelectAll)
                {
                    CurrentState.SelectAll();
                }
            }
            HandleKeyEvent(CurrentState, Flags, Callback);
            HandleMouseEvent(CurrentState, Info.Font, RenderPos);

            CurrentState.MaxLength = MaxLength;
            Info.CursorColor = Info.TextColor;
            if ((Flags & LGuiInputTextFlags.InsertMode) == LGuiInputTextFlags.InsertMode)
            {
                CurrentState.InsertMode = true;
                Info.CursorWidth = 1u;
                Info.CursorColor.A = 0.5f;
            }
            else
            {
                Info.CursorWidth = 0u;
            }
            
            var Cursor = (uint) CurrentState.GetCursor();
            if (Cursor < CurrentState.OffsetX)
            {
                CurrentState.OffsetX = Cursor;
            }
            else if (Cursor > CurrentState.OffsetX + Info.CountX)
            {
                CurrentState.OffsetX = Cursor - Info.CountX;
            }

            Info.Text = CurrentState.Text;
            Info.SelectStart = (uint) CurrentState.GetSelectStart();
            Info.SelectEnd = (uint) CurrentState.GetSelectEnd();
            Info.SelectColor = LGuiColor.Blue;
            Info.Spacing = CurrentState.Spacing;

            Info.Cursor = Cursor;
            Info.OffsetX = CurrentState.OffsetX;
            Info.OffsetY = CurrentState.OffsetY;
            
            LGuiTextRenderer.Render(Info, RenderPos);

            if ((Flags & LGuiInputTextFlags.EnterReturnsTrue) == LGuiInputTextFlags.EnterReturnsTrue)
            {
                Text = CurrentState.Text;
                var ValueChanged = LGuiContext.IO.IsKeyPressed(LGuiKeys.Enter);
                if (ValueChanged)
                {
                    LGuiContext.FocusID = 0;
                }
                return ValueChanged;
            }
            else
            {
                var NewText = CurrentState.Text;
                var ValueChanged = NewText != Text;
                Text = NewText;
                return ValueChanged;
            }
        }
        
        private static void HandleKeyEvent(LGuiTextFieldState State, LGuiInputTextFlags Flags, Func<char, bool> Callback)
        {
            var KeyCode = LGuiContext.IO.KeyCode;
            if (KeyCode == LGuiKeys.None)
            {
                return;
            }

            if (!LGuiContext.IO.IsKeyPressed(KeyCode))
            {
                return;
            }

            var CmdKey = LGuiTextFieldCmdKey.None;
            var Ctrl = LGuiContext.IO.IsKeyDown(LGuiKeys.Control);
            var Shift = LGuiContext.IO.IsKeyDown(LGuiKeys.Shift);
            var Alt = LGuiContext.IO.IsKeyDown(LGuiKeys.Alt);

            var ReadOnly = (Flags & LGuiInputTextFlags.Readonly) == LGuiInputTextFlags.Readonly;
            var NoUndoRedo = (Flags & LGuiInputTextFlags.NoUndoRedo) == LGuiInputTextFlags.NoUndoRedo;

            switch (KeyCode)
            {
                case LGuiKeys.Left:
                    CmdKey = LGuiTextFieldCmdKey.Left;
                    break;
                case LGuiKeys.Right:
                    CmdKey = LGuiTextFieldCmdKey.Right;
                    break;
                case LGuiKeys.Up:
                    CmdKey = LGuiTextFieldCmdKey.Up;
                    break;
                case LGuiKeys.Down:
                    CmdKey = LGuiTextFieldCmdKey.Down;
                    break;
                case LGuiKeys.Home:
                    CmdKey = LGuiTextFieldCmdKey.Home;
                    break;
                case LGuiKeys.End:
                    CmdKey = LGuiTextFieldCmdKey.End;
                    break;
                case LGuiKeys.CapsLock:
                    State.CapsLock = !State.CapsLock;
                    break;
                case LGuiKeys.Insert:
                    State.InsertMode = !State.InsertMode;
                    break;
                case LGuiKeys.Backspace:
                    if (!ReadOnly)
                    {
                        CmdKey = LGuiTextFieldCmdKey.Backspace;
                    }
                    break;
                case LGuiKeys.Delete:
                    if (!ReadOnly)
                    {
                        CmdKey = LGuiTextFieldCmdKey.Delete;
                    }
                    break;
                case LGuiKeys.A:
                    if (Ctrl)
                    {
                        State.SelectAll();
                    }
                    else
                    {
                        CmdKey = LGuiTextFieldCmdKey.Character;
                    }
                    break;
                case LGuiKeys.C:
                    if (!ReadOnly)
                    {
                        CmdKey = Ctrl ? LGuiTextFieldCmdKey.Copy : LGuiTextFieldCmdKey.Character;
                    }
                    break;
                case LGuiKeys.V:
                    if (!ReadOnly)
                    {
                        CmdKey = Ctrl ? LGuiTextFieldCmdKey.Paste : LGuiTextFieldCmdKey.Character;
                    }
                    break;
                case LGuiKeys.X:
                    if (!ReadOnly)
                    {
                        CmdKey = Ctrl ? LGuiTextFieldCmdKey.Cut : LGuiTextFieldCmdKey.Character;
                    }
                    break;
                case LGuiKeys.Y:
                    if (!ReadOnly && !NoUndoRedo)
                    {
                        CmdKey = Ctrl ? LGuiTextFieldCmdKey.Redo : LGuiTextFieldCmdKey.Character;
                    }
                    break;
                case LGuiKeys.Z:
                    if (!ReadOnly && !NoUndoRedo)
                    {
                        CmdKey = Ctrl ? LGuiTextFieldCmdKey.Undo : LGuiTextFieldCmdKey.Character;
                    }
                    break;
                case LGuiKeys.Control:
                case LGuiKeys.Shift:
                case LGuiKeys.Alt:
                    break;
                default:
                    if (!ReadOnly)
                    {
                        CmdKey = LGuiTextFieldCmdKey.Character;
                    }
                    break;
            }

            if (CmdKey == LGuiTextFieldCmdKey.None)
            {
                return;
            }

            if (Ctrl)
            {
                CmdKey |= LGuiTextFieldCmdKey.Ctrl;
            }

            if (Shift)
            {
                CmdKey |= LGuiTextFieldCmdKey.Shift;
            }

            if (Alt)
            {
                CmdKey |= LGuiTextFieldCmdKey.Alt;
            }

            var Ch = ParseKeyCharacter(KeyCode, Shift, State.CapsLock);
            var Filter = new LGuiTextFieldInputFilter(Flags, Callback);
            if (!Filter.Parse(ref Ch))
            {
                Ch = (char)0;
            }

            State.OnCmdKey(CmdKey, Ch);
        }

        private static void HandleMouseEvent(LGuiTextFieldState State, LGuiFont Font, LGuiVec2 RenderPos)
        {
            if (LGuiContext.IO.IsMouseClick(LGuiMouseButtons.Left))
            {
                var Pos = LGuiContext.IO.MousePos - RenderPos + new LGuiVec2(State.OffsetX * Font.FontWidth, State.OffsetY * Font.FontHeight);
                if (State.SingleLine)
                {
                    Pos.Y = 0;
                }

                var Cursor = LocateCoord(State, Font, Pos);
                State.OnClick(Cursor);
            }
            else if (LGuiContext.IO.IsMouseDown(LGuiMouseButtons.Left) && LGuiContext.IO.IsMouseMove())
            {
                var Pos = LGuiContext.IO.MousePos - RenderPos + new LGuiVec2(State.OffsetX * Font.FontWidth, State.OffsetY * Font.FontHeight);
                if (State.SingleLine)
                {
                    Pos.Y = 0;
                }

                var Cursor = LocateCoord(State, Font, Pos);
                State.OnDrag(Cursor);
            }
        }

        private static int LayoutRow(LGuiTextFieldState State, LGuiFont Font, int StartIndex, ref LGuiVec2 TextSize)
        {
            var Index = StartIndex;
            var LineWidth = 0.0f;
            TextSize.X = 0;
            TextSize.Y = 0;

            while (Index < State.TextLength)
            {
                var Ch = State.GetCharacter(Index++);

                if (Ch == '\n')
                {
                    TextSize.X = LGuiMisc.Max(TextSize.X, LineWidth);
                    TextSize.Y = TextSize.Y + Font.FontHeight + State.Spacing.Y;
                    LineWidth = 0.0f;
                    break;
                }

                if (Ch == '\r')
                {
                    continue;
                }

                LineWidth = LineWidth + Font.FontWidth + State.Spacing.X;
            }

            if (TextSize.X < LineWidth)
            {
                TextSize.X = LineWidth;
            }

            if (LineWidth > 0 || TextSize.Y == 0.0f)
            {
                TextSize.Y = TextSize.Y + Font.FontHeight + State.Spacing.Y;
            }

            return Index - StartIndex;
        }

        private static int LocateCoord(LGuiTextFieldState State, LGuiFont Font, LGuiVec2 Pos)
        {
            var Length = State.TextLength;
            var Index = 0;
            var BaseY = 0.0f;
            var Size = LGuiVec2.Zero;
            var NumChars = 0;

            while (Index < Length)
            {
                NumChars = LayoutRow(State, Font, Index, ref Size);
                if (NumChars <= 0)
                {
                    return Length;
                }

                if (Index == 0 && Pos.Y < BaseY)
                {
                    return 0;
                }

                if (Pos.Y < BaseY + Size.Y)
                {
                    break;
                }

                Index += NumChars;
                BaseY += Size.Y;
            }

            if (Index >= Length)
            {
                return Length;
            }

            if (Pos.X < 0)
            {
                return Index;
            }

            if (Pos.X < Size.X)
            {
                var PrevX = 0.0f;
                for (var N = 0; N < NumChars; ++N)
                {
                    var Width = Font.FontWidth + State.Spacing.X;
                    if (Pos.X < PrevX + Width)
                    {
                        if (Pos.X < PrevX + Width / 2.0f)
                        {
                            return Index + N;
                        }
                        else
                        {
                            return Index + N + 1;
                        }
                    }

                    PrevX += Width;
                }
            }

            if (State.GetCharacter(Index + NumChars - 1) == '\n')
            {
                return Index + NumChars - 1;
            }

            return Index + NumChars;
        }

        private static char ParseKeyCharacter(LGuiKeys KeyCode, bool Shift, bool CapsLock)
        {
            switch (KeyCode)
            {
                case LGuiKeys.A:
                    return Shift || CapsLock ? 'A' : 'a';
                case LGuiKeys.B:
                    return Shift || CapsLock ? 'B' : 'b';
                case LGuiKeys.C:
                    return Shift || CapsLock ? 'C' : 'c';
                case LGuiKeys.D:
                    return Shift || CapsLock ? 'D' : 'd';
                case LGuiKeys.E:
                    return Shift || CapsLock ? 'E' : 'e';
                case LGuiKeys.F:
                    return Shift || CapsLock ? 'F' : 'f';
                case LGuiKeys.G:
                    return Shift || CapsLock ? 'G' : 'g';
                case LGuiKeys.H:
                    return Shift || CapsLock ? 'H' : 'h';
                case LGuiKeys.I:
                    return Shift || CapsLock ? 'I' : 'i';
                case LGuiKeys.J:
                    return Shift || CapsLock ? 'J' : 'j';
                case LGuiKeys.K:
                    return Shift || CapsLock ? 'K' : 'k';
                case LGuiKeys.L:
                    return Shift || CapsLock ? 'L' : 'l';
                case LGuiKeys.M:
                    return Shift || CapsLock ? 'M' : 'm';
                case LGuiKeys.N:
                    return Shift || CapsLock ? 'N' : 'n';
                case LGuiKeys.O:
                    return Shift || CapsLock ? 'O' : 'o';
                case LGuiKeys.P:
                    return Shift || CapsLock ? 'P' : 'p';
                case LGuiKeys.Q:
                    return Shift || CapsLock ? 'Q' : 'q';
                case LGuiKeys.R:
                    return Shift || CapsLock ? 'R' : 'r';
                case LGuiKeys.S:
                    return Shift || CapsLock ? 'S' : 's';
                case LGuiKeys.T:
                    return Shift || CapsLock ? 'T' : 't';
                case LGuiKeys.U:
                    return Shift || CapsLock ? 'U' : 'u';
                case LGuiKeys.V:
                    return Shift || CapsLock ? 'V' : 'v';
                case LGuiKeys.W:
                    return Shift || CapsLock ? 'W' : 'w';
                case LGuiKeys.X:
                    return Shift || CapsLock ? 'X' : 'x';
                case LGuiKeys.Y:
                    return Shift || CapsLock ? 'Y' : 'y';
                case LGuiKeys.Z:
                    return Shift || CapsLock ? 'Z' : 'z';
                case LGuiKeys.D1:
                    return Shift ? '!' : '1';
                case LGuiKeys.D2:
                    return Shift ? '@' : '2';
                case LGuiKeys.D3:
                    return Shift ? '#' : '3';
                case LGuiKeys.D4:
                    return Shift ? '$' : '4';
                case LGuiKeys.D5:
                    return Shift ? '%' : '5';
                case LGuiKeys.D6:
                    return Shift ? '^' : '6';
                case LGuiKeys.D7:
                    return Shift ? '&' : '7';
                case LGuiKeys.D8:
                    return Shift ? '*' : '8';
                case LGuiKeys.D9:
                    return Shift ? '(' : '9';
                case LGuiKeys.D0:
                    return Shift ? ')' : '0';
                case LGuiKeys.DTilde:
                    return Shift ? '~' : '`';
                case LGuiKeys.DMinus:
                    return Shift ? '_' : '-';
                case LGuiKeys.DPlus:
                    return Shift ? '+' : '=';
                case LGuiKeys.DPipe:
                    return Shift ? '|' : '\\';
                case LGuiKeys.DOpenBrackets:
                    return Shift ? '{' : '[';
                case LGuiKeys.DCloseBrackets:
                    return Shift ? '}' : ']';
                case LGuiKeys.DSemicolon:
                    return Shift ? ':' : ';';
                case LGuiKeys.DQuotes:
                    return Shift ? '"' : '\'';
                case LGuiKeys.DComma:
                    return Shift ? '<' : ',';
                case LGuiKeys.DPeriod:
                    return Shift ? '>' : '.';
                case LGuiKeys.DQuestion:
                    return Shift ? '?' : '/';
                case LGuiKeys.Enter:
                    return '\n';
                case LGuiKeys.Space:
                    return ' ';
                case LGuiKeys.NumPad0:
                    return '0';
                case LGuiKeys.NumPad1:
                    return '1';
                case LGuiKeys.NumPad2:
                    return '2';
                case LGuiKeys.NumPad3:
                    return '3';
                case LGuiKeys.NumPad4:
                    return '4';
                case LGuiKeys.NumPad5:
                    return '5';
                case LGuiKeys.NumPad6:
                    return '6';
                case LGuiKeys.NumPad7:
                    return '7';
                case LGuiKeys.NumPad8:
                    return '8';
                case LGuiKeys.NumPad9:
                    return '9';
            }

            return (char)0;
        }
    }
}