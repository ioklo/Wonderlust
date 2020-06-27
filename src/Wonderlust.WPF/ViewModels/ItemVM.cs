using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.WPF.ViewModels
{
    public class ItemVM : DependencyObject
    {
        IWorkspaceItem? item;
        public string DisplayName { get; set; }
        public Brush Brush { get; set; }

        public ItemVM()
        {
            item = null;
            DisplayName = "Notepad.exe";
            Brush = new SolidColorBrush(Colors.LightGray);
        }

        public ItemVM(IWorkspaceItem item, Brush brush)
        {
            this.item = item;
            DisplayName = item.DisplayName.ToUpper();
            Brush = brush;
        }

        public void Exec()
        {
            item?.Exec();
        }
    }
}
