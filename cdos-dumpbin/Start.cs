using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cdos_dumpbin{
    class Start{

        static void Main(string[] args){

            for (int i = 0; i+1 < args.Length; ++i){
                switch (args[i].Trim()) {
                    case "-d": case "-dll": case "-dlls": case "-dependents":
                        ConfUtils.dependents(args[i+1]);
                        break;
                    case "-cmp": case "-1vsN":
                        ConfUtils.cmp1VsN(args[i+1]);
                        break;
                    default: break;
                }
            }

            //ConfUtils.dependents(@"C:\Program Files\Microsoft Office");
        }
    }
}