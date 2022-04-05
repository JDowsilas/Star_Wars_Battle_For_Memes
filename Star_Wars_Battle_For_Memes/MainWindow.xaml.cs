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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Star_Wars_Battle_For_Memes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }





        private void OnKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Left)
            {
                //moveLeft = true;
            }
            if (e.Key == Key.Right)
            {
                //moveRight = true;
            }

        }
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                //moveLeft = false;
            }
            if (e.Key == Key.Right)
            {
                //moveRight = false;
            }
        }
    }
}
