using System.Collections.Generic;

namespace LiteGui
{
    /*
     * Use Dear ImGui Color Style
    colors[ImGuiCol_Text]                   = ImVec4(1.00f, 1.00f, 1.00f, 1.00f);
    colors[ImGuiCol_TextDisabled]           = ImVec4(0.50f, 0.50f, 0.50f, 1.00f);
    colors[ImGuiCol_WindowBg]               = ImVec4(0.06f, 0.06f, 0.06f, 0.94f);
    colors[ImGuiCol_ChildBg]                = ImVec4(1.00f, 1.00f, 1.00f, 0.00f);
    colors[ImGuiCol_PopupBg]                = ImVec4(0.08f, 0.08f, 0.08f, 0.94f);
    colors[ImGuiCol_Border]                 = ImVec4(0.43f, 0.43f, 0.50f, 0.50f);
    colors[ImGuiCol_BorderShadow]           = ImVec4(0.00f, 0.00f, 0.00f, 0.00f);
    colors[ImGuiCol_FrameBg]                = ImVec4(0.16f, 0.29f, 0.48f, 0.54f);
    colors[ImGuiCol_FrameBgHovered]         = ImVec4(0.26f, 0.59f, 0.98f, 0.40f);
    colors[ImGuiCol_FrameBgActive]          = ImVec4(0.26f, 0.59f, 0.98f, 0.67f);
    colors[ImGuiCol_TitleBg]                = ImVec4(0.04f, 0.04f, 0.04f, 1.00f);
    colors[ImGuiCol_TitleBgActive]          = ImVec4(0.16f, 0.29f, 0.48f, 1.00f);
    colors[ImGuiCol_TitleBgCollapsed]       = ImVec4(0.00f, 0.00f, 0.00f, 0.51f);
    colors[ImGuiCol_MenuBarBg]              = ImVec4(0.14f, 0.14f, 0.14f, 1.00f);
    colors[ImGuiCol_ScrollbarBg]            = ImVec4(0.02f, 0.02f, 0.02f, 0.53f);
    colors[ImGuiCol_ScrollbarGrab]          = ImVec4(0.31f, 0.31f, 0.31f, 1.00f);
    colors[ImGuiCol_ScrollbarGrabHovered]   = ImVec4(0.41f, 0.41f, 0.41f, 1.00f);
    colors[ImGuiCol_ScrollbarGrabActive]    = ImVec4(0.51f, 0.51f, 0.51f, 1.00f);
    colors[ImGuiCol_CheckMark]              = ImVec4(0.26f, 0.59f, 0.98f, 1.00f);
    colors[ImGuiCol_SliderGrab]             = ImVec4(0.24f, 0.52f, 0.88f, 1.00f);
    colors[ImGuiCol_SliderGrabActive]       = ImVec4(0.26f, 0.59f, 0.98f, 1.00f);
    colors[ImGuiCol_Button]                 = ImVec4(0.26f, 0.59f, 0.98f, 0.40f);
    colors[ImGuiCol_ButtonHovered]          = ImVec4(0.26f, 0.59f, 0.98f, 1.00f);
    colors[ImGuiCol_ButtonActive]           = ImVec4(0.06f, 0.53f, 0.98f, 1.00f);
    colors[ImGuiCol_Header]                 = ImVec4(0.26f, 0.59f, 0.98f, 0.31f);
    colors[ImGuiCol_HeaderHovered]          = ImVec4(0.26f, 0.59f, 0.98f, 0.80f);
    colors[ImGuiCol_HeaderActive]           = ImVec4(0.26f, 0.59f, 0.98f, 1.00f);
    colors[ImGuiCol_Separator]              = colors[ImGuiCol_Border];
    colors[ImGuiCol_SeparatorHovered]       = ImVec4(0.10f, 0.40f, 0.75f, 0.78f);
    colors[ImGuiCol_SeparatorActive]        = ImVec4(0.10f, 0.40f, 0.75f, 1.00f);
    colors[ImGuiCol_ResizeGrip]             = ImVec4(0.26f, 0.59f, 0.98f, 0.25f);
    colors[ImGuiCol_ResizeGripHovered]      = ImVec4(0.26f, 0.59f, 0.98f, 0.67f);
    colors[ImGuiCol_ResizeGripActive]       = ImVec4(0.26f, 0.59f, 0.98f, 0.95f);
    colors[ImGuiCol_PlotLines]              = ImVec4(0.61f, 0.61f, 0.61f, 1.00f);
    colors[ImGuiCol_PlotLinesHovered]       = ImVec4(1.00f, 0.43f, 0.35f, 1.00f);
    colors[ImGuiCol_PlotHistogram]          = ImVec4(0.90f, 0.70f, 0.00f, 1.00f);
    colors[ImGuiCol_PlotHistogramHovered]   = ImVec4(1.00f, 0.60f, 0.00f, 1.00f);
    colors[ImGuiCol_TextSelectedBg]         = ImVec4(0.26f, 0.59f, 0.98f, 0.35f);
    colors[ImGuiCol_DragDropTarget]         = ImVec4(1.00f, 1.00f, 0.00f, 0.90f);
    colors[ImGuiCol_NavHighlight]           = ImVec4(0.26f, 0.59f, 0.98f, 1.00f);
    colors[ImGuiCol_NavWindowingHighlight]  = ImVec4(1.00f, 1.00f, 1.00f, 0.70f);
    colors[ImGuiCol_NavWindowingDimBg]      = ImVec4(0.80f, 0.80f, 0.80f, 0.20f);
    colors[ImGuiCol_ModalWindowDimBg]       = ImVec4(0.80f, 0.80f, 0.80f, 0.35f);
     */

    public enum LGuiStyleColorIndex : byte
    {
        Frame               = 00,
        FrameHovered        = 01,
        FrameActived        = 02,
        Border              = 03,
        Text                = 04,
        TextDisabled        = 05,
        Button              = 06,
        ButtonHovered       = 07,
        ButtonActived       = 08,
        CheckMask           = 09,
        Separator           = 10,
        SeparatorHovered    = 11,
        SeparatorActived    = 12,
        Group               = 13,
        GroupHovered        = 14,
        GroupActived        = 15,
        Slider              = 16,
        SliderGrab          = 17,
        SliderGrabHovered   = 18,
        SliderGrabActived   = 19,
        Header              = 20,
        HeaderHovered       = 21,
        HeaderActive        = 22,
    }

    internal class LGuiStyleColorBackup
    {
        internal LGuiStyleColorIndex Index { get; }
        internal LGuiColor Color { get; }

        internal LGuiStyleColorBackup(LGuiStyleColorIndex Index, LGuiColor Color)
        {
            this.Index = Index;
            this.Color = Color;
        }
    }

    public enum LGuiStyleValueIndex : byte
    {
        ControlSpacingX     = 00,
        ControlSpacingY     = 01,
        FrameChildSpacingX  = 02,
        FrameChildSpacingY  = 03,
        GroupChildSpacing   = 04,
        GrabMinSizeX        = 05,
        GrabMinSizeY        = 06,
        RadioButtonRadius   = 07,
        CheckBoxSize        = 08,
        SliderSize          = 09,
    }

    internal class LGuiStyleValueBackup
    {
        internal LGuiStyleValueIndex Index { get; }
        internal float Value { get; }

        internal LGuiStyleValueBackup(LGuiStyleValueIndex Index, float Value)
        {
            this.Index = Index;
            this.Value = Value;
        }
    }
    
    internal static class LGuiStyle
    {
        internal static LGuiColor[] Colors = null;
        internal static Stack<LGuiStyleColorBackup> ColorStack = null;

        internal static float[] Values = null;
        internal static Stack<LGuiStyleValueBackup> ValueStack = null;

        static LGuiStyle()
        {
            Colors = new LGuiColor[System.Enum.GetValues(typeof(LGuiStyleColorIndex)).Length];
            ColorStack = new Stack<LGuiStyleColorBackup>();

            Values = new float[System.Enum.GetValues(typeof(LGuiStyleValueIndex)).Length];
            ValueStack = new Stack<LGuiStyleValueBackup>();

            InitDefaultColor();
            InitDefaultValue();
        }

        private static void InitDefaultColor()
        {
            Colors[(int)LGuiStyleColorIndex.Frame]              = new LGuiColor(0.06f, 0.06f, 0.06f, 0.94f);
            Colors[(int)LGuiStyleColorIndex.FrameHovered]       = new LGuiColor(0.41f, 0.41f, 0.41f, 1.00f);
            Colors[(int)LGuiStyleColorIndex.FrameActived]       = new LGuiColor(0.51f, 0.51f, 0.51f, 1.00f);
            Colors[(int)LGuiStyleColorIndex.Border]             = new LGuiColor(0.43f, 0.43f, 0.50f, 0.50f);
            Colors[(int)LGuiStyleColorIndex.Text]               = new LGuiColor(1.00f, 1.00f, 1.00f, 1.00f);
            Colors[(int)LGuiStyleColorIndex.TextDisabled]       = new LGuiColor(0.50f, 0.50f, 0.50f, 1.00f);
            Colors[(int)LGuiStyleColorIndex.Button]             = new LGuiColor(0.26f, 0.59f, 0.98f, 0.40f);
            Colors[(int)LGuiStyleColorIndex.ButtonHovered]      = new LGuiColor(0.26f, 0.59f, 0.98f, 1.00f);
            Colors[(int)LGuiStyleColorIndex.ButtonActived]      = new LGuiColor(0.06f, 0.53f, 0.98f, 1.00f);
            Colors[(int)LGuiStyleColorIndex.CheckMask]          = new LGuiColor(0.90f, 0.70f, 0.00f, 1.00f);
            Colors[(int)LGuiStyleColorIndex.Separator]          = new LGuiColor(0.43f, 0.43f, 0.50f, 0.50f);
            Colors[(int)LGuiStyleColorIndex.SeparatorHovered]   = new LGuiColor(0.10f, 0.40f, 0.75f, 0.78f);
            Colors[(int)LGuiStyleColorIndex.SeparatorActived]   = new LGuiColor(0.10f, 0.40f, 0.75f, 1.00f);
            Colors[(int)LGuiStyleColorIndex.Group]              = new LGuiColor(0.26f, 0.59f, 0.98f, 0.40f);
            Colors[(int)LGuiStyleColorIndex.GroupHovered]       = new LGuiColor(0.26f, 0.59f, 0.98f, 1.00f);
            Colors[(int)LGuiStyleColorIndex.GroupActived]       = new LGuiColor(0.06f, 0.53f, 0.98f, 1.00f);
            Colors[(int)LGuiStyleColorIndex.Slider]             = new LGuiColor(0.02f, 0.02f, 0.02f, 0.53f);
            Colors[(int)LGuiStyleColorIndex.SliderGrab]         = new LGuiColor(0.31f, 0.31f, 0.31f, 1.00f);
            Colors[(int)LGuiStyleColorIndex.SliderGrabHovered]  = new LGuiColor(0.41f, 0.41f, 0.41f, 1.00f);
            Colors[(int)LGuiStyleColorIndex.SliderGrabActived]  = new LGuiColor(0.51f, 0.51f, 0.51f, 1.00f);
            Colors[(int)LGuiStyleColorIndex.Header]             = new LGuiColor(0.26f, 0.59f, 0.98f, 0.31f);
            Colors[(int)LGuiStyleColorIndex.HeaderHovered]      = new LGuiColor(0.26f, 0.59f, 0.98f, 0.80f);
            Colors[(int)LGuiStyleColorIndex.HeaderActive]       = new LGuiColor(0.26f, 0.59f, 0.98f, 1.00f);
        }

        private static void InitDefaultValue()
        {
            Values[(int)LGuiStyleValueIndex.ControlSpacingX]        = 5.0f;
            Values[(int)LGuiStyleValueIndex.ControlSpacingY]        = 5.0f;
            Values[(int)LGuiStyleValueIndex.FrameChildSpacingX]     = 5.0f;
            Values[(int)LGuiStyleValueIndex.FrameChildSpacingY]     = 5.0f;
            Values[(int)LGuiStyleValueIndex.GroupChildSpacing]      = 20.0f;
            Values[(int)LGuiStyleValueIndex.GrabMinSizeX]           = 10.0f;
            Values[(int)LGuiStyleValueIndex.GrabMinSizeY]           = 10.0f;
            Values[(int)LGuiStyleValueIndex.RadioButtonRadius]      = 10.0f;
            Values[(int)LGuiStyleValueIndex.CheckBoxSize]           = 20.0f;
            Values[(int)LGuiStyleValueIndex.SliderSize]             = 16.0f;
        }

        internal static LGuiColor GetColor(LGuiStyleColorIndex Index)
        {
            return Colors[(int) Index];
        }
        
        internal static void PushColor(LGuiStyleColorIndex Index, LGuiColor Color)
        {
            var Backup = new LGuiStyleColorBackup(Index, GetColor(Index));
            ColorStack.Push(Backup);
            Colors[(int) Index] = Color;
        }

        internal static void PopColor(int Count = 1)
        {
            if (Count > ColorStack.Count)
            {
                Count = ColorStack.Count;
            }

            for (var Index = 0; Index < Count; ++Index)
            {
                var Backup = ColorStack.Pop();
                Colors[(int) Backup.Index] = Backup.Color;
            }
        }

        internal static float GetValue(LGuiStyleValueIndex Index)
        {
            return Values[(int) Index];
        }

        internal static void PushValue(LGuiStyleValueIndex Index, float Value)
        {
            var Backup = new LGuiStyleValueBackup(Index, GetValue(Index));
            ValueStack.Push(Backup);
            Values[(int) Index] = Value;
        }

        internal static void PopValue(int Count = 1)
        {
            if (Count > ValueStack.Count)
            {
                Count = ValueStack.Count;
            }

            for (var Index = 0; Index < Count; ++Index)
            {
                var Backup = ValueStack.Pop();
                Values[(int) Backup.Index] = Backup.Value;
            }
        }
        


        internal static LGuiVec2 GetFrameChildSpacing()
        {
            return new LGuiVec2(GetValue(LGuiStyleValueIndex.FrameChildSpacingX), GetValue(LGuiStyleValueIndex.FrameChildSpacingY));
        }

        internal static LGuiVec2 GetControlSpacing()
        {
            return new LGuiVec2(GetValue(LGuiStyleValueIndex.ControlSpacingX), GetValue(LGuiStyleValueIndex.ControlSpacingY));
        }

        internal static LGuiVec2 GetGrabMinSize()
        {
            return new LGuiVec2(GetValue(LGuiStyleValueIndex.GrabMinSizeX), GetValue(LGuiStyleValueIndex.GrabMinSizeY));
        }
    }
}