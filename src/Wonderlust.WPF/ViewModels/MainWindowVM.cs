using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        //public RelayCommand MoveToParentContainerCmd { get; }

        public event Action? OnContainerChanged;

        // design-time data
        public MainWindowVM()
        {
            Items = new ObservableCollection<ItemVM>();
            for(int i = 0; i < 20; i++)
                Items.Add(new ItemVM());

            InitialSelectedItem = Items[0];
            ExecuteCmd = RelayCommand.MakeEmpty<ItemVM>();
            //MoveToParentContainerCmd = new RelayCommand();
        }

        public MainWindowVM(IWorkspace workspace)
        {
            Items = new ObservableCollection<ItemVM>();
            ExecuteCmd = new RelayCommand<ItemVM>(ExecuteItem, CanExecuteItem, true);
            // MoveToParentContainerCmd = new RelayCommand(MoveToParentContainer, , true);

            this.workspace = workspace;
            workspace.OnContainerChanged += Workspace_OnContainerChanged;            

            UpdateItemsAndSelection();
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

        private bool CanExecuteItem(ItemVM itemVM)
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
    }
}
