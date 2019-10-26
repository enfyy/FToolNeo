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
    /// Interaction logic for MessageBoxCustom.xaml
    /// </summary>
    public partial class MessageBoxCustom : Window
    {
        public static MessageBoxCustom obj = null;

        public MessageBoxCustom(string message)
        {
            if (obj != null)
            {
                obj.Close();
            }
            InitializeComponent();
            obj = this;
            Label_Custom_Message.Content = message;
            Show();
            Activate();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public static void Close_current()
        {
            if(obj != null)
            {
                obj.Close();
            }            
        }

        private void MessageBoxCustom_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) { DragMove(); }
        }
    }
}
