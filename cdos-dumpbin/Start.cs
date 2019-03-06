using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cdos_dumpbin{
    class Start{

        static void Main(string[] args){
            string folder = @"C:\Program Files\Microsoft Office";
            string folder2 = @"./conf_cmp_1vsN.ini";
            //ConfUtils.dependents(folder, true);
            ConfUtils.cmp1VsN(folder2, true);
            return;
            for (int i = 0; i+1 < args.Length; ++i){
                switch (args[i].Trim()) {
                    case "-dll": 
                        ConfUtils.dependents(args[i+1], false);
                        break;
                    case "-dlls":
                        ConfUtils.dependents(args[i + 1], true);
                        break;
                    /*case "-cmp": 
                        ConfUtils.cmp1VsN(args[i+1], false);
                        break;
                    case "-cmps":
                        ConfUtils.cmp1VsN(args[i + 1], true);
                        break;*/
                    default: break;
                }
            }

            //ConfUtils.dependents(@"C:\Program Files\Microsoft Office");
        }
    }
}