using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace NextFTool
{
    public class Spammer
    {
        public const int Min_delay = 50;
        public int delay_ms { get; set; }
        public int f_key { get; set; }
        public int? f_bar; //not int
        public int hotkey; //also not int
        public bool isSpamming;
        public Process neuz;
        MainWindow mainWindow;

        const UInt32 WM_KEYDOWN = 0x0100;
        const int VK_F1 = 0x70;

        public Spammer(Process neuz, MainWindow main)
        {
            this.neuz = neuz;
            this.mainWindow = main;
        }

        public void SetFBar(int num)
        {
            if (num == 0)
            {
                f_bar = null;
            }
            else
            {
                f_bar = num;
            }
        }

        public bool validDelay()
        {
            if(delay_ms >= Min_delay)
            {
                return true;
            } else
            {
                throw new InvalidDelayException("Select F-Key and Delay...");
                return false;
            }
        }

        public void stopSpam()
        {
            isSpamming = false;
        }

        public void startSpam()
        {
            if (validDelay())
            {
                isSpamming = true;
                Thread spammer = new Thread(spamLoop);
                mainWindow.AddActiveSpammer(spammer);
                spammer.Start();
            }        
        }

        public void spamLoop()
        {
            while (isSpamming)
            {
                if (f_bar.HasValue)
                {
                    WinAPI.PostMessage(neuz.MainWindowHandle, WM_KEYDOWN, f_bar.Value, 0); // go to skill-bar
                }               
                // maybe short delay ?
                WinAPI.PostMessage(neuz.MainWindowHandle, WM_KEYDOWN, f_key, 0); // press f-key                
                Thread.Sleep(delay_ms);
            }
        }

    }
}
