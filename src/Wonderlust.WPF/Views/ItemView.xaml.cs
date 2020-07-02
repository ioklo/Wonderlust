using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            
        }

        static bool bFirst = true;
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (!bFirst) return;

            bFirst = false;
            var depObj = VisualTreeHelper.GetParent(this);
            while (depObj != null)
            {
                if (depObj is UIElement uiElem)
                {
                    uiElem.KeyDown += UiElem_KeyDown;
                    uiElem.PreviewKeyDown += UiElem_PreviewKeyDown;
                }

                depObj = VisualTreeHelper.GetParent(depObj);
            }
        }

        private void UiElem_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("[PRE] {0}, {1}, {2}, {3}", sender, e.Key, e.SystemKey, e.Handled);
        }

        private void UiElem_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("[ACTUAL] {0}, {1}, {2}, {3}", sender, e.Key, e.SystemKey, e.Handled);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            
        }
    }
}
