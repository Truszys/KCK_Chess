using Logic;
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

namespace UIApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Settings settings = new();
        private FrontSettings frontSettings = new();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_start_game_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            var window = new game(Board.CreateNew(), settings, frontSettings);
            window.Show();
            window.Closed += gameClosed;
        }

        private void btn_quit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void gameClosed(object sender, EventArgs e)
        {
            Show();
        }
    }
}
