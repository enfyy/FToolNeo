using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NextFTool
{
    public class Spammer
    {
        private int delay_ms { get; set; }
        private int f_key { get; set; }
        private int f_bar; //not int
        private int hotkey; //also not int
        public bool isSpamming;
        public Process neuz;

        const UInt32 WM_KEYDOWN = 0x0100;
        const int VK_F1 = 0x70;

        public Spammer(Process neuz)
        {
            this.neuz = neuz;
        }

        public bool readyToSpam()
        {
            if(delay_ms != null && f_key != null)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public void startSpam()
        {
            WinAPI.PostMessage(neuz.MainWindowHandle, WM_KEYDOWN, VK_F1, 0);
        }

        public void stopSpam()
        {

        }

    }
}
