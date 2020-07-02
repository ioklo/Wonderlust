using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wonderlust.Core.Abstractions;
using Wonderlust.WPF.ViewModels;

namespace Wonderlust.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowVM? ViewModel;

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = null;
        }

        public MainWindow(MainWindowVM viewModel)
            : this()
        {
            ViewModel = viewModel;
            DataContext = viewModel;

            viewModel.OnContainerChanged += ViewModel_OnContainerChanged;

            // Bind가 끝난 시점
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {   
            UpdateFocus();
        }

        private void UpdateFocus()
        {
            if (ViewModel == null || ViewModel.InitialSelectedItem == null) return;

            ListBox.SelectedItem = ViewModel.InitialSelectedItem;
            ListBox.ScrollIntoView(ViewModel.InitialSelectedItem);
            ListBox.UpdateLayout();

            // 아이템에 포커스를 주어야 키보드로 이동할 수 있다
            if (ListBox.SelectedItem != null)
            {
                var cont = ListBox.ItemContainerGenerator.ContainerFromItem(ListBox.SelectedItem);

                if (cont is FrameworkElement elem)
                {
                    Keyboard.Focus(elem);
                }
            }
        }

        private void ViewModel_OnContainerChanged()
        {
            Debug.Assert(ViewModel != null);

            UpdateFocus();
        }        

    }
}
