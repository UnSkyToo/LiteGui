using System.Drawing;
using System.Windows.Forms;
using LiteGui;

namespace LiteGuiDemo
{
    public static class WinConvert
    {
        private static Graphics Device_;

        public static void Register()
        {
            LGuiConvert.GetTextSizeFunc = OnGetTextSize;
            LGuiConvert.GetClipboardTextFunc = OnGetClipboardText;
            LGuiConvert.SetClipboardTextFunc = OnSetClipboardText;
        }

        public static void SetDevice(Graphics Device)
        {
            Device_ = Device;
        }
        
        private static LGuiVec2 OnGetTextSize(string Text, LGuiFont GuiFont)
        {
            var Font = FontCache.GetOrCreate(GuiFont);
            var Size = Device_.MeasureString(Text, Font, 0, GdiCommandExecutor.FontStringFormat);
            return new LGuiVec2(Size.Width, Font.Height);
        }

        private static string OnGetClipboardText()
        {
            return Clipboard.GetText();
        }

        private static void OnSetClipboardText(string Text)
        {
            Clipboard.SetText(Text);
        }
    }
}