using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PChronoz.Views
{
    public partial class FilterWindow : Window
    {
        public string InputText { get; private set; }

        public FilterWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) => InputTextBox.Focus();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            InputText = InputTextBox.Text;
            DialogResult = true;
        }

        private void ClickEnter(object sender, KeyEventArgs f)
        {
            if (f.Key == Key.Enter)
            {
                InputText = InputTextBox.Text;
                DialogResult = true;
            }
        }
    }
}
