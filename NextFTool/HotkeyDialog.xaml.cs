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

namespace NextFTool
{
    /// <summary>
    /// Interaction logic for HotkeyDialog.xaml
    /// </summary>
    public partial class HotkeyDialog : Window
    {
        public HotkeyDialog()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
