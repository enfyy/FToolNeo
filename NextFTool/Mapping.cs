using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextFTool
{
    class Mapping
    {
        public static int FKeyCodeMapping(string f_key)
        {
            switch (f_key)
            {
                case "-": throw new NoFKeyException("selected F-key has no mapping.");
                case "1": return 0x70;
                case "2": return 0x71;
                case "3": return 0x72;
                case "4": return 0x73;
                case "5": return 0x74;
                case "6": return 0x75;
                case "7": return 0x76;
                case "8": return 0x77;
                case "9": return 0x78;
                default: throw new NoFKeyException("selected F-key has no mapping.");
            }
        }

        public static int NumberKeyCodeMapping(string num)
        {
            switch (num)
            {
                case "0": return 0;
                case "1": return 0x31;
                case "2": return 0x32;
                case "3": return 0x33;
                case "4": return 0x34;
                case "5": return 0x35;
                case "6": return 0x36;
                case "7": return 0x37;
                case "8": return 0x38;
                case "9": return 0x39;
                default: throw new Exception("selected Skill-Bar has no mapping.");
            }
        }

    }
}
