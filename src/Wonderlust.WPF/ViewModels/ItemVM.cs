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
        public string SizeText { get; set; }
        public string DateText { get; set; }
        public Brush Brush { get; set; }
        public RelayCommand ExecuteCommand { get; }

        public ItemVM()
        {
            item = null;
            DisplayName = "Notepadaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa.exe";
            SizeText = "3,123,534";
            DateText = "20-03-24 11:88";
            Brush = new SolidColorBrush(Colors.LightGray);
            ExecuteCommand = new RelayCommand();
        }

        public ItemVM(IWorkspaceItem item, Brush brush)
        {
            this.item = item;
            DisplayName = item.DisplayName.ToUpper();
            SizeText = item.Size.HasValue ? item.Size.Value.ToString("N0") : string.Empty;
            DateText = item.DateTime.HasValue ? item.DateTime.Value.ToString("yyyy-MM-dd hh:mm") : string.Empty;
            Brush = brush;
            ExecuteCommand = new RelayCommand(Exec, () => true, true);
        }

        public void Exec()
        {
            item?.Exec();
        }
    }
}
