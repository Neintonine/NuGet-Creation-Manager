using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using NUGETManager.Storage;

namespace NUGETManager.Dialogs
{
    /// <summary>
    /// Interaction logic for AddFramework.xaml
    /// </summary>
    public partial class AddFramework : Window
    {
        public NUGETFramework SelectedFramework;

        public AddFramework()
        {
            InitializeComponent();
            FrameworkSelection.ItemsSource = UserSettings.Settings.StoredFrameworks;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewFormular.Visibility = Visibility.Visible;
            NewFormular.DataContext = new NUGETFramework();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NUGETFramework framework = (NUGETFramework) NewFormular.DataContext;
            UserSettings.Settings.StoredFrameworks.Add(framework);
            UserSettings.Save();
            FrameworkSelection.SelectedItem = framework;

            NewFormular.Visibility = Visibility.Hidden;
            NewFormular.DataContext = null;

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedFramework = (NUGETFramework) FrameworkSelection.SelectedItem;
            DialogResult = true;
        }
    }
}
