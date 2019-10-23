using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace NextFTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string pattern = @"\b\S*_";
        ProcessSelector neuzSelect = null;
        Dictionary<string, Spammer> spammers = new Dictionary<string, Spammer>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private string GetIndexFromName(string name)
        {
            return Regex.Split(name, pattern)[1];
        }

        private Label GetLabelFromIndex(string index)
        {
            return (Label) this.FindName("Label_Attached_To_" + index);
        }

        private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) { DragMove(); }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void F_Key_Select_1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ProcessSelector_Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (neuzSelect == null)
            {
                neuzSelect = new ProcessSelector(this, button.Name);
                neuzSelect.Show();
            } 
            else
            {
                neuzSelect.Activate();
            }            
        }

        public void SetSpammer(string buttonName, Spammer spammer, string spammerName)
        {
            string index = GetIndexFromName(buttonName);
            Label label = GetLabelFromIndex(index);
            label.Content += spammerName;
            label.Opacity = 1.0;
            spammers[index] = spammer;
        }

        public void closeProcessSelector()
        {
            neuzSelect = null;
        }

        private void Set_Hotkey_1_Click(object sender, RoutedEventArgs e)
        {
            HotkeyDialog hotkeyDialog = new HotkeyDialog();
            hotkeyDialog.Show();
        }

        private void Start_Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string index = GetIndexFromName(button.Name);
            
            if (spammers.ContainsKey(index)) 
            {
                Spammer spammer = spammers[index];
                spammer.f_key = GetFKeyFromInput(index);
                spammer.SetFBar(GetSkillBarFromInput(index));
                spammer.delay_ms = GetDelayFromInput(index);
                spammer.startSpam();
            } 
            else
            {
                //Error Dialog: No process set
            }
        }

        private int GetDelayFromInput(string index)
        {
            switch (index)
            {
                case "0": return Int32.Parse(Delay_Input_0.Text); //TODO: handle exceptions
                //..
                default: return 0;
            }
        }

        private int GetFKeyFromInput(string index)
        {
            string selected;
            switch (index)
            {
                case "-":
                    throw new NoFKeyException("Please Select a F-Key");
                case "0": 
                    selected = F_Key_Select_1.SelectedItem.ToString();
                    break;
                //...
                default: 
                    selected = "";
                    break;
            }
            selected.Replace("F", ""); //format string
            return Mapping.FKeyCodeMapping(selected);
        }

        private int GetSkillBarFromInput(string index)
        {
            string selected;
            switch (index)
            {
                case "-":
                    selected = "0";
                    break;
                case "0": 
                    selected = Skill_Bar_Select_1.SelectedItem.ToString();
                    break;
                //...
                default:
                    selected = null;
                    break;
            }
            return Mapping.NumberKeyCodeMapping(selected);
        }
    }
}
