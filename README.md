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


## ИСТОРИЯ
### 1 ai-operations-ui-demo.zip — простой Blazor UI
### 2 topic-knowledge-base-demo.zip — справочник тем 101–115
### 3 docx-generator-demo.zip — отдельная генерация DOCX
### 4 workflow-pipeline-demo.zip — конвейер в консольном варианте
### 5 generated-files-api-demo.zip — API для выдачи файлов
### 6 ai-document-workflow-demo.zip — главный полный MVP с UI + конвейером
### 7 ai-document-workflow-devops-lab.zip — расширенная версия MVP с DevOps-обвязкой: health-check endpoints, Docker, Jenkinsfile, GitHub Actions CI и примеры AI infrastructure lab
### 8 ai-infra-kafka-s3-vault-dropapp-lab.zip — отдельный инфраструктурный lab: ASP.NET Core API + Redpanda/Kafka-compatible broker + MinIO/S3-compatible storage + HashiCorp Vault + dropapp-style manifest
### 9 ai-document-workflow-kafka-s3-vault-lab.zip — связка логики AI Document Workflow из проекта 6 с инфраструктурой Kafka/S3/Vault
### 10 ai-document-workflow-devops-ops-lab.zip — DevOps/Ops версия под Jenkins, Ansible, Groovy, Kafka, S3, Vault, Istio, dropapp-style, ИФТ/ПСИ и production-сопровождение
### 11 ai-document-workflow-observability-support-lab.zip — observability/support версия под сопровождение ПО: Prometheus, Grafana, Alertmanager, /metrics, healthz/readyz, readiness-проверки Kafka/S3/Vault, dashboard panels, alert rules, incident snapshot и runbook для диагностики инцидентов
### 12 ai-document-workflow-postgresql-support-lab.zip — PostgreSQL/support версия под сопровождение БД: PostgreSQL 16, Adminer, SQL schema/seed scripts, диагностические SQL-запросы, JOIN/GROUP BY/CTE, индексы, VIEW, PL/pgSQL functions, document health, incident diagnostics, release health и API endpoints для проверки состояния данных
### 13 ai-document-workflow-mvp-postgresql-support-lab.zip — гибрид проекта 6 и 12: главный MVP с UI + document workflow pipeline, объединённый с PostgreSQL/support-слоем для сопровождения БД: Docker Compose, PostgreSQL 16, Adminer, SQL schema/seed scripts, индексы, VIEW, PL/pgSQL functions, diagnostic SQL queries, document health, incident diagnostics, release health, /healthz, /readyz, /metrics, Prometheus, Grafana и Alertmanager
### 14 ai-document-workflow-business-data-analytics-lab.zip — Аналитический Blazor UI lab под роль аналитика: Visual Studio solution, запуск сценария анализа городских обращений, KPI по SLA/категориям/районам, BPMN/UML, business requirements, ТЗ, API/OpenAPI, SQL, Python ETL, dashboard spec и ML data preparation.
### 15-mortgage-vba-excel-sql-dashboard-lab.v2-blazor-ui-sln.zip — Excel/VBA/SQL lab под автоматизацию ипотечной отчётности: Visual Studio solution, Blazor UI для запуска сценария, ипотечный калькулятор, импорт CSV/SQL-выгрузок, VBA-модули, расчёт платежей/PTI/LTV, dashboard, data quality checks и регулярные отчёты.
### 16-ai-document-workflow-itil-change-management-lab.v1-html-ui-sln — ITIL/ITSM Change Management lab под инженера внедрения: Visual Studio solution, HTML UI с кнопкой формирования календаря изменений, ЗНИ/RFC, оценка рисков, поиск конфликтов, CAB checklist, планы внедрения и отката, связь с incident/problem management, KPI и SQL-отчётность.
### 17-ai-document-workflow-itsm-incident-analytics-lab.v2-blazor-ui-sln — ITSM Incident Analytics lab под роль аналитика в ITSM-системе: Blazor UI для анализа технологических инцидентов, KPI по SLA/MTTR/impact/root cause, контроль качества данных ITSM, аудит мероприятий, SQL-отчётность, RCA, dashboard spec и management report.
### 18-ml-model-validation-classic-ml-llm-lab.v5-russian-ui-comments.zip — Classic ML / LLM validation DS lab: Python pandas/sklearn, baseline logistic regression, challenger gradient boosting, ROC-AUC/Gini/KS/F1, backtest по историческим периодам, PSI/drift monitoring, LLM evaluation, generated CSV reports и Blazor UI-витрина.
### 19-Inventor-external-tool-MVP — Внешнее WinForms-приложение на .NET Framework для Autodesk Inventor. Поддерживает выбор тел в IPT рамкой, поиск внутренних и скрытых тел через RangeBox, редактируемые списки тел и элементов, копирование списков в буфер обмена, а также группировку компонентов в IAM-сборках.
### 20-Inventor-ipt-organizer — Внешний WinForms-инструмент для Autodesk Inventor: выбор тел IPT рамкой, поиск внутренних/скрытых тел через RangeBox, редактируемые списки объектов и создание папок в дереве Inventor.





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

