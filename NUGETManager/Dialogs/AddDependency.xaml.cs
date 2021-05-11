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
    /// Interaction logic for AddDependency.xaml
    /// </summary>
    public partial class AddDependency : Window
    {
        public AddDependency(NUGETDependency dependency)
        {
            InitializeComponent();

            DataContext = dependency;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
