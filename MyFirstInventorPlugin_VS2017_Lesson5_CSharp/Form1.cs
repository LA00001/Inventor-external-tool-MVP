// (C) Copyright 2011 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software
// in object code form for any purpose and without fee is hereby
// granted, provided that the above copyright notice appears in
// all copies and that both that copyright notice and the limited
// warranty and restricted rights notice below appear in all
// supporting documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK,
// INC. DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL
// BE UNINTERRUPTED OR ERROR FREE.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Inventor;

namespace MyFirstInventorPlugin_VS2017_Lesson5_CSharp
{
    public partial class Form1 : Form
    {
        private Inventor.Application _invApp;
        private bool _started;

        // These fields MUST stay alive while Inventor is waiting for the user's window selection.
        // If they are local variables, COM events can be garbage-collected and OnSelect will not fire.
        private InteractionEvents _iptWindowInteractionEvents;
        private SelectEvents _iptWindowSelectEvents;
        private bool _iptWindowSelectionRunning;
        private bool _iptWindowSelectionWasHandled;
        private bool _originalShowPromptTooltips;
        private FormWindowState _windowStateBeforeSelection;

        public Form1()
        {
            InitializeComponent();
            UpdateIptGroupListCaption();
            UpdateIptFeatureListCaption();

            try
            {
                _invApp = (Inventor.Application)Marshal.GetActiveObject("Inventor.Application");
            }
            catch (Exception)
            {
                try
                {
                    Type oInvAppType = Type.GetTypeFromProgID("Inventor.Application");

                    _invApp = (Inventor.Application)Activator.CreateInstance(oInvAppType);
                    _invApp.Visible = true;

                    // Note: if you shut down the Inventor session that was started
                    // this way there is still an Inventor.exe running. We use this
                    // Boolean to test whether or not the Inventor App will need to be shut down.
                    _started = true;
                }
                catch (Exception ex2)
                {
                    MessageBox.Show(ex2.ToString());
                    MessageBox.Show("Unable to get or start Inventor");
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopIptWindowSelection(false);

            if (_started && _invApp != null)
            {
                _invApp.Quit();
            }

            _invApp = null;
        }

        // ============================================================
        // .iam TAB
        // Original Lesson 5 code: works with assembly components.
        // ============================================================
        private void Button1_Click(object sender, EventArgs e)
        {
            if (!TryGetActiveAssemblyDocument(out AssemblyDocument asmDoc))
            {
                return;
            }

            if (asmDoc.SelectSet.Count == 0)
            {
                MessageBox.Show("Need to select a Part or Sub Assembly");
                return;
            }

            SelectSet selSet = asmDoc.SelectSet;

            try
            {
                foreach (object obj in selSet)
                {
                    ComponentOccurrence compOcc = (ComponentOccurrence)obj;
                    Debug.Print(compOcc.Name);
                    // compOcc.Visible = false;

                    AttributeSets attbSets = compOcc.AttributeSets;

                    // Add the attributes to the ComponentOccurrence.
                    // In VB this was: If Not attbSets.NameIsUsed("myPartGroup") Then
                    if (!AttributeSetNameIsUsed(attbSets, "myPartGroup"))
                    {
                        AttributeSet attbSet = attbSets.Add("myPartGroup");

                        Inventor.Attribute attb = attbSet.Add(
                            "PartGroup1",
                            ValueTypeEnum.kStringType,
                            "Group1");
                    }
                }

                MessageBox.Show("Selected assembly components were added to group.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Is the selected item a Component?");
                MessageBox.Show(ex.ToString());
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (!TryGetActiveAssemblyDocument(out AssemblyDocument asmDoc))
            {
                return;
            }

            try
            {
                AttributeManager attbMan = asmDoc.AttributeManager;

                ObjectCollection objsCol = attbMan.FindObjects(
                    "myPartGroup",
                    "PartGroup1",
                    "Group1");

                int count = 0;

                foreach (object obj in objsCol)
                {
                    ComponentOccurrence compOcc = (ComponentOccurrence)obj;

                    // Toggle the visibility of the Component Occurrence.
                    compOcc.Visible = !compOcc.Visible;
                    count++;
                }

                MessageBox.Show("Assembly components toggled: " + count.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem hiding component");
                MessageBox.Show(ex.ToString());
            }
        }

        // ============================================================
        // .ipt TAB
        // Works with part bodies. You can select a face or a body.
        // If a face is selected, the code adds/toggles the whole body of that face.
        // ============================================================
        private void ButtonIptAdd_Click(object sender, EventArgs e)
        {
            if (!TryGetActivePartDocument(out PartDocument partDoc))
            {
                return;
            }

            if (partDoc.SelectSet.Count == 0)
            {
                MessageBox.Show("Select a solid body or a face in the part");
                return;
            }

            try
            {
                int count = 0;

                foreach (object obj in partDoc.SelectSet)
                {
                    SurfaceBody body = GetSurfaceBodyFromSelectedObject(obj);

                    if (body == null)
                    {
                        continue;
                    }

                    AddBodyToGroup(body);
                    AddFeaturesForBodyToGroup(body);
                    count++;
                }

                if (count == 0)
                {
                    MessageBox.Show("No solid bodies found in the current selection. Select a face or a solid body.");
                    return;
                }

                RefreshIptGroupList(partDoc);
                RefreshIptFeatureList(partDoc);

                MessageBox.Show("Selected IPT bodies were added to group: " + count.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem adding selected IPT bodies to group");
                MessageBox.Show(ex.ToString());
            }
        }

        // New interactive workflow:
        // 1) Press button.
        // 2) User gets a prompt.
        // 3) User drags a window/frame with LMB in Inventor.
        // 4) OnSelect fires after LMB release.
        // 5) We build a RangeBox from the selected visible BODIES only.
        // 6) We scan ALL SurfaceBodies in the .ipt and add every body whose RangeBox intersects that area.
        // Important: do NOT add kPartFaceFilter here. It can return thousands of faces and make selection slow.
        private void ButtonIptAddInsideBox_Click(object sender, EventArgs e)
        {
            if (!TryGetActivePartDocument(out PartDocument partDoc))
            {
                return;
            }

            if (_iptWindowSelectionRunning)
            {
                MessageBox.Show("Window selection is already running. Press Esc in Inventor to cancel it.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Выберите ТЕЛА рамкой в окне Inventor.\r\n\r\n" +
                "1) Нажмите OK.\r\n" +
                "2) В Inventor зажмите ЛКМ.\r\n" +
                "3) Протяните рамку.\r\n" +
                "4) Отпустите ЛКМ.\r\n\r\n" +
                "После отпускания ЛКМ программа добавит в группу найденные тела, включая внутренние/скрытые по RangeBox.\r\n\r\n" +
                "Оптимизировано: выбираем только SurfaceBody, без граней.",
                "IPT window selection",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information);

            if (result != DialogResult.OK)
            {
                return;
            }

            try
            {
                partDoc.SelectSet.Clear();

                _iptWindowSelectionWasHandled = false;
                _iptWindowSelectionRunning = true;
                _windowStateBeforeSelection = this.WindowState;

                // Create Inventor's real interactive selection command.
                _iptWindowInteractionEvents = _invApp.CommandManager.CreateInteractionEvents();
                _iptWindowInteractionEvents.InteractionDisabled = false;
                _iptWindowInteractionEvents.StatusBarText = "Выберите рамкой ТЕЛА в .ipt. Отпустите ЛКМ для завершения. Esc - отмена.";
                _iptWindowInteractionEvents.OnTerminate += IptWindowInteractionEvents_OnTerminate;

                _iptWindowSelectEvents = _iptWindowInteractionEvents.SelectEvents;

                // FAST MODE: select only solid/surface bodies.
                // Do NOT add kPartFaceFilter here: on complex models it can return thousands of faces.
                // We only need the visible selected bodies to build a RangeBox, then we scan all bodies ourselves.
                TryAddSelectionFilter(_iptWindowSelectEvents, SelectionFilterEnum.kPartBodyFilter);

                // This is the key Inventor API flag that enables drag-window selection.
                _iptWindowSelectEvents.WindowSelectEnabled = true;

                // Do not force single select. Window-select can return several entities.
                try
                {
                    _iptWindowSelectEvents.SingleSelectEnabled = false;
                }
                catch
                {
                    // Some Interop versions may expose this differently; WindowSelectEnabled is the important part.
                }

                // These event handlers are intentionally not empty: without OnSelect/OnPreSelect,
                // some Inventor versions do not highlight/select reliably during InteractionEvents.
                _iptWindowSelectEvents.OnPreSelect += IptWindowSelectEvents_OnPreSelect;
                _iptWindowSelectEvents.OnSelect += IptWindowSelectEvents_OnSelect;

                _originalShowPromptTooltips = _invApp.GeneralOptions.ShowCommandPromptTooltips;
                _invApp.GeneralOptions.ShowCommandPromptTooltips = true;

                // Move our WinForms window away so it does not cover the Inventor view.
                this.WindowState = FormWindowState.Minimized;

                _iptWindowInteractionEvents.Start();
            }
            catch (Exception ex)
            {
                StopIptWindowSelection(true);
                MessageBox.Show("Problem starting IPT window selection");
                MessageBox.Show(ex.ToString());
            }
        }

        private void IptWindowSelectEvents_OnPreSelect(
            ref object PreSelectEntity,
            out bool DoHighlight,
            ref ObjectCollection MorePreSelectEntities,
            SelectionDeviceEnum SelectionDevice,
            Inventor.Point ModelPosition,
            Point2d ViewPosition,
            Inventor.View View)
        {
            // Fast mode: highlight/select only bodies, not faces.
            // Selecting faces can create thousands of selected entities and becomes slow on large parts.
            DoHighlight = PreSelectEntity is SurfaceBody;
        }

        private void IptWindowSelectEvents_OnSelect(
            ObjectsEnumerator JustSelectedEntities,
            SelectionDeviceEnum SelectionDevice,
            Inventor.Point ModelPosition,
            Point2d ViewPosition,
            Inventor.View View)
        {
            if (!_iptWindowSelectionRunning)
            {
                return;
            }

            try
            {
                if (!TryGetActivePartDocument(out PartDocument partDoc))
                {
                    StopIptWindowSelection(true);
                    return;
                }

                BoxLimits selectionLimits = new BoxLimits();
                HashSet<IntPtr> selectedBodyKeys = new HashSet<IntPtr>();
                int selectedBodyCount = 0;

                // These are only the visible/selectable BODIES returned by Inventor's real window selection.
                // We use them only to define the 3D RangeBox area.
                foreach (object obj in JustSelectedEntities)
                {
                    SurfaceBody selectedBody = obj as SurfaceBody;

                    if (selectedBody == null)
                    {
                        continue;
                    }

                    IntPtr key = GetComIdentityKey(selectedBody);

                    if (key != IntPtr.Zero && selectedBodyKeys.Contains(key))
                    {
                        continue;
                    }

                    if (key != IntPtr.Zero)
                    {
                        selectedBodyKeys.Add(key);
                    }

                    selectionLimits.Include(selectedBody.RangeBox);
                    selectedBodyCount++;
                }

                if (!selectionLimits.HasValue)
                {
                    _iptWindowSelectionWasHandled = true;
                    StopIptWindowSelection(true);
                    MessageBox.Show("Рамка не захватила ни одного тела. Попробуйте захватить хотя бы одно видимое тело.");
                    return;
                }

                partDoc.SelectSet.Clear();

                int addedCount = 0;
                int selectedInInventorCount = 0;

                foreach (SurfaceBody body in partDoc.ComponentDefinition.SurfaceBodies)
                {
                    if (body == null)
                    {
                        continue;
                    }

                    // Crossing-window behavior: every body whose RangeBox intersects the selected area is included.
                    // If you want only bodies fully inside the area, replace Intersects with Contains.
                    if (!selectionLimits.Intersects(body.RangeBox, 0.001))
                    {
                        continue;
                    }

                    AddBodyToGroup(body);
                    AddFeaturesForBodyToGroup(body);
                    addedCount++;

                    try
                    {
                        partDoc.SelectSet.Select(body);
                        selectedInInventorCount++;
                    }
                    catch
                    {
                        // Invisible/special bodies can be tagged by attributes but may not highlight in the Inventor UI.
                    }
                }

                // No partDoc.Update() here: adding attributes does not need model rebuild, and Update can be slow.
                RefreshIptGroupList(partDoc);
                RefreshIptFeatureList(partDoc);

                _iptWindowSelectionWasHandled = true;
                StopIptWindowSelection(true);

                MessageBox.Show(
                    "Объекты добавлены в группу.\r\n\r\n" +
                    "Видимых тел, попавших в рамку Inventor: " + selectedBodyCount.ToString() + "\r\n" +
                    "Всего тел добавлено по RangeBox, включая внутренние/скрытые: " + addedCount.ToString() + "\r\n" +
                    "Также выделено в UI Inventor: " + selectedInInventorCount.ToString());
            }
            catch (Exception ex)
            {
                _iptWindowSelectionWasHandled = true;
                StopIptWindowSelection(true);
                MessageBox.Show("Problem processing IPT window selection");
                MessageBox.Show(ex.ToString());
            }
        }

        private void IptWindowInteractionEvents_OnTerminate()
        {
            bool showCancelMessage = _iptWindowSelectionRunning && !_iptWindowSelectionWasHandled;

            CleanupIptWindowSelection(true);

            if (showCancelMessage)
            {
                MessageBox.Show("Выделение рамкой отменено.");
            }
        }

        private void StopIptWindowSelection(bool restoreForm)
        {
            if (_iptWindowInteractionEvents != null)
            {
                try
                {
                    _iptWindowInteractionEvents.Stop();
                }
                catch
                {
                    CleanupIptWindowSelection(restoreForm);
                }
            }
            else
            {
                CleanupIptWindowSelection(restoreForm);
            }
        }

        private void CleanupIptWindowSelection(bool restoreForm)
        {
            try
            {
                if (_invApp != null)
                {
                    _invApp.GeneralOptions.ShowCommandPromptTooltips = _originalShowPromptTooltips;
                }
            }
            catch
            {
            }

            if (_iptWindowSelectEvents != null)
            {
                try { _iptWindowSelectEvents.OnPreSelect -= IptWindowSelectEvents_OnPreSelect; } catch { }
                try { _iptWindowSelectEvents.OnSelect -= IptWindowSelectEvents_OnSelect; } catch { }
            }

            if (_iptWindowInteractionEvents != null)
            {
                try { _iptWindowInteractionEvents.OnTerminate -= IptWindowInteractionEvents_OnTerminate; } catch { }
            }

            _iptWindowSelectEvents = null;
            _iptWindowInteractionEvents = null;
            _iptWindowSelectionRunning = false;

            if (restoreForm)
            {
                RunOnUiThread(RestoreFormAfterSelection);
            }
        }

        private void RestoreFormAfterSelection()
        {
            if (this.IsDisposed)
            {
                return;
            }

            try
            {
                this.WindowState = _windowStateBeforeSelection;
                this.Activate();
            }
            catch
            {
            }
        }

        private void RunOnUiThread(Action action)
        {
            if (action == null || this.IsDisposed)
            {
                return;
            }

            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(action);
                    return;
                }

                action();
            }
            catch
            {
                // The form can be closing while Inventor COM events are still terminating.
            }
        }

        private void ButtonIptToggle_Click(object sender, EventArgs e)
        {
            if (!TryGetActivePartDocument(out PartDocument partDoc))
            {
                return;
            }

            try
            {
                AttributeManager attbMan = partDoc.AttributeManager;

                ObjectCollection objsCol = attbMan.FindObjects(
                    "myBodyGroup",
                    "BodyGroup1",
                    "Group1");

                int count = 0;

                foreach (object obj in objsCol)
                {
                    SurfaceBody body = obj as SurfaceBody;

                    if (body == null)
                    {
                        continue;
                    }

                    // Toggle the visibility of the SurfaceBody in a Part document.
                    body.Visible = !body.Visible;
                    count++;
                }

                partDoc.Update();
                RefreshIptGroupList(partDoc);
                RefreshIptFeatureList(partDoc);

                MessageBox.Show("IPT bodies toggled: " + count.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem hiding/showing IPT bodies");
                MessageBox.Show(ex.ToString());
            }
        }

        private bool TryGetActivePartDocument(out PartDocument partDoc)
        {
            partDoc = null;

            if (_invApp == null)
            {
                MessageBox.Show("Unable to get or start Inventor");
                return false;
            }

            if (_invApp.Documents.Count == 0)
            {
                MessageBox.Show("Need to open a Part document");
                return false;
            }

            if (_invApp.ActiveDocument.DocumentType != DocumentTypeEnum.kPartDocumentObject)
            {
                MessageBox.Show("Need to have a Part document active (.ipt)");
                return false;
            }

            partDoc = (PartDocument)_invApp.ActiveDocument;
            return true;
        }

        private bool TryGetActiveAssemblyDocument(out AssemblyDocument asmDoc)
        {
            asmDoc = null;

            if (_invApp == null)
            {
                MessageBox.Show("Unable to get or start Inventor");
                return false;
            }

            if (_invApp.Documents.Count == 0)
            {
                MessageBox.Show("Need to open an Assembly document");
                return false;
            }

            if (_invApp.ActiveDocument.DocumentType != DocumentTypeEnum.kAssemblyDocumentObject)
            {
                MessageBox.Show("Need to have an Assembly document active");
                return false;
            }

            asmDoc = (AssemblyDocument)_invApp.ActiveDocument;
            return true;
        }

        private static void TryAddSelectionFilter(SelectEvents selectEvents, SelectionFilterEnum filter)
        {
            try
            {
                selectEvents.AddSelectionFilter(filter);
            }
            catch
            {
                // Keep going. Different Inventor/Interop versions can behave differently with some filters.
            }
        }

        private static bool AddBodyToGroup(SurfaceBody body)
        {
            AttributeSets attbSets = body.AttributeSets;

            if (AttributeSetNameIsUsed(attbSets, "myBodyGroup"))
            {
                return false;
            }

            AttributeSet attbSet = attbSets.Add("myBodyGroup");

            Inventor.Attribute attb = attbSet.Add(
                "BodyGroup1",
                ValueTypeEnum.kStringType,
                "Group1");

            return true;
        }

        private static bool RemoveBodyFromGroup(SurfaceBody body)
        {
            if (body == null)
            {
                return false;
            }

            AttributeSets attbSets = body.AttributeSets;

            if (!AttributeSetNameIsUsed(attbSets, "myBodyGroup"))
            {
                return false;
            }

            try
            {
                dynamic sets = attbSets;
                AttributeSet attbSet = sets.Item("myBodyGroup");
                attbSet.Delete();
                return true;
            }
            catch
            {
                try
                {
                    // Fallback for Interop versions where Item is exposed as an indexer.
                    dynamic sets = attbSets;
                    AttributeSet attbSet = sets["myBodyGroup"];
                    attbSet.Delete();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        private void RefreshIptGroupList(PartDocument partDoc)
        {
            if (partDoc == null || this.IsDisposed)
            {
                return;
            }

            // Inventor InteractionEvents can call OnSelect from a COM thread that is not
            // the WinForms UI thread. All ListBox/Label changes must therefore be marshaled
            // back to the form thread, otherwise WinForms raises InvalidOperationException.
            if (this.InvokeRequired)
            {
                RunOnUiThread(delegate { RefreshIptGroupList(partDoc); });
                return;
            }

            bool beginUpdateStarted = false;

            try
            {
                listBoxIptBodies.BeginUpdate();
                beginUpdateStarted = true;
                listBoxIptBodies.Items.Clear();

                AttributeManager attbMan = partDoc.AttributeManager;
                ObjectCollection objsCol = attbMan.FindObjects(
                    "myBodyGroup",
                    "BodyGroup1",
                    "Group1");

                HashSet<IntPtr> bodyKeys = new HashSet<IntPtr>();
                int index = 1;

                foreach (object obj in objsCol)
                {
                    SurfaceBody body = obj as SurfaceBody;

                    if (body == null)
                    {
                        continue;
                    }

                    IntPtr key = GetComIdentityKey(body);

                    if (key != IntPtr.Zero && bodyKeys.Contains(key))
                    {
                        continue;
                    }

                    if (key != IntPtr.Zero)
                    {
                        bodyKeys.Add(key);
                    }

                    listBoxIptBodies.Items.Add(new BodyListItem(body, GetBodyDisplayName(body, index), key));
                    index++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem refreshing IPT body list");
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (beginUpdateStarted)
                {
                    listBoxIptBodies.EndUpdate();
                }

                UpdateIptGroupListCaption();
            }
        }

        private void UpdateIptGroupListCaption()
        {
            if (this.IsDisposed)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                RunOnUiThread(UpdateIptGroupListCaption);
                return;
            }

            labelIptGroupList.Text = "Bodies in current group / список тел в группе: " + listBoxIptBodies.Items.Count.ToString();
        }

        private static string GetBodyDisplayName(SurfaceBody body, int index)
        {
            string name = null;

            try
            {
                dynamic dynBody = body;
                name = dynBody.Name as string;
            }
            catch
            {
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                name = "SurfaceBody";
            }

            string visibilityText = string.Empty;

            try
            {
                visibilityText = body.Visible ? "" : " [hidden]";
            }
            catch
            {
            }

            return index.ToString("000") + " - " + name + visibilityText;
        }

        private void ButtonIptClearList_Click(object sender, EventArgs e)
        {
            if (!TryGetActivePartDocument(out PartDocument partDoc))
            {
                return;
            }

            DialogResult result = MessageBox.Show(
                "Очистить списки и удалить группы myBodyGroup / myFeatureGroup из текущей .ipt?",
                "Clear IPT lists and groups",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if (result != DialogResult.OK)
            {
                return;
            }

            try
            {
                int removedBodyCount = ClearGroupFromPartDocument(partDoc, "myBodyGroup", "BodyGroup1", "Group1");
                int removedFeatureCount = ClearGroupFromPartDocument(partDoc, "myFeatureGroup", "FeatureGroup1", "Group1");

                listBoxIptBodies.Items.Clear();
                listBoxIptFeatures.Items.Clear();
                UpdateIptGroupListCaption();
                UpdateIptFeatureListCaption();

                MessageBox.Show(
                    "Списки очищены.\r\n\r\n" +
                    "Удалено из группы тел: " + removedBodyCount.ToString() + "\r\n" +
                    "Удалено из группы элементов: " + removedFeatureCount.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem clearing IPT lists");
                MessageBox.Show(ex.ToString());
            }
        }

        private void ButtonIptCopyList_Click(object sender, EventArgs e)
        {
            if (listBoxIptBodies.Items.Count == 0)
            {
                MessageBox.Show("Список пустой.");
                return;
            }

            StringBuilder sb = new StringBuilder();

            foreach (object obj in listBoxIptBodies.Items)
            {
                sb.AppendLine(obj.ToString());
            }

            Clipboard.SetText(sb.ToString());
            MessageBox.Show("Список скопирован в буфер обмена: " + listBoxIptBodies.Items.Count.ToString());
        }

        private void ButtonIptCopyFeatureList_Click(object sender, EventArgs e)
        {
            if (listBoxIptFeatures.Items.Count == 0)
            {
                MessageBox.Show("Список элементов пустой.");
                return;
            }

            StringBuilder sb = new StringBuilder();

            foreach (object obj in listBoxIptFeatures.Items)
            {
                sb.AppendLine(obj.ToString());
            }

            Clipboard.SetText(sb.ToString());
            MessageBox.Show("Список элементов скопирован в буфер обмена: " + listBoxIptFeatures.Items.Count.ToString());
        }

        private void ButtonIptClearFeatureList_Click(object sender, EventArgs e)
        {
            if (!TryGetActivePartDocument(out PartDocument partDoc))
            {
                return;
            }

            DialogResult result = MessageBox.Show(
                "Очистить только список элементов и удалить группу myFeatureGroup?",
                "Clear IPT feature group",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if (result != DialogResult.OK)
            {
                return;
            }

            try
            {
                int removedFeatureCount = ClearGroupFromPartDocument(partDoc, "myFeatureGroup", "FeatureGroup1", "Group1");
                listBoxIptFeatures.Items.Clear();
                UpdateIptFeatureListCaption();
                MessageBox.Show("Список элементов очищен. Удалено из группы элементов: " + removedFeatureCount.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem clearing IPT feature list");
                MessageBox.Show(ex.ToString());
            }
        }

        private void ButtonIptRemoveSelectedFeature_Click(object sender, EventArgs e)
        {
            RemoveSelectedIptFeaturesFromListAndGroup();
        }

        private void listBoxIptFeatures_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                RemoveSelectedIptFeaturesFromListAndGroup();
                e.Handled = true;
            }
        }

        private void ButtonIptRemoveSelected_Click(object sender, EventArgs e)
        {
            RemoveSelectedIptBodiesFromListAndGroup();
        }

        private void listBoxIptBodies_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                RemoveSelectedIptBodiesFromListAndGroup();
                e.Handled = true;
            }
        }

        private void RemoveSelectedIptBodiesFromListAndGroup()
        {
            if (listBoxIptBodies.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите одну или несколько строк списка для удаления.");
                return;
            }

            List<BodyListItem> selectedItems = new List<BodyListItem>();

            foreach (object obj in listBoxIptBodies.SelectedItems)
            {
                BodyListItem item = obj as BodyListItem;

                if (item != null)
                {
                    selectedItems.Add(item);
                }
            }

            int removedCount = 0;

            foreach (BodyListItem item in selectedItems)
            {
                if (RemoveBodyFromGroup(item.Body))
                {
                    removedCount++;
                }

                listBoxIptBodies.Items.Remove(item);
            }

            UpdateIptGroupListCaption();
            MessageBox.Show("Удалено из списка и группы: " + removedCount.ToString());
        }

        private void RefreshIptFeatureList(PartDocument partDoc)
        {
            if (partDoc == null || this.IsDisposed)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                RunOnUiThread(delegate { RefreshIptFeatureList(partDoc); });
                return;
            }

            bool beginUpdateStarted = false;

            try
            {
                listBoxIptFeatures.BeginUpdate();
                beginUpdateStarted = true;
                listBoxIptFeatures.Items.Clear();

                AttributeManager attbMan = partDoc.AttributeManager;
                ObjectCollection objsCol = attbMan.FindObjects(
                    "myFeatureGroup",
                    "FeatureGroup1",
                    "Group1");

                HashSet<IntPtr> featureKeys = new HashSet<IntPtr>();
                int index = 1;

                foreach (object obj in objsCol)
                {
                    if (obj == null)
                    {
                        continue;
                    }

                    IntPtr key = GetComIdentityKey(obj);

                    if (key != IntPtr.Zero && featureKeys.Contains(key))
                    {
                        continue;
                    }

                    if (key != IntPtr.Zero)
                    {
                        featureKeys.Add(key);
                    }

                    listBoxIptFeatures.Items.Add(new FeatureListItem(obj, GetFeatureDisplayName(obj, index), key));
                    index++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem refreshing IPT feature list");
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (beginUpdateStarted)
                {
                    listBoxIptFeatures.EndUpdate();
                }

                UpdateIptFeatureListCaption();
            }
        }

        private void UpdateIptFeatureListCaption()
        {
            if (this.IsDisposed)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                RunOnUiThread(UpdateIptFeatureListCaption);
                return;
            }

            labelIptFeatureList.Text = "Features / элементы, связанные с телами: " + listBoxIptFeatures.Items.Count.ToString();
        }

        private static int AddFeaturesForBodyToGroup(SurfaceBody body)
        {
            if (body == null)
            {
                return 0;
            }

            List<object> features = GetFeaturesForBody(body);
            int added = 0;

            foreach (object feature in features)
            {
                if (AddFeatureToGroup(feature))
                {
                    added++;
                }
            }

            return added;
        }

        private static List<object> GetFeaturesForBody(SurfaceBody body)
        {
            List<object> result = new List<object>();
            HashSet<IntPtr> featureKeys = new HashSet<IntPtr>();

            Action<object> addFeature = delegate (object feature)
            {
                if (feature == null)
                {
                    return;
                }

                IntPtr key = GetComIdentityKey(feature);

                if (key != IntPtr.Zero && featureKeys.Contains(key))
                {
                    return;
                }

                if (key != IntPtr.Zero)
                {
                    featureKeys.Add(key);
                }

                result.Add(feature);
            };

            // Best case: some Inventor objects expose the feature that created the whole body.
            try
            {
                dynamic dynBody = body;
                addFeature(dynBody.CreatedByFeature);
            }
            catch
            {
            }

            // Reliable fallback: scan faces of the body and collect the features that created those faces.
            // This is done only for the grouped bodies, not for every face in the whole document.
            try
            {
                foreach (Face face in body.Faces)
                {
                    try
                    {
                        dynamic dynFace = face;
                        addFeature(dynFace.CreatedByFeature);
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }

            return result;
        }

        private static bool AddFeatureToGroup(object feature)
        {
            AttributeSets attbSets = GetAttributeSetsFromInventorObject(feature);

            if (attbSets == null)
            {
                return false;
            }

            if (AttributeSetNameIsUsed(attbSets, "myFeatureGroup"))
            {
                return false;
            }

            AttributeSet attbSet = attbSets.Add("myFeatureGroup");

            Inventor.Attribute attb = attbSet.Add(
                "FeatureGroup1",
                ValueTypeEnum.kStringType,
                "Group1");

            return true;
        }

        private static bool RemoveFeatureFromGroup(object feature)
        {
            return RemoveAttributeGroupFromInventorObject(feature, "myFeatureGroup");
        }

        private void RemoveSelectedIptFeaturesFromListAndGroup()
        {
            if (listBoxIptFeatures.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите одну или несколько строк списка элементов для удаления.");
                return;
            }

            List<FeatureListItem> selectedItems = new List<FeatureListItem>();

            foreach (object obj in listBoxIptFeatures.SelectedItems)
            {
                FeatureListItem item = obj as FeatureListItem;

                if (item != null)
                {
                    selectedItems.Add(item);
                }
            }

            int removedCount = 0;

            foreach (FeatureListItem item in selectedItems)
            {
                if (RemoveFeatureFromGroup(item.Feature))
                {
                    removedCount++;
                }

                listBoxIptFeatures.Items.Remove(item);
            }

            UpdateIptFeatureListCaption();
            MessageBox.Show("Удалено из списка элементов и группы: " + removedCount.ToString());
        }

        private static string GetFeatureDisplayName(object feature, int index)
        {
            string name = null;
            string typeText = null;
            string suppressedText = string.Empty;

            try
            {
                dynamic dynFeature = feature;
                name = dynFeature.Name as string;
            }
            catch
            {
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                name = "PartFeature";
            }

            try
            {
                dynamic dynFeature = feature;
                typeText = Convert.ToString(dynFeature.Type);
            }
            catch
            {
            }

            try
            {
                dynamic dynFeature = feature;
                bool suppressed = Convert.ToBoolean(dynFeature.Suppressed);
                suppressedText = suppressed ? " [suppressed]" : string.Empty;
            }
            catch
            {
            }

            if (!string.IsNullOrWhiteSpace(typeText))
            {
                return index.ToString("000") + " - " + name + " [" + typeText + "]" + suppressedText;
            }

            return index.ToString("000") + " - " + name + suppressedText;
        }

        private static AttributeSets GetAttributeSetsFromInventorObject(object inventorObject)
        {
            if (inventorObject == null)
            {
                return null;
            }

            try
            {
                dynamic dynObj = inventorObject;
                return (AttributeSets)dynObj.AttributeSets;
            }
            catch
            {
                return null;
            }
        }

        private static bool RemoveAttributeGroupFromInventorObject(object inventorObject, string setName)
        {
            if (inventorObject == null)
            {
                return false;
            }

            AttributeSets attbSets = GetAttributeSetsFromInventorObject(inventorObject);

            if (attbSets == null || !AttributeSetNameIsUsed(attbSets, setName))
            {
                return false;
            }

            try
            {
                dynamic sets = attbSets;
                AttributeSet attbSet = sets.Item(setName);
                attbSet.Delete();
                return true;
            }
            catch
            {
                try
                {
                    dynamic sets = attbSets;
                    AttributeSet attbSet = sets[setName];
                    attbSet.Delete();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        private static int ClearGroupFromPartDocument(PartDocument partDoc, string setName, string attributeName, string attributeValue)
        {
            AttributeManager attbMan = partDoc.AttributeManager;
            ObjectCollection objsCol = attbMan.FindObjects(setName, attributeName, attributeValue);

            List<object> objectsToRemove = new List<object>();

            foreach (object obj in objsCol)
            {
                if (obj != null)
                {
                    objectsToRemove.Add(obj);
                }
            }

            int removedCount = 0;

            foreach (object obj in objectsToRemove)
            {
                if (RemoveAttributeGroupFromInventorObject(obj, setName))
                {
                    removedCount++;
                }
            }

            return removedCount;
        }

        private class BodyListItem
        {
            public BodyListItem(SurfaceBody body, string displayName, IntPtr identityKey)
            {
                Body = body;
                DisplayName = displayName;
                IdentityKey = identityKey;
            }

            public SurfaceBody Body { get; private set; }
            public string DisplayName { get; private set; }
            public IntPtr IdentityKey { get; private set; }

            public override string ToString()
            {
                return DisplayName;
            }
        }

        private class FeatureListItem
        {
            public FeatureListItem(object feature, string displayName, IntPtr identityKey)
            {
                Feature = feature;
                DisplayName = displayName;
                IdentityKey = identityKey;
            }

            public object Feature { get; private set; }
            public string DisplayName { get; private set; }
            public IntPtr IdentityKey { get; private set; }

            public override string ToString()
            {
                return DisplayName;
            }
        }

        private class BoxLimits
        {
            public bool HasValue { get; private set; }

            private double _minX;
            private double _minY;
            private double _minZ;
            private double _maxX;
            private double _maxY;
            private double _maxZ;

            public void Include(Box box)
            {
                Point min = box.MinPoint;
                Point max = box.MaxPoint;

                if (!HasValue)
                {
                    _minX = min.X;
                    _minY = min.Y;
                    _minZ = min.Z;
                    _maxX = max.X;
                    _maxY = max.Y;
                    _maxZ = max.Z;
                    HasValue = true;
                    return;
                }

                _minX = Math.Min(_minX, min.X);
                _minY = Math.Min(_minY, min.Y);
                _minZ = Math.Min(_minZ, min.Z);
                _maxX = Math.Max(_maxX, max.X);
                _maxY = Math.Max(_maxY, max.Y);
                _maxZ = Math.Max(_maxZ, max.Z);
            }

            public bool Intersects(Box box, double tolerance)
            {
                Point min = box.MinPoint;
                Point max = box.MaxPoint;

                return !(max.X < _minX - tolerance || min.X > _maxX + tolerance ||
                         max.Y < _minY - tolerance || min.Y > _maxY + tolerance ||
                         max.Z < _minZ - tolerance || min.Z > _maxZ + tolerance);
            }

            public bool Contains(Box box, double tolerance)
            {
                Point min = box.MinPoint;
                Point max = box.MaxPoint;

                return min.X >= _minX - tolerance && max.X <= _maxX + tolerance &&
                       min.Y >= _minY - tolerance && max.Y <= _maxY + tolerance &&
                       min.Z >= _minZ - tolerance && max.Z <= _maxZ + tolerance;
            }
        }

        private static SurfaceBody GetSurfaceBodyFromSelectedObject(object selectedObject)
        {
            SurfaceBody body = selectedObject as SurfaceBody;
            if (body != null)
            {
                return body;
            }

            Face face = selectedObject as Face;
            if (face != null)
            {
                return face.SurfaceBody;
            }

            // Extra fallback for different Inventor Interop wrappers.
            // Some selected objects expose SurfaceBody but do not cast directly to Face here.
            try
            {
                dynamic dynObj = selectedObject;
                return dynObj.SurfaceBody as SurfaceBody;
            }
            catch
            {
                return null;
            }
        }

        private static IntPtr GetComIdentityKey(object comObject)
        {
            if (comObject == null)
            {
                return IntPtr.Zero;
            }

            IntPtr unknown = IntPtr.Zero;

            try
            {
                unknown = Marshal.GetIUnknownForObject(comObject);
                return unknown;
            }
            catch
            {
                return IntPtr.Zero;
            }
            finally
            {
                if (unknown != IntPtr.Zero)
                {
                    Marshal.Release(unknown);
                }
            }
        }

        private static bool AttributeSetNameIsUsed(AttributeSets attributeSets, string name)
        {
            // Late binding keeps this C# conversion compatible with Inventor Interop versions
            // where the VB call NameIsUsed("...") appears differently in C#.
            dynamic sets = attributeSets;
            return sets.NameIsUsed(name);
        }
    }
}
