# LiteGui

## Introduction

Immediate Mode GUI from scratch based on .Net Standard 2.0

Currently only bindings WinForm

Plan more platforms in the future

----

## Plan

1. [x] Text
2. [x] Button
3. [x] Frame
    1. [x] Clipping
    2. [x] Scrollbar
    3. [ ] Drag
    4. [ ] Title
    5. [ ] Sizable
4. [x] Group
5. [x] InputText
    1. [x] Input Filter
    2. [x] Undo/Redo
    3. [x] Selection
    4. [x] Copy/Paste/Cut
    5. [x] Single/Mulit
    6. [ ] History
6. [x] Slider
7. [x] Separator
8. [x] CheckBox
9. [x] Texture
10. [x] RadioButton
11. [x] ToolTips
12. [ ] Menu
13. [ ] DragInput
14. [ ] Layout
    1. [x] Vertical
    2. [x] Horizontal
    3. [ ] Custom
15. [x] SameLine/NextLine
16. [x] ListBox
17. [ ] Tree
18. [ ] FreeType
19. [x] ColorPikcer
20. [x] Popup
21. [x] Selectable
22. [x] Combox
23. [x] Window

## Usage

Use LGui.XXX() between LGui.Begin() and LGui.End() The next examples to omit this part and variable define

Code
```c#
LGui.Text("Lite Gui {0}", Value);
if (LGui.Button("Click Me"))
{

}
LGui.Slider("SliderV", ref Value, 0, 10, 1);
```

Result
![Usage1](https://github.com/UnSkyToo/LiteGui/blob/master/Docs/Images/LiteGui_Usage_01.png)

Code
```c#
if (LGui.ColorButton("ColorButton1", Color, new LGuiVec2(30, 30)))
{
    LGui.OpenPopup("Popup 1");
}

if (LGui.BeginPopup("Popup 1", new LGuiVec2(260, 310)))
{
    LGui.ColorPicker("Color Picker 1", ref Color);
    LGui.EndPopup();
}

LGui.ListBox("ListBox", ref ItemIndex, Items, new LGuiVec2(100, 150));
LGui.Text("Current Item : {0}", Items[ItemIndex]);
```

Result
![Usage2](https://github.com/UnSkyToo/LiteGui/blob/master/Docs/Images/LiteGui_Usage_02.gif)

MemoryEditor & Window
![Usage3](https://github.com/UnSkyToo/LiteGui/blob/master/Docs/Images/LiteGui_Usage_03.gif)

Demo
![UsageWhole](https://github.com/UnSkyToo/LiteGui/blob/master/Docs/Images/LiteGui_Usage_Whole.gif)