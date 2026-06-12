# Inventor External Tool MVP

Windows Forms `.exe` utility for Autodesk Inventor.

This is **not** an Inventor add-in DLL. The application runs separately and connects to an open Inventor session through COM:

```text
WinForms .exe -> Inventor.Application -> active .ipt/.iam document
```

## Current MVP features

### `.ipt` tab

- Interactive frame/window selection in Inventor.
- Fast body-only selection, without selecting thousands of faces.
- Adds selected and inner/hidden `SurfaceBody` objects to a body group using `RangeBox` logic.
- Shows editable list of selected bodies.
- Shows editable list of related Inventor features/elements.
- Removes selected body/feature from the list.
- Clears all lists/groups.
- Copies body or feature list to clipboard.
- Toggles visibility of grouped bodies.

### `.iam` tab

- Keeps the original assembly workflow from the Autodesk Lesson 5 style example:
  - add selected assembly components to a group;
  - hide/show components in the group.

## Requirements

- Windows
- Visual Studio 2017 or newer
- .NET Framework 4.8 developer targeting pack
- Autodesk Inventor installed
- `Autodesk.Inventor.Interop.dll` referenced from your Inventor installation

## Inventor Interop reference

The project may contain a local HintPath such as:

```text
C:\Program Files\Autodesk\Inventor 2019\Bin\Public Assemblies\Autodesk.Inventor.Interop.dll
```

If the reference is broken on your machine:

1. Open the project in Visual Studio.
2. Remove the broken `Autodesk.Inventor.Interop` reference.
3. Add it again:
   `References -> Add Reference -> Browse`
4. Choose `Autodesk.Inventor.Interop.dll` from your Inventor installation folder.
5. Set reference properties:
   - `Embed Interop Types = False`
   - `Copy Local = False`

## Build settings

The project targets:

```text
.NET Framework 4.8
x64
WinForms EXE
```

Inventor is normally 64-bit, so keep the project platform as `x64`.

## Usage

1. Start Autodesk Inventor.
2. Open an `.ipt` or `.iam` file.
3. Start this external tool.
4. Use the matching tab:
   - `.ipt` for part bodies and related features;
   - `.iam` for assembly component occurrences.

## Notes

This MVP was developed as an external automation tool, not as an Inventor add-in. This keeps testing simple and avoids Inventor add-in registration during early development.

Do not commit Autodesk proprietary DLL files to the repository. Keep only source code and project files in Git.


Copyright (c) 2026 Андрей / LA00001

All rights reserved.

This repository is provided for portfolio and demonstration purposes only.
Copying, redistribution, modification, sublicensing, commercial use, or publication
of the source code is not permitted without prior written permission from the author.

---

Авторское право (c) 2026 Андрей / LA00001

Все права защищены.

Данный репозиторий предоставлен только для демонстрации в портфолио.
Копирование, распространение, изменение, сублицензирование, коммерческое использование
или публикация исходного кода не допускаются без предварительного письменного разрешения автора.

