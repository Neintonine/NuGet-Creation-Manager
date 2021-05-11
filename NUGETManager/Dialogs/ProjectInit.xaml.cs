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

namespace NUGETManager.Dialogs
{
    /// <summary>
    /// Interaction logic for ProjectInit.xaml
    /// </summary>
    public partial class ProjectInit : Window
    {
        public string Identifier;
        public string ProjectPath;

        public ProjectInit()
        {
            InitializeComponent();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (IdentifierBox.Text == "")
            {
                IdentifierBox.Background = Brushes.Red;
                return;
            } 

            if (Path.Path == "")
            {
                Path.Background = Brushes.Red;
                return;
            }

            ProjectPath = Path.Path;
            Identifier = IdentifierBox.Text;

            DialogResult = true;

        }
    }
}
