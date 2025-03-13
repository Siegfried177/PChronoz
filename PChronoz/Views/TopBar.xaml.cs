using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PChronoz.Views
{
    public partial class TopBar : UserControl
    {
        public TopBar()
        {
            InitializeComponent();
        }

        private void DragWindow(object sender, MouseButtonEventArgs mouse)
        {
            if (mouse.LeftButton == MouseButtonState.Pressed)
                Window.GetWindow(this).DragMove();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).WindowState = WindowState.Minimized;
        }

        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            if (window.WindowState == WindowState.Normal)
                window.WindowState = WindowState.Maximized;
            else
                window.WindowState = WindowState.Normal;
        }
    }
}
