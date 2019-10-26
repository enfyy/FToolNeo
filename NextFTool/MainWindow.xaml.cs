using FontAwesome.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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
        Color pause_color = (Color)ColorConverter.ConvertFromString("#FF1F48"); // Color of the Stop Button
        Color play_color  = (Color)ColorConverter.ConvertFromString("#FF00FFAE"); // Color of the Play Button
        const string pattern = @"\b\S*_"; //Regex pattern for Getting index from UI-Component name
        ProcessSelector neuzSelect = null; //instance of the ProcessSelector
        HotkeyDialog hotkeyDialog = null; //instance of the HotkeyDialog
        Dictionary<string, Spammer> spammers = new Dictionary<string, Spammer>(); // dictionary of index -> spammer instance
        List<Thread> activeThreads = new List<Thread>(); // all the active threads
        IntPtr handle; //handle of the main window

        // constructor of the main window.
        public MainWindow()
        {
            InitializeComponent();
            handle = new WindowInteropHelper(this).Handle;
            RegisterDefaultHotkeys();           
        }

        // Registers the default hotkeys
        private void RegisterDefaultHotkeys()
        {
            WinAPI.RegisterHotKey(handle, id: 0, ModifierKeys.Shift, System.Windows.Forms.Keys.F1);
            WinAPI.RegisterHotKey(handle, id: 1, ModifierKeys.Shift, System.Windows.Forms.Keys.F2);
            WinAPI.RegisterHotKey(handle, id: 2, ModifierKeys.Shift, System.Windows.Forms.Keys.F3);
            WinAPI.RegisterHotKey(handle, id: 3, ModifierKeys.Shift, System.Windows.Forms.Keys.F4);
            WinAPI.RegisterHotKey(handle, id: 4, ModifierKeys.Shift, System.Windows.Forms.Keys.F5);
            WinAPI.RegisterHotKey(handle, id: 5, ModifierKeys.Shift, System.Windows.Forms.Keys.F6);
            WinAPI.RegisterHotKey(handle, id: 6, ModifierKeys.Shift, System.Windows.Forms.Keys.F7);
            WinAPI.RegisterHotKey(handle, id: 7, ModifierKeys.Shift, System.Windows.Forms.Keys.F8);
            WinAPI.RegisterHotKey(handle, id: 8, ModifierKeys.Shift, System.Windows.Forms.Keys.F9);
            ComponentDispatcher.ThreadPreprocessMessage += ThreadPreprocessMethod;
        }

        // unregisters all hotkeys
        private void UnRegisterHotkeys()
        {            
            WinAPI.UnregisterHotKey(handle, id: 0);
            WinAPI.UnregisterHotKey(handle, id: 1);
            WinAPI.UnregisterHotKey(handle, id: 2);
            WinAPI.UnregisterHotKey(handle, id: 3);
            WinAPI.UnregisterHotKey(handle, id: 4);
            WinAPI.UnregisterHotKey(handle, id: 5);
            WinAPI.UnregisterHotKey(handle, id: 6);
            WinAPI.UnregisterHotKey(handle, id: 7);
            WinAPI.UnregisterHotKey(handle, id: 8);
        }

        //  unregistered a hotkey indicated by its ID.
        public void UnRegisterHotkey(int id)
        {
            WinAPI.UnregisterHotKey(handle, id: id);
        }

        //fires all the time and checks for hotkey press
        private void ThreadPreprocessMethod(ref MSG msg, ref bool handled)
        {
            const int WmHotKey = 786;
            if (handled || msg.message != WmHotKey)
                return;

            var id = (int)msg.wParam;
            Start_Stop_Action(id.ToString());       
        }

        // Stops all running threads
        public void StopThreads()
        {
            foreach (Thread t in activeThreads)
            {
                t.Abort();
            }
        }

        // retrieves the index by the name of a UI-Component
        private string GetIndexFromName(string name)
        {
            return Regex.Split(name, pattern)[1];
        }

        // retrieves the 'Attached_To'- Label by index
        private Label GetLabelFromIndex(string index)
        {
            return (Label) this.FindName("Label_Attached_To_" + index);
        }

        // On Mouse Down Action for the main window -> makes the window drag-able
        private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) { DragMove(); }
        }

        // Action of the Close-Button -> closes the main window.
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            StopThreads();
            UnRegisterHotkeys();
            Close();
        }


        // Adds a Thread to the Local Thread List
        public void AddActiveSpammer(Thread t)
        {
            activeThreads.Add(t);
        }

        // Minimize Button Action -> minimizes the window
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        // Action of the ProcessSelector Button -> opens a ProcessSelector window
        private void ProcessSelector_Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string index = GetIndexFromName(button.Name);
            if (neuzSelect == null)
            {
                neuzSelect = new ProcessSelector(this, index);
                neuzSelect.Show();
            }
            else if (!string.Equals(neuzSelect.index, index))
            {
                neuzSelect.Close();
                neuzSelect = new ProcessSelector(this, index);
                neuzSelect.Show();
            }
            else
            {
                neuzSelect.Activate();
            }            
        }

        // attaches the spammer to the correct index
        public void AttachSpammer(string index, Spammer spammer, string spammerName)
        {
            Label label = GetLabelFromIndex(index);
            label.Content += spammerName;
            label.Opacity = 1.0;
            spammers[index] = spammer;
        }

        // resets the local variable of the processSelector
        public void closeProcessSelector()
        {
            neuzSelect = null;
        }

        // resets the local variable of the hotkeydialog
        public void closeHotkeyDialog()
        {
            hotkeyDialog = null;
        }

        // Action of the Hotkey Button -> opens the hotkeydialog
        private void Set_Hotkey_Click(object sender, RoutedEventArgs e)
        {  
            Button button = sender as Button;
            string index = GetIndexFromName(button.Name);
            if (hotkeyDialog == null) //No window open
            {
                hotkeyDialog = new HotkeyDialog(index);
                hotkeyDialog.Show();
            }
            else if (!string.Equals(hotkeyDialog.index, index)) //window of other index is open
            {
                hotkeyDialog.Close();
                hotkeyDialog = new HotkeyDialog(index);
                hotkeyDialog.Show();

            }
            else //window with same index is open
            {
                hotkeyDialog.Activate();
            }
        }

        // Action of the Start/Stop Button -> starts or stops the spammer
        private void Start_Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string index = GetIndexFromName(button.Name);
            Start_Stop_Action(index);
        }


        // Action that is run when start stop button or hotkey is pressed
        private void Start_Stop_Action(string index)
        {
            MessageBoxCustom.Close_current();
            if (spammers.ContainsKey(index))
            {
                Spammer spammer = spammers[index];
                if (spammer.isSpamming)
                {
                    DeactivateSpammer(spammer, index);
                }
                else
                {
                    try
                    {
                        spammer.f_key = GetFKeyFromInput(index);
                    }
                    catch (NoFKeyException)
                    {
                        new MessageBoxCustom("Please select a F-Key.");
                        return;
                    }
                    spammer.SetFBar(GetSkillBarFromInput(index));
                    spammer.delay_ms = GetDelayFromInput(index);
                    ActivateSpammer(spammer, index);
                }
            }
            else
            {
                new MessageBoxCustom("Please select a Process.");
            }
        }

        // deactivates the spammer
        private void DeactivateSpammer(Spammer spammer, string index)
        {
            spammer.stopSpam();
            ToggleStartIcon(index);
        }

        // activates the spammer
        private void ActivateSpammer(Spammer spammer, string index)
        {
            try
            {                
                spammer.startSpam();                
            }
            catch (InvalidDelayException)
            {
                new MessageBoxCustom("Make sure to set a Delay ≥ " + Spammer.Min_delay + " ms.");
                return;
            }                       
            ToggleStartIcon(index);
        }

        // retrieve Delay Setting from the Input indicated by the index
        private int GetDelayFromInput(string index)
        {
            string value;
            int delay;
            TextBox input;
            switch (index)
            {
                case "0":
                    input = Delay_Input_0;
                    break;
                case "1":
                    input = Delay_Input_1;
                    break;
                case "2":
                    input = Delay_Input_2;
                    break;
                case "3":
                    input = Delay_Input_3;
                    break;
                case "4":
                    input = Delay_Input_4;
                    break;
                case "5":
                    input = Delay_Input_5;
                    break;
                default: return Spammer.Min_delay;
            }
            value = input.Text;
            if (value == string.Empty)
            {
                new MessageBoxCustom("Make sure to set a Delay ≥ " + Spammer.Min_delay + " ms.");
                delay = Spammer.Min_delay;
                input.Text = Spammer.Min_delay.ToString();
                
            } else
            {
                delay = Int32.Parse(value);
            }
            if (delay < Spammer.Min_delay )
            {
                new MessageBoxCustom("Make sure to set a Delay ≥ " + Spammer.Min_delay + " ms.");
                delay = Spammer.Min_delay;
                input.Text = Spammer.Min_delay.ToString();
            }
            return delay;
        }

        // retrieve F-Key Setting from the Input indicated by the index
        private int GetFKeyFromInput(string index)
        {
            string selected;
            switch (index)
            {
                case "-":
                    throw new NoFKeyException("Please Select a F-Key");
                case "0": 
                    selected = F_Key_Select_0.Text;
                    break;
                case "1":
                    selected = F_Key_Select_1.Text;
                    break;
                case "2":
                    selected = F_Key_Select_2.Text;
                    break;
                case "3":
                    selected = F_Key_Select_3.Text;
                    break;
                case "4":
                    selected = F_Key_Select_4.Text;
                    break;
                case "5":
                    selected = F_Key_Select_5.Text;
                    break;                
                default:
                    throw new NoFKeyException("Please Select a F-Key");                    
            }
            selected = selected.Replace("F", ""); //format string
            return Mapping.FKeyCodeMapping(selected);            
        }

        // retrieve Skill-Bar Setting from the Input indicated by the index
        private int GetSkillBarFromInput(string index)
        {
            string selected;
            switch (index)
            {
                case "-":
                    selected = "0";
                    break;
                case "0":
                    selected = Skill_Bar_Select_0.Text;
                    break;
                case "1":
                    selected = Skill_Bar_Select_1.Text;
                    break;
                case "2":
                    selected = Skill_Bar_Select_2.Text;
                    break;
                case "3":
                    selected = Skill_Bar_Select_3.Text;
                    break;
                case "4":
                    selected = Skill_Bar_Select_4.Text;
                    break;
                case "5":
                    selected = Skill_Bar_Select_5.Text;
                    break;
                default:
                    selected = null;
                    break;
            }
            return Mapping.NumberKeyCodeMapping(selected);
        }

        // Retrieve ImageAwesome Object by index
        private ImageAwesome GetStartImageFromIndex(string index)
        {           
            switch (index)
            {
                case "0":
                    return Start_Icon_0;
                case "1":
                    return Start_Icon_1;
                case "2":
                    return Start_Icon_2;
                case "3":
                    return Start_Icon_3;
                case "4":
                    return Start_Icon_4;
                case "5":
                    return Start_Icon_5;
                default:
                    throw new Exception("There is no Start/Stop Icon with that index.");
            }           
        }

        // Toggle Icon of the Start/Stop Button
        private void ToggleStartIcon(string index)
        {
            ImageAwesome image = GetStartImageFromIndex(index);
            if (image.Icon == FontAwesomeIcon.Play)
            {
                image.Icon = FontAwesomeIcon.Pause;
                image.Foreground = new SolidColorBrush(pause_color);
            } 
            else
            {
                image.Icon = FontAwesomeIcon.Play;
                image.Foreground = new SolidColorBrush(play_color);
            }
        }

        // The Action that runs when the input of the delay textbox is changed -> restricts input to numbers only with 7 digits
        private void Delay_Input_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            string text = textbox.Text;
            Regex not_numeric = new Regex("[^0-9]");
            if (text.Length > 7)
            {
                text = text.Substring(0,7);
            }
            text = not_numeric.Replace(text, "");
            textbox.Text = text;
        }
    }
}
