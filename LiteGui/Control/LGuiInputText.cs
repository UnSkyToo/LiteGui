using LiteGui.Control.TextField;
using LiteGui.Graphics;

namespace LiteGui.Control
{
    internal class LGuiInputText
    {
        internal static bool OnProcess(string Title, ref string Value, uint MaxLength, LGuiInputTextFlags Flags)
        {
            var TextSize = LGuiConvert.GetTextSize(Value, LGuiContext.Font);

            var Size = new LGuiVec2(LGuiStyle.GetValue(LGuiStyleValueIndex.LargeControlLength), TextSize.Y + 2);
            if ((Flags & LGuiInputTextFlags.Multiline) == LGuiInputTextFlags.Multiline)
            {
                Size.Y = LGuiStyle.GetValue(LGuiStyleValueIndex.LargeControlLength);
            }

            var Rect = LGuiLayout.DoLayout(Size);
            return OnProcess(Title, ref Value, Rect, MaxLength, Flags);
        }

        internal static bool OnProcess(string Title, ref string Value, uint MaxLength, LGuiVec2 Size, LGuiInputTextFlags Flags)
        {
            var Rect = LGuiLayout.DoLayout(Size);
            return OnProcess(Title, ref Value, Rect, MaxLength, Flags);
        }

        internal static bool OnProcess(string Title, ref string Value, LGuiRect Rect, uint MaxLength, LGuiInputTextFlags Flags)
        {
            var ID = LGuiHash.CalculateID(Title);
            LGuiContext.SetPreviousControlID(ID);

            if (!LGuiMisc.CheckVisible(ref Rect))
            {
                return false;
            }
            
            LGuiMisc.CheckAndSetContextID(ref Rect, ID);
            
            LGuiGraphics.DrawRect(Rect, LGuiStyleColorIndex.Frame, true);
            LGuiGraphics.DrawRect(Rect, LGuiStyleColorIndex.Border, false);

            if (LGuiMisc.CheckAndSetFocusID(ID))
            {
            }

            var ValueChanged = false;
            if (LGuiContext.FocusID == ID || LGuiContext.ActiveID == ID)
            {
                ValueChanged = LGuiTextFieldController.OnProcess(ID, ref Value, Rect, MaxLength, Flags, null);
            }
            else
            {
                LGuiTextFieldController.OnProcess(ID, ref Value, Rect, MaxLength, Flags | LGuiInputTextFlags.OnlyDisplay, null);
            }

            return ValueChanged;
        }
    }
}