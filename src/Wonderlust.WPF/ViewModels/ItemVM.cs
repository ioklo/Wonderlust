using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.WPF.ViewModels
{
    public class ItemVM : DependencyObject
    { 
        public string DisplayName { get; set; }

        public ItemVM()
        {
            DisplayName = "Notepad.exe";
        }

        public ItemVM(IItem item)
        {
            DisplayName = item.Name;
        }
    }
}
