using System;
using System.Windows;

namespace PChronoz.Views
{
    public partial class NumberInputWindow : Window
    {
        public string InputValue { get; private set; }

        public NumberInputWindow()
        {
            InitializeComponent();
            Loaded += NumberInputWindow_Loaded;
        }

        private void NumberInputWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ValueTextBox.Focus();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue = ValueTextBox.Text;
            DialogResult = true;
        }
    }
}