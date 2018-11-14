using System;

namespace LiteGui.Control.TextField
{
    internal class LGuiTextFieldInputFilter
    {
        internal LGuiInputTextFlags Flags;
        internal Func<char, bool> Callback;

        internal LGuiTextFieldInputFilter(LGuiInputTextFlags Flags, Func<char, bool> Callback)
        {
            this.Flags = Flags;
            this.Callback = Callback;
        }

        internal bool Parse(ref char Ch)
        {
            if ((Flags & LGuiInputTextFlags.CharsDecimal) == LGuiInputTextFlags.CharsDecimal)
            {
                if (!(Ch >= '0' && Ch <= '9') && (Ch != '.') && (Ch != '-') && (Ch != '+') && (Ch != '*') && (Ch != '/'))
                {
                    return false;
                }
            }

            if ((Flags & LGuiInputTextFlags.CharsScientific) == LGuiInputTextFlags.CharsScientific)
            {
                if (!(Ch >= '0' && Ch <= '9') && (Ch != '.') && (Ch != '-') && (Ch != '+') && (Ch != '*') && (Ch != '/') && (Ch != 'e') && (Ch != 'E'))
                {
                    return false;
                }
            }

            if ((Flags & LGuiInputTextFlags.CharsHexadecimal) == LGuiInputTextFlags.CharsHexadecimal)
            {
                if (!(Ch >= '0' && Ch <= '9') && !(Ch >= 'a' && Ch <= 'f') && !(Ch >= 'A' && Ch <= 'F'))
                {
                    return false;
                }
            }

            if ((Flags & LGuiInputTextFlags.CharsUppercase) == LGuiInputTextFlags.CharsUppercase)
            {
                if (Ch >= 'a' && Ch <= 'z')
                {
                    Ch = (char)(Ch + 'A' - 'a');
                }
            }

            if ((Flags & LGuiInputTextFlags.CharsNoBlank) == LGuiInputTextFlags.CharsNoBlank)
            {
                if (Ch == ' ' || Ch == '\t')
                {
                    return false;
                }
            }
            
            if ((Flags & LGuiInputTextFlags.CharsCallback) == LGuiInputTextFlags.CharsCallback)
            {
                if (Callback != null)
                {
                    return Callback(Ch);
                }
            }

            return true;
        }
    }
}
