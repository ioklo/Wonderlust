using System;
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

        public ObservableCollection<ItemVM> Items { get; private set; }
        public int SelectedIndex { get; set; }
        public ItemVM? InitialSelectedItem { get; private set; }                
        public RelayCommand<ItemVM> ExecuteCmd { get; }
        public RelayCommand<string> SetContainerToDrivePathCmd { get; }
        public RelayCommand<ItemVM> ShowPropertiesCmd { get; }

        public event Action? OnContainerChanged;
        public event Action? OnExitRequested;

        // design-time data
        public MainWindowVM()
        {
            Items = new ObservableCollection<ItemVM>();
            for(int i = 0; i < 20; i++)
                Items.Add(new ItemVM());

            InitialSelectedItem = Items[0];
            ExecuteCmd = RelayCommand.MakeEmpty<ItemVM>();
            SetContainerToDrivePathCmd = RelayCommand.MakeEmpty<string>();
            ShowPropertiesCmd = RelayCommand.MakeEmpty<ItemVM>();
        }

        public MainWindowVM(IWorkspace workspace)
        {
            Items = new ObservableCollection<ItemVM>();
            ExecuteCmd = new RelayCommand<ItemVM>(ExecuteItem, AlwaysCanExecute<ItemVM>, true);
            SetContainerToDrivePathCmd = new RelayCommand<string>(SetContainerToDrivePath, CanSetContainerToDrivePath, true);
            ShowPropertiesCmd = new RelayCommand<ItemVM>(ShowProperties, AlwaysCanExecute<ItemVM>, true);

            this.workspace = workspace;
            workspace.OnContainerChanged += Workspace_OnContainerChanged;            

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

        public void SetContainerToParent()
        {
            Debug.Assert(workspace != null);
            workspace.SetContainerToParent();
        }

        private void ExecuteItem(ItemVM itemVM)
        {
            itemVM.Exec();
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

        public void ShowProperties(ItemVM itemVM)
        {
            itemVM.ShowProperties();
        }        
    }
}
