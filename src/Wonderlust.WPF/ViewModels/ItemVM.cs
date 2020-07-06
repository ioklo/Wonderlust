using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.WPF.ViewModels
{
    public class ItemVM
    {
        public IWorkspaceItem? Item { get; }
        public string DisplayName { get; set; }
        public string SizeText { get; set; }
        public string DateText { get; set; }
        public Brush Brush { get; set; }

        public ItemVM()
        {
            Item = null;
            DisplayName = "Notepadaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa.exe";
            SizeText = "3,123,534";
            DateText = "20-03-24 11:88";
            Brush = new SolidColorBrush(Colors.LightGray);
        }

        public ItemVM(IWorkspaceItem item, Brush brush)
        {
            Item = item;
            DisplayName = item.DisplayName.ToUpper();
            SizeText = item.Size.HasValue ? item.Size.Value.ToString("N0") : string.Empty;
            DateText = item.DateTime.HasValue ? item.DateTime.Value.ToString("yyyy-MM-dd hh:mm") : string.Empty;
            Brush = brush;
        }
    }
}
