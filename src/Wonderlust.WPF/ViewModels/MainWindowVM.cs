using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using Wonderlust.Core;
using Wonderlust.Core.Abstractions;
using Wonderlust.WPF.Views;

namespace Wonderlust.WPF.ViewModels
{
    public class MainWindowVM
    {
        public ObservableCollection<ItemVM> Items { get; private set; }

        // design-time data
        public MainWindowVM()
        {
            Items = new ObservableCollection<ItemVM>();
            for(int i = 0; i < 20; i++)
                Items.Add(new ItemVM());
        }

        public MainWindowVM(IWorkspace workspace)
        {
            Items = new ObservableCollection<ItemVM>(workspace.GetItems().Select(item => new ItemVM(item)));
        }
    }
}
