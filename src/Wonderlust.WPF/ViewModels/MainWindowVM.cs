using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Wonderlust.Core;
using Wonderlust.Core.Abstractions;
using Wonderlust.WPF.Views;

namespace Wonderlust.WPF.ViewModels
{
    public class MainWindowVM
    {
        public ObservableCollection<ItemVM> Items { get; private set; }
        public int SelectedIndex { get; set; }
        public ItemVM? InitialSelectedItem { get; private set; }

        public event Action? OnContainerChanged;

        // design-time data
        public MainWindowVM()
        {
            Items = new ObservableCollection<ItemVM>();
            for(int i = 0; i < 20; i++)
                Items.Add(new ItemVM());

            InitialSelectedItem = Items[0];
        }

        public MainWindowVM(IWorkspace workspace)
        {
            Items = new ObservableCollection<ItemVM>(workspace.GetItems().Select(item => new ItemVM(item, new SolidColorBrush(Color.FromArgb(item.Color.A, item.Color.R, item.Color.G, item.Color.B)))));
            workspace.OnContainerChanged += Workspace_OnContainerChanged;
            InitialSelectedItem = 0 < Items.Count ? Items[0] : null;
        }

        private void Workspace_OnContainerChanged(IWorkspace workspace)
        {
            Items.Clear();

            InitialSelectedItem = null;
            foreach (var item in workspace.GetItems())
            {
                var itemVM = new ItemVM(item, new SolidColorBrush(Color.FromArgb(item.Color.A, item.Color.R, item.Color.G, item.Color.B)));
                Items.Add(itemVM);

                if (InitialSelectedItem == null && workspace.IsRelatedPrevContainer(item))
                    InitialSelectedItem = itemVM;
            }

            if (InitialSelectedItem == null && 0 < Items.Count)
                InitialSelectedItem = Items[0];

            OnContainerChanged?.Invoke();
        }
    }
}
