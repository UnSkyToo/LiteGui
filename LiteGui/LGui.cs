using LiteGui.Control;
using LiteGui.Graphics;

namespace LiteGui
{
    public static class LGui
    {
        public static void SetCommandExecutor(LGuiCommandExecutor Executor)
        {
            LGuiGraphics.SetExecutor(Executor);
        }
        
        public static LGuiIO GetIO()
        {
            return LGuiContext.IO;
        }
        
        public static LGuiVec2 GetCursorPos()
        {
            return LGuiLayout.GetCurrentLayoutContext().CursorPos;
        }

        public static int GetDrawCall()
        {
            return LGuiGraphics.GetDrawCall();
        }

        public static ILGuiCommandList CreateCommandList()
        {
            return LGuiGraphics.CreateCommandList();
        }

        public static void AddCommandList(ILGuiCommandList CommandList)
        {
            LGuiGraphics.AddCommandList(CommandList);
        }

        public static void Begin()
        {
            LGuiGraphics.Begin();
            LGuiContext.Begin();
            LGuiLayout.Begin();
            LGuiWindow.Begin();

            LGuiGraphics.DrawRect(LGuiContext.GetCurrentFrame().Rect, new LGuiColor(0.06f, 0.06f, 0.06f, 0.94f), true, false);
        }

        public static void End()
        {
            LGuiWindow.End();
            LGuiLayout.End();
            LGuiContext.End();
            LGuiGraphics.End();
        }

        public static void ClearContext()
        {
            LGuiContext.Clear();
            LGuiWindow.Clear();
        }

        public static void PushID(int ID)
        {
            LGuiContext.PushID(ID);
        }

        public static void PopID()
        {
            LGuiContext.PopID();
        }
        
        public static void BeginFrame(string Title, LGuiVec2 Size)
        {
            LGuiFrame.Begin(Title, Size);
        }

        public static void BeginFrame(string Title, LGuiRect Rect)
        {
            LGuiFrame.Begin(Title, Rect);
        }

        public static void EndFrame()
        {
            LGuiFrame.End();
        }

        public static void BeginLayout(LGuiLayoutMode LayoutMode)
        {
            LGuiLayout.BeginLayout(LayoutMode, true);
        }

        public static void EndLayout()
        {
            LGuiLayout.EndLayout();
        }

        public static void SameLine()
        {
            LGuiLayout.SameLine();
        }

        public static void SameLine(float CursorX)
        {
            LGuiLayout.SameLine(CursorX);
        }

        public static void NextLine()
        {
            LGuiLayout.NextLine();
        }

        public static void NextLine(float CursorY)
        {
            LGuiLayout.NextLine(CursorY);
        }
        
        public static bool BeginGroup(string Title)
        {
            return LGuiGroup.Begin(Title);
        }

        public static void EndGroup()
        {
            LGuiGroup.End();
        }

        public static bool BeginPopup(string Title, LGuiVec2 Size)
        {
            return LGuiPopup.Begin(Title, Size);
        }
        
        public static void EndPopup()
        {
            LGuiPopup.End();
        }

        public static void OpenPopup(string Title)
        {
            LGuiPopup.Open(Title);
        }

        public static void OpenPopup(string Title, LGuiVec2 Pos)
        {
            LGuiPopup.Open(Title, Pos);
        }

        public static void ClosePopup(string Title)
        {
            LGuiPopup.Close(Title);
        }

        public static bool BeginWindow(string Title, LGuiVec2 Size, LGuiWindowFlags Flags = LGuiWindowFlags.None)
        {
            return LGuiWindow.BeginWindow(Title, Size, Flags);
        }

        public static bool BeginWindow(string Title, LGuiRect Rect, LGuiWindowFlags Flags = LGuiWindowFlags.None)
        {
            return LGuiWindow.BeginWindow(Title, Rect, Flags);
        }

        public static void EndWindow()
        {
            LGuiWindow.EndWindow();
        }

        public static void Text(string Format, params object[] Args)
        {
            Text(LGuiStyle.GetColor(LGuiStyleColorIndex.Text), Format, Args);
        }
        
        public static void Text(LGuiColor Color, string Format, params object[] Args)
        {
            LGuiText.OnProcess(Color, Format, Args);
        }

        public static bool Button(string Title, LGuiButtonFlags Flags = LGuiButtonFlags.None)
        {
            return LGuiButton.OnProcess(Title, Flags);
        }

        public static bool Button(string Title, LGuiVec2 Size, LGuiButtonFlags Flags = LGuiButtonFlags.None)
        {
            return LGuiButton.OnProcess(Title, Size, Flags);
        }
        
        public static bool InvisibleButton(string Title, LGuiVec2 Size)
        {
            return LGuiButton.OnProcess(Title, Size, LGuiButtonFlags.Invisible);
        }

        public static bool ColorButton(string Title, LGuiColor Color, LGuiVec2 Size)
        {
            return LGuiColorButton.OnProcess(Title, Color, Size);
        }
        
        public static bool InputText(string Title, ref string Value, uint MaxLength, LGuiVec2 Size, LGuiInputTextFlags Flags = LGuiInputTextFlags.None)
        {
            return LGuiInputText.OnProcess(Title, ref Value, MaxLength, Size, Flags);
        }

        public static bool InputText(string Title, ref string Value)
        {
            return LGuiInputText.OnProcess(Title, ref Value, 64, LGuiInputTextFlags.None);
        }
        
        public static bool CheckBox(string Title, bool Value)
        {
            return LGuiCheckBox.OnProcess(Title, Value);
        }
        
        public static bool Slider(string Title, ref float Value, float Min, float Max, float Step, bool IsHorizontal = true, string Format = "", float Length = 200.0f)
        {
            return LGuiSlider.OnProcess(Title, ref Value, Min, Max, Step, IsHorizontal, Format, Length);
        }
        
        public static bool Slider(string Title, ref int Value, int Min, int Max, int Step, bool IsHorizontal = true, string Format = "", float Length = 200.0f)
        {
            return LGuiSlider.OnProcess(Title, ref Value, Min, Max, Step, IsHorizontal, Format, Length);
        }

        public static void Separator()
        {
            Separator(LGuiStyle.GetColor(LGuiStyleColorIndex.Separator));
        }

        public static void Separator(LGuiColor Color)
        {
            LGuiSeparator.OnProcess(Color);
        }

        public static void Texture(int TextureID)
        {
            Texture(TextureID, LGuiRect.Zero, LGuiVec2.Zero);
        }

        public static void Texture(int TextureID, LGuiVec2 DstSize)
        {
            Texture(TextureID, LGuiRect.Zero, DstSize);
        }

        public static void Texture(int TextureID, LGuiRect SrcRect, LGuiVec2 DstSize)
        {
            LGuiTexture.OnProcess(TextureID, SrcRect, DstSize);
        }

        public static void Texture(string FilePath)
        {
            Texture(FilePath, LGuiRect.Zero, LGuiVec2.Zero);
        }

        public static void Texture(string FilePath, LGuiVec2 DstSize)
        {
            Texture(FilePath, LGuiRect.Zero, DstSize);
        }

        public static void Texture(string FilePath, LGuiRect SrcRect, LGuiVec2 DstSize)
        {
            LGuiTexture.OnProcess(FilePath, SrcRect, DstSize);
        }

        public static int RadioButton(string Title, int Index, int Value)
        {
            return LGuiRadioButton.OnProcess(Title, Index, Value);
        }
        
        public static bool BeginToolTips(LGuiVec2 Size)
        {
            return LGuiToolTips.Begin(Size);
        }

        public static void EndToolTips()
        {
            LGuiToolTips.End();
        }
        
        public static void SetToolTips(string Format, params object[] Args)
        {
            var TextSize = LGuiConvert.GetTextSize(string.Format(Format, Args), LGuiContext.Font) + LGuiStyle.GetFrameChildSpacing() * 2.0f;
            if (BeginToolTips(TextSize))
            {
                Text(Format, Args);
                EndToolTips();
            }
        }

        public static bool ColorPicker(string Title, ref LGuiColor Color)
        {
            return LGuiColorPicker.OnProcess(Title, new LGuiVec2(250, 300), ref Color);
        }

        public static bool ListBox(string Title, ref int ItemIndex, string[] Items, LGuiVec2 Size)
        {
            return LGuiListBox.OnProcess(Title, ref ItemIndex, Items, Size);
        }
        
        public static bool Combox(string Title, ref int ItemIndex, string[] Items, float Width, float PopupHeight)
        {
            return LGuiCombox.OnProcess(Title, ref ItemIndex, Items, Width, PopupHeight);
        }

        public static bool Selectable(string Text, bool Selected)
        {
            return LGuiSelectable.OnProcess(Text, Selected);
        }

        public static bool Selectable(string Text, bool Selected, float Width)
        {
            return LGuiSelectable.OnProcess(Text, Selected, Width);
        }

        public static void ProgressBar(string Title, float Value)
        {
            LGuiProgressBar.OnProcess(Title, Value);
        }

        public static bool PreviousControlIsHovered()
        {
            return LGuiMisc.PreviousControlIsHovered();
        }

        public static bool PreviousControlIsActive()
        {
            return LGuiMisc.PreviousControlIsActive();
        }

        public static void PreviousControlFocus()
        {
            LGuiMisc.PreviousControlFocus();
        }

        public static float GetFrameScrollY()
        {
            var Title = LGuiContext.GetCurrentFrame().Title;
            return LGuiContextCache.GetFrameOffset(Title).Y;
        }

        public static void SetFrameScrollY(float Y)
        {
            var Title = LGuiContext.GetCurrentFrame().Title;
            LGuiContextCache.SetFrameOffset(Title, new LGuiVec2(LGuiContextCache.GetFrameOffset(Title).X, Y));
        }
    }
}