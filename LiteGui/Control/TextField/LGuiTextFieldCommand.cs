namespace LiteGui.Control.TextField
{
    internal interface LGuiITextCommand
    {
        void Execute();
    }

    internal interface LGuiIUndoTextCommand : LGuiITextCommand
    {
        void Undo();
    }

    internal class LGuiInsertCharacterCommand : LGuiIUndoTextCommand
    {
        private LGuiTextFieldState State { get; }
        private int Cursor { get; }
        private char Ch { get; }

        public LGuiInsertCharacterCommand(LGuiTextFieldState State, int Cursor, char Ch)
        {
            this.State = State;
            this.Cursor = Cursor;
            this.Ch = Ch;
        }

        public void Execute()
        {
            State.InsertCharacter(Cursor, Ch);
            State.SetCursor(Cursor + 1);
        }

        public void Undo()
        {
            State.RemoveCharacter(Cursor);
            State.SetCursor(Cursor);
        }
    }

    internal class LGuiRemoveCharacterCommand : LGuiIUndoTextCommand
    {
        private LGuiTextFieldState State { get; }
        private int Cursor { get; }
        private char OldCh;

        public LGuiRemoveCharacterCommand(LGuiTextFieldState State, int Cursor)
        {
            this.State = State;
            this.Cursor = Cursor;
        }

        public void Execute()
        {
            OldCh = State.GetCharacter(Cursor - 1);
            State.RemoveCharacter(Cursor - 1);
            State.SetCursor(Cursor - 1);
        }

        public void Undo()
        {
            State.InsertCharacter(Cursor - 1, OldCh);
            State.SetCursor(Cursor);
        }
    }

    internal class LGuiReplaceCharacterCommand : LGuiIUndoTextCommand
    {
        private LGuiTextFieldState State { get; }
        private int Cursor { get; }
        private char NewCh { get; }
        private char OldCh;

        public LGuiReplaceCharacterCommand(LGuiTextFieldState State, int Cursor, char NewCh)
        {
            this.State = State;
            this.Cursor = Cursor;
            this.NewCh = NewCh;
        }

        public void Execute()
        {
            OldCh = State.GetCharacter(Cursor);
            State.RemoveCharacter(Cursor);
            State.InsertCharacter(Cursor, NewCh);
            State.SetCursor(Cursor + 1);
        }

        public void Undo()
        {
            State.RemoveCharacter(Cursor);
            State.InsertCharacter(Cursor, OldCh);
            State.SetCursor(Cursor);
        }
    }

    internal class LGuiInsertStringCommand : LGuiIUndoTextCommand
    {
        private LGuiTextFieldState State { get; }
        private int Cursor { get; }
        private string Value { get; }

        public LGuiInsertStringCommand(LGuiTextFieldState State, int Cursor, string Value)
        {
            this.State = State;
            this.Cursor = Cursor;
            this.Value = Value;
        }

        public void Execute()
        {
            State.InsertString(Cursor, Value);
            State.SetCursor(Cursor + Value.Length);
        }

        public void Undo()
        {
            State.RemoveString(Cursor, Value.Length);
            State.SetCursor(Cursor);
        }
    }

    internal class LGuiRemoveStringCommand : LGuiIUndoTextCommand
    {
        private LGuiTextFieldState State { get; }
        private int Cursor { get; }
        private int Length { get; }
        private string OldValue;

        public LGuiRemoveStringCommand(LGuiTextFieldState State, int Cursor, int Length)
        {
            this.State = State;
            this.Cursor = Cursor;
            this.Length = Length;
        }

        public void Execute()
        {
            OldValue = State.GetString(Cursor, Length);
            State.RemoveString(Cursor, Length);
            State.SetCursor(Cursor);
        }

        public void Undo()
        {
            State.InsertString(Cursor, OldValue);
            State.SetCursor(Cursor + OldValue.Length);
        }
    }

    internal class LGuiReplaceStringCommand : LGuiIUndoTextCommand
    {
        private LGuiTextFieldState State { get; }
        private int Cursor { get; }
        private int Length { get; }
        private string NewValue { get; }
        private string OldValue;

        public LGuiReplaceStringCommand(LGuiTextFieldState State, int Cursor, int Length, string NewValue)
        {
            this.State = State;
            this.Cursor = Cursor;
            this.Length = Length;
            this.NewValue = NewValue;
        }

        public void Execute()
        {
            OldValue = State.GetString(Cursor, Length);
            State.RemoveString(Cursor, Length);
            State.InsertString(Cursor, NewValue);
            State.SetCursor(Cursor + NewValue.Length);
        }

        public void Undo()
        {
            State.RemoveString(Cursor, NewValue.Length);
            State.InsertString(Cursor, OldValue);
            State.SetCursor(Cursor + OldValue.Length);
        }
    }
}