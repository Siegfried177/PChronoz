using System.Windows;
using PChronoz.ViewModels;

namespace PChronoz
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new ViewModelControl();
        }
    }
}
