C# WinForms .NET Framework 4.8 EXE для Autodesk Inventor.

Это НЕ add-in DLL. Это отдельное .exe приложение:
.exe -> Inventor.Application -> активный документ Inventor.

Что добавлено в этой версии:

1) Вкладка .ipt:
   - список тел в группе;
   - список элементов/Features, связанных с выбранными телами;
   - копирование списка тел;
   - копирование списка элементов;
   - удаление выбранных строк Delete или кнопкой Remove selected;
   - очистка всех списков/групп;
   - отдельная очистка только списка элементов.

2) Как формируется список элементов:
   - при добавлении SurfaceBody программа пытается найти CreatedByFeature у тела;
   - дополнительно проходит по граням этого тела и собирает CreatedByFeature у граней;
   - найденные элементы получают атрибут myFeatureGroup и показываются во втором списке.

Если у какого-то тела Inventor API не отдаёт CreatedByFeature, оно может попасть в список тел, но не дать строку в списке элементов.

3) Вкладка .iam:
   - оставлен прежний код Autodesk Lesson 5 для компонентов сборки.

Если ссылка Autodesk.Inventor.Interop сломана:
References -> удалить битую ссылку Autodesk.Inventor.Interop -> Add Reference -> Browse -> выбрать DLL из вашей папки Inventor.

Для Inventor 2019 путь обычно:
C:\Program Files\Autodesk\Inventor 2019\Bin\Public Assemblies\Autodesk.Inventor.Interop.dll

Параметры ссылки:
Embed Interop Types = False
Copy Local = False

Платформа проекта:
x64


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
