using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using Wonderlust.Core;
using Wonderlust.Core.Abstractions;
using Wonderlust.WPF.Views;

namespace Wonderlust.WPF.ViewModels
{
    public class MainWindowVM
    {
        IWorkspace? workspace;
        IList? selectedItems;

        public ObservableCollection<ItemVM> Items { get; private set; }
        public int SelectedIndex { get; set; }
        public ItemVM? InitialSelectedItem { get; private set; }                        
        public RelayCommand<string> ExecActionCmd { get; }
        public RelayCommand<string> SetContainerToDrivePathCmd { get; }
        public RelayCommand ShowPropertiesCmd { get; }
        
        public event Action? OnContainerChanged;
        public event Action? OnExitRequested;

        // design-time data
        public MainWindowVM()
        {
            Items = new ObservableCollection<ItemVM>();
            for(int i = 0; i < 20; i++)
                Items.Add(new ItemVM());

            InitialSelectedItem = Items[0];            
            ExecActionCmd = RelayCommand.MakeEmpty<string>();
            SetContainerToDrivePathCmd = RelayCommand.MakeEmpty<string>();
            ShowPropertiesCmd = new RelayCommand();
            selectedItems = null;
        }

        public MainWindowVM(IWorkspace workspace)
        {
            Items = new ObservableCollection<ItemVM>();            
            ExecActionCmd = new RelayCommand<string>(ExecAction, AlwaysCanExecute<string>, true);
            SetContainerToDrivePathCmd = new RelayCommand<string>(SetContainerToDrivePath, CanSetContainerToDrivePath, true);
            ShowPropertiesCmd = new RelayCommand(ShowProperties, () => true, true);

            this.workspace = workspace;            
            workspace.OnContainerChanged += Workspace_OnContainerChanged;
            selectedItems = null;

            UpdateItemsAndSelection();
            
        }

        private bool CanSetContainerToDrivePath(string drivePath)
        {
            return Directory.Exists(drivePath);
        }

        private void SetContainerToDrivePath(string drivePath)
        {
            Debug.Assert(workspace != null);
            workspace.SetContainer(new DriveContainer(drivePath, Directory.GetLastWriteTime(drivePath)), true);
        }

        public void UpdateSelectedItems(IList selectedItems)
        {
            this.selectedItems = selectedItems;
        }

        public void SetContainerToParent()
        {
            Debug.Assert(workspace != null);
            workspace.SetContainerToParent();
        }
        
        private void ExecAction(string actionName)
        {
            Debug.Assert(workspace != null);
            var workspaceItems = GetSelectedWorkspaceItems();

            if (actionName == "Default")
            {
                foreach (var item in workspaceItems)
                {
                    if (item is ContainerWorkspaceItem containerItem)
                    {
                        workspace.SetContainer(containerItem.Container, false);
                    }
                    else 
                    {
                        if (item.PhysicalPath != null)
                        {
                            try
                            {
                                var psi = new ProcessStartInfo();
                                psi.FileName = item.PhysicalPath;
                                psi.UseShellExecute = true;

                                Process.Start(psi);
                            }
                            catch
                            {

                            }
                        }
                    }
                }
            }
            else if (actionName == "View")
            {   
                var sb = new StringBuilder();
                bool bError = false, bFirst = true;
                foreach (var wi in workspaceItems)
                {
                    if (wi.PhysicalPath == null)
                    {
                        // TODO: 에러 전달 수단
                        bError = true;
                        break;
                    }

                    if (bFirst) bFirst = false;
                    else sb.Append(' ');
                    sb.Append($"\"{wi.PhysicalPath}\"");
                }

                if (!bError)
                {
                    try
                    {
                        var psi = new ProcessStartInfo();
                        psi.FileName = "code"; // TODO:                 
                        psi.Arguments = sb.ToString();
                        psi.UseShellExecute = true;

                        Process.Start(psi);
                    }
                    catch
                    {

                    }
                }
            }
        }

        private List<IWorkspaceItem> GetSelectedWorkspaceItems()
        {
            Debug.Assert(selectedItems != null);

            var workspaceItems = new List<IWorkspaceItem>(selectedItems.Count);
            foreach (var selectedItem in selectedItems)
            {
                var itemVM = selectedItem as ItemVM;

                if (itemVM == null) continue;
                if (itemVM.Item == null) continue;

                workspaceItems.Add(itemVM.Item);
            }

            return workspaceItems;
        }

        private bool AlwaysCanExecute<T>(T t)
        {
            return true;
        }

        private void UpdateItemsAndSelection()
        {
            Debug.Assert(workspace != null);

            Items.Clear();

            var (items, initialSelection) = workspace.GetItems();

            InitialSelectedItem = null;
            foreach (var item in items)
            {
                var color = item.Category.Color;

                var itemVM = new ItemVM(item, new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B)));
                Items.Add(itemVM);

                if (item == initialSelection)
                    InitialSelectedItem = itemVM;
            }

            if (InitialSelectedItem == null && 0 < Items.Count)
                InitialSelectedItem = Items[0];
        }

        private void Workspace_OnContainerChanged(IWorkspace workspace)
        {
            Debug.Assert(this.workspace == workspace);

            UpdateItemsAndSelection();

            OnContainerChanged?.Invoke();
        }

        public void RequestExit()
        {
            OnExitRequested?.Invoke();
        }

        public void ShowProperties()
        {
            var workspaceItems = GetSelectedWorkspaceItems();

            if (workspaceItems.Count == 0)
            {
                return;
            }
            else if (workspaceItems.Count == 1)
            {
                var item = workspaceItems[0];
                if (item.PhysicalPath != null)
                {
                    PropertyWindow.Open(item.PhysicalPath);
                }
            }
            else
            {
                if (0 < workspaceItems.Count(item => item.PhysicalPath == null))
                    return;

                PropertyWindow.Open(
                    workspaceItems.Select(item => item.PhysicalPath)!);
            }
        }        
    }
}
