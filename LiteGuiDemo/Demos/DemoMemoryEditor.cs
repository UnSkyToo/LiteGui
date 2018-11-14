using System;
using System.Globalization;
using System.Text;
using LiteGui;
using LiteGui.Graphics;

namespace LiteGuiDemo.Demos
{
    internal class DemoMemoryEditor : DemoBase
    {
        private readonly byte[] MemoryBuffer = new byte[10240];

        private enum DataType : byte
        {
            DataType_S8,
            DataType_U8,
            DataType_S16,
            DataType_U16,
            DataType_S32,
            DataType_U32,
            DataType_S64,
            DataType_U64,
            DataType_Float,
            DataType_Double,
            DataType_COUNT
        };

        private enum DataFormat : byte
        {
            DataFormat_Bin = 0,
            DataFormat_Dec = 1,
            DataFormat_Hex = 2,
            DataFormat_COUNT
        };

        // Settings
        private bool Open; // = true   // set to false when DrawWindow() was closed. ignore if not using DrawWindow().
        private bool ReadOnly; // = false  // disable any editing.
        private int Cols; // = 16     // number of columns to display.

        private bool
            OptShowOptions; // = true   // display options button/context menu. when disabled, options will be locked unless you provide your own UI for them.

        private bool
            OptShowDataPreview; // = false  // display a footer previewing the decimal/binary/hex/float representation of the currently selected bytes.

        private bool
            OptShowHexII; // = false  // display values in HexII representation instead of regular hexadecimal: hide null/zero bytes, ascii values as ".X".

        private bool OptShowAscii; // = true   // display ASCII representation on the right side.
        private bool OptGreyOutZeroes; // = true   // display null/zero bytes using the TextDisabled color.
        private bool OptUpperCaseHex; // = true   // display hexadecimal values as "FF" instead of "ff".
        private int OptMidColsCount; // = 8      // set to 0 to disable extra spacing between every mid-cols.

        private int
            OptAddrDigitsCount; // = 0      // number of addr digits to display (default calculated based on maximum displayed addr).

        private LGuiColor HighlightColor; //          // background color of highlighted bytes.
        //u8(*ReadFn)(const u8* data, size_t off);  // = NULL   // optional handler to read bytes.
        //void (* WriteFn) (u8* data, size_t off, u8 d); // = NULL   // optional handler to write bytes.
        //bool (* HighlightFn) (const u8* data, size_t off);//NULL   // optional handler to return Highlight property (to support non-contiguous highlighting).

        // [Internal State]
        private bool ContentsWidthChanged;
        private int DataPreviewAddr;
        private int DataEditingAddr;
        private bool DataEditingTakeFocus;
        private string DataInputBuf = string.Empty;
        private string AddrInputBuf = string.Empty;
        private int GotoAddr;
        private int HighlightMin, HighlightMax;
        private int PreviewEndianess;
        private DataType PreviewDataType;

        internal DemoMemoryEditor()
        {
            var R = new Random((int)DateTime.Now.Ticks);
            R.NextBytes(MemoryBuffer);

            // Settings
            Open = true;
            ReadOnly = false;
            Cols = 16;
            OptShowOptions = true;
            OptShowDataPreview = false;
            OptShowHexII = false;
            OptShowAscii = true;
            OptGreyOutZeroes = true;
            OptUpperCaseHex = true;
            OptMidColsCount = 8;
            OptAddrDigitsCount = 0;
            HighlightColor = new LGuiColor(255, 255, 255, 50);

            // State/Internals
            ContentsWidthChanged = false;
            DataPreviewAddr = DataEditingAddr = (int)-1;
            DataEditingTakeFocus = false;
            GotoAddr = 4 - 1;
            HighlightMin = HighlightMax = (int)-1;
            PreviewEndianess = 0;
            PreviewDataType = DataType.DataType_S32;
        }

        void GotoAddrAndHighlight(int addr_min, int addr_max)
        {
            GotoAddr = addr_min;
            HighlightMin = addr_min;
            HighlightMax = addr_max;
        }

        private struct Sizes
        {
            internal int AddrDigitsCount;
            internal float LineHeight;
            internal float GlyphWidth;
            internal float HexCellWidth;
            internal float SpacingBetweenMidCols;
            internal float PosHexStart;
            internal float PosHexEnd;
            internal float PosAsciiStart;
            internal float PosAsciiEnd;
            internal float WindowWidth;
        };

        internal override bool Startup()
        {
            return true;
        }

        internal override void Shutdown()
        {
        }

        internal override void Tick(float Seconds)
        {
        }

        internal override void Render()
        {
            DrawWindow("MemoryDemo", MemoryBuffer, MemoryBuffer.Length);
        }

        void CalcSizes(ref Sizes s, int mem_size, int base_display_addr)
        {
            //ImGuiStyle & style = ImGui::GetStyle();
            s.AddrDigitsCount = OptAddrDigitsCount;
            if (s.AddrDigitsCount == 0)
                for (int n = base_display_addr + mem_size - 1; n > 0; n >>= 4)
                    s.AddrDigitsCount++;
            s.LineHeight = LGuiFont.Default.FontHeight + 2;
            s.GlyphWidth = LGuiFont.Default.FontWidth + 1; // We assume the font is mono-space
            s.HexCellWidth =
                (float) (int) (s.GlyphWidth *
                               2.5f); // "FF " we include trailing space in the width to easily catch clicks everywhere
            s.SpacingBetweenMidCols =
                (float) (int) (s.HexCellWidth * 0.25f); // Every OptMidColsCount columns we add a bit of extra spacing
            s.PosHexStart = (s.AddrDigitsCount + 2) * s.GlyphWidth;
            s.PosHexEnd = s.PosHexStart + (s.HexCellWidth * Cols);
            s.PosAsciiStart = s.PosAsciiEnd = s.PosHexEnd;
            if (OptShowAscii)
            {
                s.PosAsciiStart = s.PosHexEnd + s.GlyphWidth * 1;
                if (OptMidColsCount > 0)
                    s.PosAsciiStart += ((Cols + OptMidColsCount - 1) / OptMidColsCount) * s.SpacingBetweenMidCols;
                s.PosAsciiEnd = s.PosAsciiStart + Cols * s.GlyphWidth;
            }

            s.WindowWidth = s.PosAsciiEnd + 5 * 2 + s.GlyphWidth;
        }

        // Standalone Memory Editor window
        internal void DrawWindow(string title, byte[] mem_data, int mem_size, int base_display_addr = 0x0000)
        {
            Sizes s = new Sizes();
            CalcSizes(ref s, mem_size, base_display_addr);

            LGui.Begin();
            LGui.BeginFrame(title, new LGuiVec2(s.WindowWidth, 500));

            DrawContents(mem_data, mem_size, base_display_addr);

            LGui.EndFrame();

            LGui.Slider("Cols", ref Cols, 4, 32, 1, true, true, 200.0f);
            LGui.SameLine();
            OptShowAscii = LGui.CheckBox("OptShowAscii", OptShowAscii);
            LGui.SameLine();
            OptShowDataPreview = LGui.CheckBox("OptShowDataPreview", OptShowDataPreview);
            if (OptShowDataPreview && DataPreviewAddr == -1)
            {
                DataPreviewAddr = 0;
            }

            LGui.End();
        }

        // Memory Editor contents only
        void DrawContents(byte[] mem_data_void_ptr, int mem_size, int base_display_addr = 0x0000)
        {
            var CmdList = LGui.CreateCommandList();

            var mem_data = mem_data_void_ptr;
            Sizes s = new Sizes();
            CalcSizes(ref s, mem_size, base_display_addr);
            var IO = LGui.GetIO();

            // We begin into our scrolling region with the 'ImGuiWindowFlags_NoMove' in order to prevent click from moving the window.
            // This is used as a facility since our main click detection code doesn't assign an ActiveId so the click would normally be caught as a window-move.
            float height_separator = 5;
            
            int line_total_count = (int)((mem_size + Cols - 1) / Cols);
            /*ImGuiListClipper clipper(line_total_count, s.LineHeight);
            const size_t visible_start_addr = clipper.DisplayStart * Cols;
            const size_t visible_end_addr = clipper.DisplayEnd * Cols;*/

            var DisplayStart = (int)(LGui.GetFrameScrollY() / (s.LineHeight + 3));
            var DisplayEnd = (int)((LGui.GetFrameScrollY() + 500) / (s.LineHeight + 3));
            int visible_start_addr = DisplayStart * Cols;
            int visible_end_addr = DisplayEnd * Cols;

            bool data_next = false;

            if (ReadOnly || DataEditingAddr >= mem_size)
                DataEditingAddr = (int)-1;
            if (DataPreviewAddr >= mem_size)
                DataPreviewAddr = (int)-1;

            int preview_data_type_size = OptShowDataPreview ? DataTypeGetSize(PreviewDataType) : 0;

            int data_editing_addr_backup = DataEditingAddr;
            int data_editing_addr_next = (int)-1;
            if (DataEditingAddr != (int)-1)
            {
                // Move cursor but only apply on next frame so scrolling with be synchronized (because currently we can't change the scrolling while the window is being rendered)
                if (IO.IsKeyPressed(LGuiKeys.Up) && DataEditingAddr >= (int)Cols) { data_editing_addr_next = DataEditingAddr - Cols; DataEditingTakeFocus = true; }
                else if (IO.IsKeyPressed(LGuiKeys.Down) && DataEditingAddr < mem_size - Cols) { data_editing_addr_next = DataEditingAddr + Cols; DataEditingTakeFocus = true; }
                else if (IO.IsKeyPressed(LGuiKeys.Left) && DataEditingAddr > 0) { data_editing_addr_next = DataEditingAddr - 1; DataEditingTakeFocus = true; }
                else if (IO.IsKeyPressed(LGuiKeys.Right) && DataEditingAddr < mem_size - 1) { data_editing_addr_next = DataEditingAddr + 1; DataEditingTakeFocus = true; }
            }
            if (data_editing_addr_next != (int)-1 && (data_editing_addr_next / Cols) != (data_editing_addr_backup / Cols))
            {
                // Track cursor movements
                int scroll_offset = ((int)(data_editing_addr_next / Cols) - (int)(data_editing_addr_backup / Cols));
                bool scroll_desired = (scroll_offset < 0 && data_editing_addr_next < visible_start_addr + Cols * 2) || (scroll_offset > 0 && data_editing_addr_next > visible_end_addr - Cols * 2);
                if (scroll_desired)
                    LGui.SetFrameScrollY(LGui.GetFrameScrollY() + scroll_offset * (s.LineHeight + 3));
            }

            // Draw vertical separator
            LGuiVec2 window_pos = new LGuiVec2(5, 5);
            if (OptShowAscii)
            {
                CmdList.DrawLine(
                    new LGuiVec2(window_pos.X + s.PosAsciiStart - s.GlyphWidth, window_pos.Y),
                    new LGuiVec2(window_pos.X + s.PosAsciiStart - s.GlyphWidth, window_pos.Y + 9999),
                    new LGuiColor(0.43f, 0.43f, 0.50f, 0.50f));
            }

            var color_text = new LGuiColor(1.00f, 1.00f, 1.00f, 1.00f);
            var color_disabled = OptGreyOutZeroes ? new LGuiColor(0.50f, 0.50f, 0.50f, 1.00f) : color_text;

            var format_address = OptUpperCaseHex ? "{0:X4}: " : "{0:X4}: ";
            var format_data = OptUpperCaseHex ? "{0:X4}: " : "{0:X4}: ";
            var format_range = OptUpperCaseHex ? "Range {0:X4}..{1:X4}" : "Range {0:X4}..{1:X4}";
            var format_byte = OptUpperCaseHex ? "{0:X2}" : "{0:X2}";
            var format_byte_space = OptUpperCaseHex ? "{0:X2}" : "{0:X2}";

            for (int line_i = 0; line_i < mem_size / Cols; ++line_i)
            {
                int addr = (int)(line_i * Cols);

                LGui.Text(format_address, base_display_addr + addr);

                for (int n = 0; n < Cols && addr < mem_size; ++n, ++addr)
                {
                    LGui.PushID(addr);

                    float byte_pos_x = s.PosHexStart + s.HexCellWidth * n;
                    if (OptMidColsCount > 0)
                        byte_pos_x += (n / OptMidColsCount) * s.SpacingBetweenMidCols;
                    LGui.SameLine(byte_pos_x);

                    // Draw highlight
                    bool is_highlight_from_user_range = (addr >= HighlightMin && addr < HighlightMax);
                    bool is_highlight_from_preview = (addr >= DataPreviewAddr && addr < DataPreviewAddr + preview_data_type_size);
                    if (is_highlight_from_user_range || is_highlight_from_preview)
                    {
                        var pos = LGui.GetCursorPos();
                        float highlight_width = s.GlyphWidth * 2;
                        bool is_next_byte_highlighted = (addr + 1 < mem_size) && ((HighlightMax != (int)-1 && addr + 1 < HighlightMax));
                        if (is_next_byte_highlighted || (n + 1 == Cols))
                        {
                            highlight_width = s.HexCellWidth;
                            if (OptMidColsCount > 0 && n > 0 && (n + 1) < Cols && ((n + 1) % OptMidColsCount) == 0)
                                highlight_width += s.SpacingBetweenMidCols;
                        }
                        CmdList.DrawRect(new LGuiRect(pos, new LGuiVec2(highlight_width, s.LineHeight)), HighlightColor, true, false);
                    }
                    if (DataEditingAddr == addr)
                    {// Display text input on current byte
                        bool data_write = false;
                        if (DataEditingTakeFocus)
                        {
                            AddrInputBuf = string.Format(format_data, s.AddrDigitsCount, base_display_addr + addr);
                            DataInputBuf = string.Format(format_byte, mem_data[addr]);
                        }
                        var itemwidth = s.GlyphWidth * 2;
                        /*struct UserData
                        {
                            // FIXME: We should have a way to retrieve the text edit cursor position more easily in the API, this is rather tedious. This is such a ugly mess we may be better off not using InputText() at all here.
                            static int Callback(ImGuiInputTextCallbackData* data)
                            {
                                UserData* user_data = (UserData*)data->UserData;
                                if (!data->HasSelection())
                                    user_data->CursorPos = data->CursorPos;
                                if (data->SelectionStart == 0 && data->SelectionEnd == data->BufTextLen)
                                {
                                    // When not editing a byte, always rewrite its content (this is a bit tricky, since InputText technically "owns" the master copy of the buffer we edit it in there)
                                    data->DeleteChars(0, data->BufTextLen);
                                    data->InsertChars(0, user_data->CurrentBufOverwrite);
                                    data->SelectionStart = 0;
                                    data->SelectionEnd = data->CursorPos = 2;
                                }
                                return 0;
                            }
                            char CurrentBufOverwrite[3];  // Input
                            int CursorPos;               // Output
                        };
                        UserData user_data;
                        user_data.CursorPos = -1;
                        sprintf(user_data.CurrentBufOverwrite, format_byte, ReadFn? ReadFn(mem_data, addr) : mem_data[addr]);
                        ImGuiInputTextFlags flags = ImGuiInputTextFlags_CharsHexadecimal | ImGuiInputTextFlags_EnterReturnsTrue | ImGuiInputTextFlags_AutoSelectAll | ImGuiInputTextFlags_NoHorizontalScroll | ImGuiInputTextFlags_AlwaysInsertMode | ImGuiInputTextFlags_CallbackAlways;
                        if (ImGui::InputText("##data", DataInputBuf, 32, flags, UserData::Callback, &user_data))
                            data_write = data_next = true;
                        else if (!DataEditingTakeFocus && !ImGui::IsItemActive())
                            DataEditingAddr = data_editing_addr_next = (size_t)-1;
                        DataEditingTakeFocus = false;
                        if (user_data.CursorPos >= 2)
                            data_write = data_next = true;
                        if (data_editing_addr_next != (size_t)-1)
                            data_write = data_next = false;
                        int data_input_value;
                        if (data_write && sscanf(DataInputBuf, "%X", &data_input_value) == 1)
                        {
                            if (WriteFn)
                                WriteFn(mem_data, addr, (u8) data_input_value);
                            else
                                mem_data[addr] = (u8) data_input_value;
                        }*/


                        var Flags = LGuiInputTextFlags.CharsHexadecimal | /*LGuiInputTextFlags.AutoSelectAll |*/
                                    LGuiInputTextFlags.InsertMode | LGuiInputTextFlags.EnterReturnsTrue;
                        if (LGui.InputText("##data", ref DataInputBuf, 2,
                            new LGuiVec2(itemwidth + 5, LGuiFont.Default.FontHeight + 2), Flags))
                        {
                            data_write = data_next = true;
                        }
                        else if (!DataEditingTakeFocus && !LGui.PreviousControlIsActive())
                        {
                            DataEditingAddr = data_editing_addr_next = (int)-1;
                        }

                        //LGui.PreviousControlFocus();

                        int data_input_value;
                        if (data_write && int.TryParse(DataInputBuf, NumberStyles.HexNumber, null, out data_input_value))
                        {
                            mem_data[addr] = (byte)data_input_value;
                        }
                    }
                    else
                    {
                        byte b = mem_data[addr];

                        if (OptShowHexII)
                        {
                            if ((b >= 32 && b < 128))
                                LGui.Text(".{0}", (char)b);
                            else if (b == 0xFF && OptGreyOutZeroes)
                                LGui.Text(color_disabled, "##");
                            else if (b == 0x00)
                                LGui.Text("  ");
                            else
                                LGui.Text(format_byte_space, b);
                        }
                        else
                        {
                            if (b == 0 && OptGreyOutZeroes)
                                LGui.Text(color_disabled, "00");
                            else
                                LGui.Text(format_byte_space, b);
                        }

                        if (!ReadOnly && LGui.PreviousControlIsHovered() && IO.IsMouseClick(LGuiMouseButtons.Left))
                        {
                            DataEditingTakeFocus = true;
                            data_editing_addr_next = addr;
                        }
                    }

                    LGui.PopID();
                }

                if (OptShowAscii)
                {
                    // Draw ASCII values
                    LGui.SameLine(s.PosAsciiStart);
                    var pos = LGui.GetCursorPos();
                    addr = line_i * Cols;
                    LGui.PushID(line_i);
                    if (LGui.InvisibleButton("ascii", new LGuiVec2(s.PosAsciiEnd - s.PosAsciiStart, s.LineHeight)))
                    {
                        DataEditingAddr = DataPreviewAddr = addr + (int)((LGui.GetIO().MousePos.X - pos.X) / s.GlyphWidth);
                        DataEditingTakeFocus = true;
                    }
                    LGui.PopID();
                    for (int n = 0; n < Cols && addr < mem_size; n++, addr++)
                    {
                        if (addr == DataEditingAddr)
                        {
                            CmdList.DrawRect(new LGuiRect(pos, new LGuiVec2(s.GlyphWidth, s.LineHeight)), new LGuiColor(0.16f, 0.29f, 0.48f, 0.54f), true, false);
                            CmdList.DrawRect(new LGuiRect(pos, new LGuiVec2(s.GlyphWidth, s.LineHeight)), new LGuiColor(0.26f, 0.59f, 0.98f, 0.35f), true, false);
                        }
                        byte c = mem_data[addr];
                        char display_c = (c < 32 || c >= 128) ? '.' : (char)c;
                        LGui.SameLine(pos.X);
                        LGui.Text((display_c == '.') ? color_disabled : color_text, "{0}", display_c.ToString());
                        //LGuiGraphics.DrawText(display_c.ToString(), pos, new LGuiTextStyle(LGuiFontStyle.Default, (display_c == '.') ? color_disabled : color_text));
                        pos.X += s.GlyphWidth;
                    }
                }
            }


            if (data_next && DataEditingAddr < mem_size)
            {
                DataEditingAddr = DataPreviewAddr = DataEditingAddr + 1;
                DataEditingTakeFocus = true;
            }
            else if (data_editing_addr_next != (int)-1)
            {
                DataEditingAddr = DataPreviewAddr = data_editing_addr_next;
            }

            LGui.AddCommandList(CmdList);

            /*for (int line_i = clipper.DisplayStart; line_i < clipper.DisplayEnd; line_i++) // display only visible lines
            {
                int addr = (int)(line_i * Cols);

                LGui.Text(format_address, base_display_addr + addr);

                // Draw Hexadecimal
                for (int n = 0; n < Cols && addr < mem_size; n++, addr++)
                {
                    float byte_pos_x = s.PosHexStart + s.HexCellWidth * n;
                    if (OptMidColsCount > 0)
                        byte_pos_x += (n / OptMidColsCount) * s.SpacingBetweenMidCols;
                    ImGui::SameLine(byte_pos_x);

                    // Draw highlight
                    bool is_highlight_from_user_range = (addr >= HighlightMin && addr < HighlightMax);
                    bool is_highlight_from_user_func = (HighlightFn && HighlightFn(mem_data, addr));
                    bool is_highlight_from_preview = (addr >= DataPreviewAddr && addr < DataPreviewAddr + preview_data_type_size);
                    if (is_highlight_from_user_range || is_highlight_from_user_func || is_highlight_from_preview)
                    {
                        ImVec2 pos = ImGui::GetCursorScreenPos();
                        float highlight_width = s.GlyphWidth * 2;
                        bool is_next_byte_highlighted = (addr + 1 < mem_size) && ((HighlightMax != (size_t) - 1 && addr + 1 < HighlightMax) || (HighlightFn && HighlightFn(mem_data, addr + 1)));
                        if (is_next_byte_highlighted || (n + 1 == Cols))
                        {
                            highlight_width = s.HexCellWidth;
                            if (OptMidColsCount > 0 && n > 0 && (n + 1) < Cols && ((n + 1) % OptMidColsCount) == 0)
                                highlight_width += s.SpacingBetweenMidCols;
                        }
                        draw_list->AddRectFilled(pos, ImVec2(pos.x + highlight_width, pos.y + s.LineHeight), HighlightColor);
                    }

                    if (DataEditingAddr == addr)
                    {
                        // Display text input on current byte
                        bool data_write = false;
                        ImGui::PushID((void*)addr);
                        if (DataEditingTakeFocus)
                        {
                            ImGui::SetKeyboardFocusHere();
                            ImGui::CaptureKeyboardFromApp(true);
                            sprintf(AddrInputBuf, format_data, s.AddrDigitsCount, base_display_addr + addr);
                            sprintf(DataInputBuf, format_byte, ReadFn ? ReadFn(mem_data, addr) : mem_data[addr]);
                        }
                        ImGui::PushItemWidth(s.GlyphWidth * 2);
                        struct UserData
                        {
                            // FIXME: We should have a way to retrieve the text edit cursor position more easily in the API, this is rather tedious. This is such a ugly mess we may be better off not using InputText() at all here.
                            static int Callback(ImGuiInputTextCallbackData* data)
                            {
                                UserData* user_data = (UserData*)data->UserData;
                                if (!data->HasSelection())
                                    user_data->CursorPos = data->CursorPos;
                                if (data->SelectionStart == 0 && data->SelectionEnd == data->BufTextLen)
                                {
                                    // When not editing a byte, always rewrite its content (this is a bit tricky, since InputText technically "owns" the master copy of the buffer we edit it in there)
                                    data->DeleteChars(0, data->BufTextLen);
                                    data->InsertChars(0, user_data->CurrentBufOverwrite);
                                    data->SelectionStart = 0;
                                    data->SelectionEnd = data->CursorPos = 2;
                                }
                                return 0;
                            }
                            char CurrentBufOverwrite[3];  // Input
                            int CursorPos;               // Output
                        };
                        UserData user_data;
                        user_data.CursorPos = -1;
                        sprintf(user_data.CurrentBufOverwrite, format_byte, ReadFn? ReadFn(mem_data, addr) : mem_data[addr]);
                        ImGuiInputTextFlags flags = ImGuiInputTextFlags_CharsHexadecimal | ImGuiInputTextFlags_EnterReturnsTrue | ImGuiInputTextFlags_AutoSelectAll | ImGuiInputTextFlags_NoHorizontalScroll | ImGuiInputTextFlags_AlwaysInsertMode | ImGuiInputTextFlags_CallbackAlways;
                        if (ImGui::InputText("##data", DataInputBuf, 32, flags, UserData::Callback, &user_data))
                            data_write = data_next = true;
                        else if (!DataEditingTakeFocus && !ImGui::IsItemActive())
                            DataEditingAddr = data_editing_addr_next = (size_t)-1;
                        DataEditingTakeFocus = false;
                        ImGui::PopItemWidth();
                        if (user_data.CursorPos >= 2)
                            data_write = data_next = true;
                        if (data_editing_addr_next != (size_t)-1)
                            data_write = data_next = false;
                        int data_input_value;
                        if (data_write && sscanf(DataInputBuf, "%X", &data_input_value) == 1)
                        {
                            if (WriteFn)
                                WriteFn(mem_data, addr, (u8) data_input_value);
                            else
                                mem_data[addr] = (u8) data_input_value;
                        }
                        ImGui::PopID();
                    }
                    else
                    {
                        // NB: The trailing space is not visible but ensure there's no gap that the mouse cannot click on.
                        u8 b = ReadFn ? ReadFn(mem_data, addr) : mem_data[addr];

                        if (OptShowHexII)
                        {
                            if ((b >= 32 && b< 128))
                                ImGui::Text(".%c ", b);
                            else if (b == 0xFF && OptGreyOutZeroes)
                                ImGui::TextDisabled("## ");
                            else if (b == 0x00)
                                ImGui::Text("   ");
                            else
                                ImGui::Text(format_byte_space, b);
                        }
                        else
                        {
                            if (b == 0 && OptGreyOutZeroes)
                                ImGui::TextDisabled("00 ");
                            else
                                ImGui::Text(format_byte_space, b);
                        }
                        if (!ReadOnly && ImGui::IsItemHovered() && ImGui::IsMouseClicked(0))
                        {
                            DataEditingTakeFocus = true;
                            data_editing_addr_next = addr;
                        }
                    }
                }

                if (OptShowAscii)
                {
                    // Draw ASCII values
                    ImGui::SameLine(s.PosAsciiStart);
                    ImVec2 pos = ImGui::GetCursorScreenPos();
        addr = line_i* Cols;
        ImGui::PushID(line_i);
                    if (ImGui::InvisibleButton("ascii", ImVec2(s.PosAsciiEnd - s.PosAsciiStart, s.LineHeight)))
                    {
                        DataEditingAddr = DataPreviewAddr = addr + (size_t) ((ImGui::GetIO().MousePos.x - pos.x) / s.GlyphWidth);
                        DataEditingTakeFocus = true;
                    }
                    ImGui::PopID();
                    for (int n = 0; n<Cols && addr<mem_size; n++, addr++)
                    {
                        if (addr == DataEditingAddr)
                        {
                            draw_list->AddRectFilled(pos, ImVec2(pos.x + s.GlyphWidth, pos.y + s.LineHeight), ImGui::GetColorU32(ImGuiCol_FrameBg));
                            draw_list->AddRectFilled(pos, ImVec2(pos.x + s.GlyphWidth, pos.y + s.LineHeight), ImGui::GetColorU32(ImGuiCol_TextSelectedBg));
                        }
                        unsigned char c = ReadFn ? ReadFn(mem_data, addr) : mem_data[addr];
        char display_c = (c < 32 || c >= 128) ? '.' : c;
        draw_list->AddText(pos, (display_c == '.') ? color_disabled : color_text, &display_c, &display_c + 1);
                        pos.x += s.GlyphWidth;
                    }
                }
            }
            clipper.End();
            ImGui::PopStyleVar(2);
            ImGui::EndChild();

            if (data_next && DataEditingAddr<mem_size)
            {
                DataEditingAddr = DataPreviewAddr = DataEditingAddr + 1;
                DataEditingTakeFocus = true;
            }
            else if (data_editing_addr_next != (size_t)-1)
            {
                DataEditingAddr = DataPreviewAddr = data_editing_addr_next;
            }

            bool next_show_data_preview = OptShowDataPreview;
            if (OptShowOptions)
            {
                ImGui::Separator();

                // Options menu

                if (ImGui::Button("Options"))
                    ImGui::OpenPopup("context");
                if (ImGui::BeginPopup("context"))
                {
                    ImGui::PushItemWidth(56);
                    if (ImGui::DragInt("##cols", &Cols, 0.2f, 4, 32, "%d cols")) { ContentsWidthChanged = true; }
                    ImGui::PopItemWidth();
                    ImGui::Checkbox("Show Data Preview", &next_show_data_preview);
                    ImGui::Checkbox("Show HexII", &OptShowHexII);
                    if (ImGui::Checkbox("Show Ascii", &OptShowAscii)) { ContentsWidthChanged = true; }
                    ImGui::Checkbox("Grey out zeroes", &OptGreyOutZeroes);
                    ImGui::Checkbox("Uppercase Hex", &OptUpperCaseHex);

                    ImGui::EndPopup();
                }

                ImGui::SameLine();
                ImGui::Text(format_range, s.AddrDigitsCount, base_display_addr, s.AddrDigitsCount, base_display_addr + mem_size - 1);
                ImGui::SameLine();
                ImGui::PushItemWidth((s.AddrDigitsCount + 1) * s.GlyphWidth + style.FramePadding.x* 2.0f);
                if (ImGui::InputText("##addr", AddrInputBuf, 32, ImGuiInputTextFlags_CharsHexadecimal | ImGuiInputTextFlags_EnterReturnsTrue))
                {
                    size_t goto_addr;
                    if (sscanf(AddrInputBuf, "%" _PRISizeT "X", &goto_addr) == 1)
                    {
                        GotoAddr = goto_addr - base_display_addr;
                        HighlightMin = 3 = (size_t)-1;
                    }
                }
                ImGui::PopItemWidth();

                if (GotoAddr != (size_t)-1)
                {
                    if (GotoAddr<mem_size)
                    {
                        ImGui::BeginChild("##scrolling");
                        ImGui::SetScrollFromPosY(ImGui::GetCursorStartPos().y + (GotoAddr / Cols) * ImGui::GetTextLineHeight());
                        ImGui::EndChild();
                        DataEditingAddr = DataPreviewAddr = GotoAddr;
                        DataEditingTakeFocus = true;
                    }
                    GotoAddr = (size_t)-1;
                }
            }

            if (OptShowDataPreview)
            {
                ImGui::Separator();
                ImGui::AlignTextToFramePadding();
                ImGui::Text("Preview as:");
                ImGui::SameLine();
                ImGui::PushItemWidth((s.GlyphWidth* 10.0f) + style.FramePadding.x* 2.0f + style.ItemInnerSpacing.x);
                if (ImGui::BeginCombo("##combo_type", DataTypeGetDesc(PreviewDataType), ImGuiComboFlags_HeightLargest))
                {
                    for (int n = 0; n<DataType_COUNT; n++)
                        if (ImGui::Selectable(DataTypeGetDesc((DataType) n), PreviewDataType == n))
                            PreviewDataType = (DataType) n;
        ImGui::EndCombo();
                }
                ImGui::PopItemWidth();
                ImGui::SameLine();
                ImGui::PushItemWidth((s.GlyphWidth* 6.0f) + style.FramePadding.x* 2.0f + style.ItemInnerSpacing.x);
                ImGui::Combo("##combo_endianess", &PreviewEndianess, "LE\0BE\0\0");
                ImGui::PopItemWidth();

                char buf[128];
        float x = s.GlyphWidth * 6.0f;
        bool has_value = DataPreviewAddr != (size_t) - 1;
                if (has_value)
                    DisplayPreviewData(DataPreviewAddr, mem_data, mem_size, PreviewDataType, DataFormat_Dec, buf, (size_t) IM_ARRAYSIZE(buf));
        ImGui::Text("Dec"); ImGui::SameLine(x); ImGui::TextUnformatted(has_value? buf : "N/A");
                if (has_value)
                    DisplayPreviewData(DataPreviewAddr, mem_data, mem_size, PreviewDataType, DataFormat_Hex, buf, (size_t) IM_ARRAYSIZE(buf));
        ImGui::Text("Hex"); ImGui::SameLine(x); ImGui::TextUnformatted(has_value? buf : "N/A");
                if (has_value)
                    DisplayPreviewData(DataPreviewAddr, mem_data, mem_size, PreviewDataType, DataFormat_Bin, buf, (size_t) IM_ARRAYSIZE(buf));
        ImGui::Text("Bin"); ImGui::SameLine(x); ImGui::TextUnformatted(has_value? buf : "N/A");
            }

            OptShowDataPreview = next_show_data_preview;

            // Notify the main window of our ideal child content size (FIXME: we are missing an API to get the contents size from the child)
            ImGui::SetCursorPosX(s.WindowWidth);*/
        }

        // Utilities for Data Preview
        private string DataTypeGetDesc(DataType data_type)
        {
            string[] descs =
                {"Int8", "Uint8", "Int16", "Uint16", "Int32", "Uint32", "Int64", "Uint64", "Float", "Double"};
            return descs[(int) data_type];
        }

        private int DataTypeGetSize(DataType data_type)
        {
            int[] sizes = {1, 1, 2, 2, 4, 4, 8, 8, 4, 8};
            return sizes[(int) data_type];
        }

        private string DataFormatGetDesc(DataFormat data_format)
        {
            string[] descs = {"Bin", "Dec", "Hex"};
            return descs[(int) data_format];
        }

        private bool IsBigEndian()
        {
            return !BitConverter.IsLittleEndian;
        }

        private T EndianessCopy<T>(ref byte[] src, int size)
        {
            return (T) Convert.ChangeType(src, typeof(T));
        }


        private string FormatBinary(byte[] buf, int width)
        {
            int out_n = 0;
            StringBuilder out_buf = new StringBuilder(64 + 8 + 1);
            for (int j = 0, n = width / 8; j < n; ++j)
            {
                for (int i = 0; i < 8; ++i)
                {
                    out_buf[out_n++] = (buf[j] & (1 << (7 - i))) == 1 ? '1' : '0';
                }

                out_buf[out_n++] = ' ';
            }

            return out_buf.ToString();
        }

        private string DisplayPreviewData(int addr, byte[] mem_data, int mem_size, DataType data_type, DataFormat data_format)
        {
            byte[] buf = new byte[8];
            int elem_size = DataTypeGetSize(data_type);
            int size = addr + elem_size > mem_size ? mem_size - addr : elem_size;

            Array.Copy(mem_data, addr, buf, 0, size);

            if (data_format == DataFormat.DataFormat_Bin)
            {
                return FormatBinary(buf, (int) size * 8);
            }

            switch (data_type)
            {
                case DataType.DataType_S8:
                {
                    char int8 = EndianessCopy<char>(ref buf, size);
                    if (data_format == DataFormat.DataFormat_Dec)
                    {
                        return $"{int8:D2}";
                    }

                    if (data_format == DataFormat.DataFormat_Hex)
                    {
                        return $"{int8 & 0xFF:X2}";
                    }

                    break;
                }
                case DataType.DataType_U8:
                {
                    byte uint8 = EndianessCopy<byte>(ref buf, size);
                    if (data_format == DataFormat.DataFormat_Dec)
                    {
                        return $"{uint8:D2}";
                    }

                    if (data_format == DataFormat.DataFormat_Hex)
                    {
                        return $"{uint8 & 0XFF:X2}";
                    }

                    break;
                }
                case DataType.DataType_S16:
                {
                    short int16 = EndianessCopy<short>(ref buf, size);
                    if (data_format == DataFormat.DataFormat_Dec)
                    {
                        return $"{int16:D4}";
                    }

                    if (data_format == DataFormat.DataFormat_Hex)
                    {
                        return $"{int16 & 0xFFFF:X4}";
                    }

                    break;
                }
                case DataType.DataType_U16:
                {
                    ushort uint16 = EndianessCopy<ushort>(ref buf, size);
                    if (data_format == DataFormat.DataFormat_Dec)
                    {
                        return $"{uint16:D4}";
                    }

                    if (data_format == DataFormat.DataFormat_Hex)
                    {
                        return $"{uint16 & 0xFFFF:X4}";
                    }

                    break;
                }
                case DataType.DataType_S32:
                {
                    int int32 = EndianessCopy<int>(ref buf, size);
                    if (data_format == DataFormat.DataFormat_Dec)
                    {
                        return $"{int32:D8}";
                    }

                    if (data_format == DataFormat.DataFormat_Hex)
                    {
                        return $"{int32 & 0xFFFF:X8}";
                    }

                    break;
                }
                case DataType.DataType_U32:
                {
                    uint uint32 = EndianessCopy<uint>(ref buf, size);
                    if (data_format == DataFormat.DataFormat_Dec)
                    {
                        return $"{uint32:D8}";
                    }

                    if (data_format == DataFormat.DataFormat_Hex)
                    {
                        return $"{uint32 & 0xFFFF:X8}";
                    }

                    break;
                }
                case DataType.DataType_S64:
                {
                    long int64 = EndianessCopy<long>(ref buf, size);
                    if (data_format == DataFormat.DataFormat_Dec)
                    {
                        return $"{int64:D16}";
                    }

                    if (data_format == DataFormat.DataFormat_Hex)
                    {
                        return $"{int64:X16}";
                    }

                    break;
                }
                case DataType.DataType_U64:
                {
                    ulong uint64 = EndianessCopy<ulong>(ref buf, size);
                    if (data_format == DataFormat.DataFormat_Dec)
                    {
                        return $"{uint64:D16}";
                    }

                    if (data_format == DataFormat.DataFormat_Hex)
                    {
                        return $"{uint64:X16}";
                    }

                    break;
                }
                case DataType.DataType_Float:
                {
                    float float32 = EndianessCopy<float>(ref buf, size);
                    if (data_format == DataFormat.DataFormat_Dec)
                    {
                        return $"{float32:F}";
                    }

                    if (data_format == DataFormat.DataFormat_Hex)
                    {
                        return $"{float32:G}";
                    }

                    break;
                }
                case DataType.DataType_Double:
                {
                    double float64 = EndianessCopy<double>(ref buf, size);
                    if (data_format == DataFormat.DataFormat_Dec)
                    {
                        return $"{float64:F}";
                    }

                    if (data_format == DataFormat.DataFormat_Hex)
                    {
                        return $"{float64:G}";
                    }
                }
                    break;
            }

            return string.Empty;
        }
    }
}