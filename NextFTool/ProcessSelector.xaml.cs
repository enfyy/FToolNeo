using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Interaction logic for ProcessSelector.xaml
    /// </summary>
    public partial class ProcessSelector : Window
    {
        MainWindow mainWindow = null;
        public string index; //indicates which of the process selector buttons was clicked.
        public ProcessSelector(MainWindow main, string index)
        {
            InitializeComponent();
            PopulateProcessList();
            mainWindow = main;
            this.index = index;
        }

        private void ProcessSelector_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) { DragMove(); }
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            mainWindow.closeProcessSelector();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            PopulateProcessList();
        }

        private void PopulateProcessList()
        {
            List<Process> neuzList = GetProcesses().ToList();
            ProcessList.ItemsSource = neuzList;
            ProcessList.DisplayMemberPath = "MainWindowTitle";
        }

        private Process[] GetProcesses()
        {
            String process_name = File.ReadAllText("process_name.txt");
            Process[] neuzProcesses = Process.GetProcessesByName(process_name); //TODO: maybe get from .ini file
            return neuzProcesses;
        }

        private void Select_Process_Click(object sender, RoutedEventArgs e)
        {
            if (ProcessList.SelectedItem != null)
            {
                Process neuz = ProcessList.SelectedItem as Process;
                Spammer spam = new Spammer(neuz, mainWindow);
                mainWindow.AttachSpammer(index, spam, neuz.MainWindowTitle);
                Close();
                mainWindow.closeProcessSelector();
            } 
            else
            {
                //Nothing happens
            }
        }

        private void Jump_to_Process_Click(object sender, RoutedEventArgs e)
        {
            if (ProcessList.SelectedItem != null)
            {
                Process neuz = ProcessList.SelectedItem as Process;
                IntPtr handle = neuz.MainWindowHandle;

                if (WinAPI.IsIconic(handle))
                {
                    WinAPI.ShowWindow(handle, 9);
                    WinAPI.SetForegroundWindow(handle);
                }
                else
                {
                    WinAPI.ShowWindow(handle, 6); //minimize it first lmao. i hate this but it works :)
                    WinAPI.ShowWindow(handle, 9);
                    WinAPI.SetForegroundWindow(handle);
                }
            }
            else
            {
                //Nothing happens.
            }
            
        }
    }
}
