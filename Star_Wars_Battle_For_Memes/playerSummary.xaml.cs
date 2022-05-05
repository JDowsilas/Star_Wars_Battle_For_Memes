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

namespace Star_Wars_Battle_For_Memes
{
    /// <summary>
    /// Interaction logic for playerSummary.xaml
    /// </summary>
    public partial class playerSummary : Window
    {
        public string userName;
        public playerSummary()
        {
            InitializeComponent();
            userName = "anonymous";
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            userName = userNameTextBox.Text;
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
