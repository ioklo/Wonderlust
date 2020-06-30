using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wonderlust.WPF.Views
{
    /// <summary>
    /// ItemView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ItemView : UserControl
    {
        public ItemView()
        {
            InitializeComponent();
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            return base.ArrangeOverride(arrangeBounds);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }
    }
}
