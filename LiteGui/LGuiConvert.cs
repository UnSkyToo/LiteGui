using System;
using System.Collections.Generic;

namespace LiteGui
{
    public static class LGuiConvert
    {
        public static Func<string, LGuiFont, LGuiVec2> GetTextSizeFunc = null;
        private static readonly Dictionary<int, Dictionary<string, LGuiVec2>> TextSizeCache_ = new Dictionary<int, Dictionary<string, LGuiVec2>>();

        public static Func<string> GetClipboardTextFunc = null;
        public static Action<string> SetClipboardTextFunc = null;

        internal static LGuiVec2 GetTextSize(string Text, LGuiFont Font)
        {
            if (string.IsNullOrEmpty(Text))
            {
                Text = " ";
            }

            var Hash = Font.GetHashCode();
            if (!TextSizeCache_.ContainsKey(Hash))
            {
                TextSizeCache_.Add(Hash, new Dictionary<string, LGuiVec2>());
            }

            if (!TextSizeCache_[Hash].ContainsKey(Text))
            {
                TextSizeCache_[Hash].Add(Text, GetTextSizeFunc?.Invoke(Text, Font) ?? LGuiVec2.Zero);
            }

            return TextSizeCache_[Hash][Text];

            // return GetTextSizeFunc?.Invoke(Text, Style) ?? LGuiVec2.Zero;
        }

        internal static string GetClipboardText()
        {
            return GetClipboardTextFunc?.Invoke() ?? string.Empty;
        }

        internal static void SetClipboardText(string Text)
        {
            SetClipboardTextFunc?.Invoke(Text);
        }
    }
}